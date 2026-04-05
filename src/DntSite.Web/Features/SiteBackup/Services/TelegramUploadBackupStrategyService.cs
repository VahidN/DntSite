using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace DntSite.Web.Features.SiteBackup.Services;

public class TelegramUploadBackupStrategyService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    ILogger<TelegramUploadBackupStrategyService> logger) : IUploadBackupService
{
    private const int MaxPartSize = 49 * 1024 * 1024; // 49 مگابایت
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 15);

    public async Task UploadToHostAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var telegramBackupGroup = await ValidateAndGetTelegramBackupGroupAsync(filePath);

        if (telegramBackupGroup?.AccessToken is null || telegramBackupGroup.ChatId is null)
        {
            return;
        }

        await UploadBackupPartsAsync(telegramBackupGroup.AccessToken, telegramBackupGroup.ChatId, filePath,
            cancellationToken);
    }

    private async Task<TelegramBackupGroup?> ValidateAndGetTelegramBackupGroupAsync(string? filePath)
    {
        if (!filePath.FileExists())
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "Backup file: `{File}` not found.", filePath);
            }

            return null;
        }

        if (!NetworkExtensions.IsConnectedToInternet(_delay))
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "There is no internet connection to run UploadBackupService.");
            }

            return null;
        }

        var telegramBackupGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).TelegramBackupGroup;

        if (telegramBackupGroup.IsActive && !telegramBackupGroup.AccessToken.IsEmpty() &&
            !telegramBackupGroup.ChatId.IsEmpty())
        {
            return telegramBackupGroup;
        }

        if (logger.IsEnabled(LogLevel.Critical))
        {
            logger.LogCritical(message: "`TelegramBackupGroup` is not active or set.");
        }

        return null;
    }

    private async Task UploadBackupPartsAsync(string accessToken,
        string chatId,
        string filePath,
        CancellationToken cancellationToken)
    {
        var originalFileName = filePath.GetFileName();
        var tempDirectory = GetTempDirectory();

        var partPaths = await filePath.SplitFileToMultiplePartsAsync(tempDirectory,
            partNumber => string.Create(CultureInfo.InvariantCulture,
                $"backup_{originalFileName}_{partNumber:00}.part"), MaxPartSize, cancellationToken);

        var totalParts = partPaths.Count;
        var partNumber = 1;

        var telegramBotClient = new TelegramBotClient(accessToken);

        foreach (var partPath in partPaths)
        {
            var uploadedSize = new FileInfo(partPath).Length;

            await using (var content = File.OpenRead(partPath))
            {
                await telegramBotClient.SendDocumentAsync(chatId, new InputOnlineFile(content, partPath.GetFileName()),
                    caption: $"""
                              🔹 بخش {partNumber.ToPersianNumbers()} از {totalParts.ToPersianNumbers()}
                              📁 فایل: {originalFileName}
                              📏 حجم بخش: {uploadedSize.ToFormattedFileSize()}
                              """, disableContentTypeDetection: false, disableNotification: false,
                    cancellationToken: cancellationToken);
            }

            partNumber++;

            await Task.Delay(_delay, cancellationToken);
            partPath.TryDeleteFile(logger);
        }

        await Task.Delay(_delay, cancellationToken);

        await telegramBotClient.SendTextMessageAsync(chatId, $"""
                                                              📦 **راهنمای دریافت فایل بک‌آپ تاریخ {DateTime.IranNowUtc.Persian.Text.LongDateTime} **

                                                              ✅ فایل بک‌آپ به {totalParts.ToPersianNumbers()} بخش تقسیم و ارسال شد.

                                                              🔹 **برای دریافت کل فایل:**
                                                              1. روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
                                                              2. آن‌ها را در یک پوشه قرار دهید
                                                              3. نام فایل‌ها باید به ترتیب باشند: backup_1.part، backup_2.part، و غیره
                                                              4. با ابزار ترکیب، فایل‌ها را به هم بچسبانید:

                                                              **با خط فرمان (ویندوز):**
                                                              type backup_*.part > backup_combined.zip

                                                              **با خط فرمان (لینوکس/مک):**
                                                              cat backup_*.part > backup_combined.zip

                                                              ⚠️ **نکته:** حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
                                                              """.Trim(), ParseMode.Markdown,
            cancellationToken: cancellationToken);
    }

    private string GetTempDirectory()
    {
        var tempDirectory = appFoldersService.BackupFolderPath.SafePathCombine("Temp");
        tempDirectory.TryDeleteDirectory(logger);
        tempDirectory.TryCreateDirectory();

        return tempDirectory!;
    }
}
