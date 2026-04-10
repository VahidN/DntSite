using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace DntSite.Web.Features.SiteBackup.Services;

public class TelegramUploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    ILogger<TelegramUploadBackupService> logger) : ITelegramUploadBackupService
{
    private const int MaxPartSize = 49 * 1024 * 1024; // 49 مگابایت
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 15);

    public async Task UploadSiteBackupFileToTelegramAsync(string filePath,
        CancellationToken cancellationToken = default)
    {
        var telegramBackupGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).TelegramBackupGroup;

        if (!telegramBackupGroup.IsActive || telegramBackupGroup.AccessToken.IsEmpty() ||
            telegramBackupGroup.ChatId.IsEmpty())
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "`TelegramBackupGroup` is not active or set.");
            }

            return;
        }

        await UploadFileToTelegramAsync(filePath, telegramBackupGroup.AccessToken, telegramBackupGroup.ChatId,
            cancellationToken);
    }

    public async Task UploadFileToTelegramAsync(string filePath,
        string accessToken,
        string chatId,
        CancellationToken cancellationToken = default)
    {
        if (!filePath.FileExists())
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "Backup file: `{File}` not found.", filePath);
            }

            return;
        }

        if (!NetworkExtensions.IsConnectedToInternet(_delay))
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "There is no internet connection to run UploadBackupService.");
            }

            return;
        }

        var originalFileName = filePath.GetFileName();
        var tempDirectory = GetTempDirectory();

        var partPaths = await filePath.SplitFileToMultiplePartsAsync(tempDirectory, partsInfo =>
        {
            var totalWidth = partsInfo.TotalParts.CountDigits();
            var number = partsInfo.PartNumber.ToStringPadLeft(totalWidth);

            return string.Create(CultureInfo.InvariantCulture, $"{originalFileName}_{number}.part");
        }, MaxPartSize, cancellationToken);

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
                                                              3. نام فایل‌ها باید به ترتیب باشند: {originalFileName}_1.part، {originalFileName}_2.part، و غیره
                                                              4. با ابزار ترکیب، فایل‌ها را به هم بچسبانید:

                                                              **با خط فرمان (ویندوز):**
                                                              type {originalFileName}_*.part > {originalFileName}

                                                              **با خط فرمان (لینوکس/مک):**
                                                              cat {originalFileName}_*.part > {originalFileName}

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
