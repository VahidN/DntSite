using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace DntSite.Web.Features.SiteBackup.Services;

public class UploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    ILogger<UploadBackupService> logger) : IUploadBackupService
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

        var telegramBotClient = new TelegramBotClient(telegramBackupGroup.AccessToken);

        var totalParts = (int)Math.Ceiling((double)new FileInfo(filePath).Length / MaxPartSize);

        var tempDirectory = GetTempDirectory();

        await UploadBackupPartsAsync(telegramBotClient, telegramBackupGroup.ChatId, filePath, totalParts, tempDirectory,
            cancellationToken);

        await Task.Delay(_delay, cancellationToken);

        await SendInstructionMessageAsync(telegramBotClient, telegramBackupGroup.ChatId, totalParts, cancellationToken);
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

    private async Task UploadBackupPartsAsync(TelegramBotClient telegramBotClient,
        string chatId,
        string filePath,
        int totalParts,
        string tempDirectory,
        CancellationToken cancellationToken)
    {
        var originalFileName = filePath.GetFileName();
        await using var sourceStream = File.OpenRead(filePath);

        for (var i = 0; i < totalParts; i++)
        {
            var partNumber = i + 1;

            var outputPath = tempDirectory.SafePathCombine(string.Create(CultureInfo.InvariantCulture,
                $"backup_{originalFileName}_{partNumber:00}.part"));

            await SplitAndUploadAsync(telegramBotClient, chatId, sourceStream, outputPath, partNumber, totalParts,
                originalFileName, cancellationToken);

            await Task.Delay(_delay, cancellationToken);

            outputPath.TryDeleteFile();
        }
    }

    private string GetTempDirectory()
    {
        var tempDirectory = appFoldersService.BackupFolderPath.SafePathCombine("Temp");

        if (Directory.Exists(tempDirectory))
        {
            Directory.Delete(tempDirectory, recursive: true);
        }

        tempDirectory.TryCreateDirectory();

        return tempDirectory;
    }

    private static async Task SplitAndUploadAsync(TelegramBotClient botClient,
        string chatId,
        FileStream sourceStream,
        string outputPath,
        int partNumber,
        int totalParts,
        string originalFileName,
        CancellationToken cancellationToken)
    {
        var uploadedSize = await SplitStreamAsync(sourceStream, outputPath, cancellationToken);

        await using var content = File.OpenRead(outputPath);

        await botClient.SendDocumentAsync(chatId, new InputOnlineFile(content, outputPath.GetFileName()),
            caption:
            $"🔹 بخش {partNumber.ToPersianNumbers()} از {totalParts.ToPersianNumbers()}\n📁 فایل: {originalFileName}\n📏 حجم بخش: {uploadedSize.ToFormattedFileSize()}",
            disableContentTypeDetection: false, disableNotification: false, cancellationToken: cancellationToken);
    }

    private static async Task<long> SplitStreamAsync(FileStream sourceStream,
        string outputPath,
        CancellationToken cancellationToken)
    {
        await using var outputFileStream = outputPath.CreateAsyncFileStream(FileMode.Create, FileAccess.Write);

        long bytesCopied = 0;
        long bytesToCopy = MaxPartSize;

        var memoryBuffer = new byte[81920];

        while (bytesToCopy > 0)
        {
            var availableBytes = (int)Math.Min(memoryBuffer.Length, bytesToCopy);

            var bytesRead = await sourceStream.ReadAsync(new Memory<byte>(memoryBuffer, start: 0, availableBytes),
                cancellationToken);

            if (bytesRead == 0)
            {
                break;
            }

            await outputFileStream.WriteAsync(new ReadOnlyMemory<byte>(memoryBuffer, start: 0, bytesRead),
                cancellationToken);

            bytesCopied += bytesRead;
            bytesToCopy -= bytesRead;
        }

        outputFileStream.Close();
        sourceStream.Seek(sourceStream.Position - bytesCopied, SeekOrigin.Current);

        return bytesCopied;
    }

    private static async Task SendInstructionMessageAsync(TelegramBotClient botClient,
        string chatId,
        int totalParts,
        CancellationToken cancellationToken)
    {
        var instruction = $"""
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
                           """;

        await botClient.SendTextMessageAsync(chatId, instruction.Trim(), ParseMode.Markdown,
            cancellationToken: cancellationToken);
    }
}
