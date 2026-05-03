using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Models;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace DntSite.Web.Features.SiteBackup.Services;

public class TelegramUploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppFoldersService appFoldersService,
    IHttpClientFactory httpClientFactory,
    ILogger<TelegramUploadBackupService> logger) : ITelegramUploadBackupService
{
    private const int MaxPartSizeInBytes = 45 * 1024 * 1024; // 45 مگابایت
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 15);

    public async Task UploadSiteBackupFileToTelegramAsync(string filePath,
        FileSplitterType fileSplitterType,
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
            fileSplitterType, cancellationToken);
    }

    public async Task UploadSiteEPubFileToTelegramAsync(string filePath,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default)
    {
        var telegramEPubGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).TelegramEPubGroup;

        if (!telegramEPubGroup.IsActive || telegramEPubGroup.AccessToken.IsEmpty() ||
            telegramEPubGroup.ChatId.IsEmpty())
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "`TelegramEPubGroup` is not active or set.");
            }

            return;
        }

        await UploadFileToTelegramAsync(filePath, telegramEPubGroup.AccessToken, telegramEPubGroup.ChatId,
            fileSplitterType, cancellationToken);
    }

    public async Task UploadFileToTelegramAsync(string filePath,
        string accessToken,
        string chatId,
        FileSplitterType fileSplitterType,
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

        var partPaths = await GetPartPathsAsync(filePath, fileSplitterType, originalFileName, cancellationToken);

        using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);
        var telegramBotClient = new TelegramBotClient(accessToken, httpClient, cancellationToken);

        await UploadPartsAsync(telegramBotClient, chatId, partPaths, originalFileName, cancellationToken);

        await SendMessageAsync(telegramBotClient, chatId, fileSplitterType, partPaths?.Count ?? 0, originalFileName,
            cancellationToken);
    }

    public async Task UploadSiteFolderContentsToTelegramAsync(string folderPath,
        string outputZipFilePath,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default)
    {
        var telegramEPubGroup = (await cachedAppSettingsProvider.GetAppSettingsAsync()).TelegramEPubGroup;

        if (!telegramEPubGroup.IsActive || telegramEPubGroup.AccessToken.IsEmpty() ||
            telegramEPubGroup.ChatId.IsEmpty())
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "`TelegramEPubGroup` is not active or set.");
            }

            return;
        }

        await UploadFolderContentsToTelegramAsync(folderPath, outputZipFilePath, telegramEPubGroup.AccessToken,
            telegramEPubGroup.ChatId, fileSplitterType, cancellationToken);
    }

    public async Task UploadFolderContentsToTelegramAsync(string folderPath,
        string outputZipFilePath,
        string accessToken,
        string chatId,
        FileSplitterType fileSplitterType,
        CancellationToken cancellationToken = default)
    {
        if (!folderPath.DirectoryExists())
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(message: "Backup folder: `{Dir}` not found.", folderPath);
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

        var originalFileName = new DirectoryInfo(folderPath).Name;
        var tempDirectory = GetTempDirectory();
        IList<string>? partPaths = [];

        switch (fileSplitterType)
        {
            case FileSplitterType.NormalFileSplit:

                folderPath.CompressFolderToZipFile(outputZipFilePath);
                await Task.Delay(_delay, cancellationToken);

                partPaths = await outputZipFilePath.SplitFileToMultiplePartsAsync(tempDirectory, partsInfo =>
                {
                    var totalWidth = partsInfo.TotalParts.CountDigits();
                    var number = partsInfo.PartNumber.ToStringPadLeft(totalWidth);

                    return string.Create(CultureInfo.InvariantCulture, $"{originalFileName}_{number}.part");
                }, MaxPartSizeInBytes, cancellationToken);

                break;
            case FileSplitterType.ZipFileAndSplit:
                partPaths = await folderPath.ZipAndSplitFileToMultiplePartsAsync(
                    MaxPartSizeInBytes.ToMegabytes(FileSizeUnit.Byte), tempDirectory, overwriteExistingFiles: true,
                    logger, cancellationToken);

                break;
        }

        using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);
        var telegramBotClient = new TelegramBotClient(accessToken, httpClient, cancellationToken);

        await UploadPartsAsync(telegramBotClient, chatId, partPaths, originalFileName, cancellationToken);

        await SendMessageAsync(telegramBotClient, chatId, fileSplitterType, partPaths?.Count ?? 0, originalFileName,
            cancellationToken);
    }

    private async Task UploadPartsAsync(TelegramBotClient telegramBotClient,
        string chatId,
        IList<string>? partPaths,
        string? originalFileName,
        CancellationToken cancellationToken)
    {
        var totalParts = partPaths?.Count ?? 0;

        if (partPaths is null || totalParts == 0)
        {
            return;
        }

        var partNumber = 1;

        foreach (var partPath in partPaths)
        {
            var uploadedSize = new FileInfo(partPath).Length;

            await using (var content = File.OpenRead(partPath))
            {
                await telegramBotClient.SendDocument(chatId, new InputFileStream(content, partPath.GetFileName()), $"""
                     🔹 بخش {partNumber.ToPersianNumbers()} از {totalParts.ToPersianNumbers()}
                     📁 فایل: {originalFileName}
                     📏 حجم بخش: {uploadedSize.ToFormattedFileSize()}
                     """, cancellationToken: cancellationToken);
            }

            partNumber++;

            await Task.Delay(_delay, cancellationToken);
            partPath.TryDeleteFile(logger);
        }

        await Task.Delay(_delay, cancellationToken);
    }

    private async Task<IList<string>?> GetPartPathsAsync(string filePath,
        FileSplitterType fileSplitterType,
        string? originalFileName,
        CancellationToken cancellationToken)
    {
        var tempDirectory = GetTempDirectory();

        switch (fileSplitterType)
        {
            case FileSplitterType.NormalFileSplit:
                var backupZipFilePath = $"{filePath}.zip";
                backupZipFilePath.CompressFilesToZipFile(filePath);
                await Task.Delay(_delay, cancellationToken);

                return await backupZipFilePath.SplitFileToMultiplePartsAsync(tempDirectory, partsInfo =>
                {
                    var totalWidth = partsInfo.TotalParts.CountDigits();
                    var number = partsInfo.PartNumber.ToStringPadLeft(totalWidth);

                    return string.Create(CultureInfo.InvariantCulture, $"{originalFileName}_{number}.part");
                }, MaxPartSizeInBytes, cancellationToken);
            case FileSplitterType.ZipFileAndSplit:
                return await filePath.ZipAndSplitFileToMultiplePartsAsync(
                    MaxPartSizeInBytes.ToMegabytes(FileSizeUnit.Byte), tempDirectory, overwriteExistingFiles: true,
                    logger, cancellationToken);
            default:
                return [];
        }
    }

    private static async Task SendMessageAsync(TelegramBotClient telegramBotClient,
        string chatId,
        FileSplitterType fileSplitterType,
        int totalParts,
        string? originalFileName,
        CancellationToken cancellationToken)
    {
        var text = fileSplitterType switch
        {
            FileSplitterType.NormalFileSplit => $"""
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
                                                 """.Trim(),
            FileSplitterType.ZipFileAndSplit => $"""
                                                 📦 **راهنمای دریافت فایل بک‌آپ تاریخ {DateTime.IranNowUtc.Persian.Text.LongDateTime} **

                                                 ✅ فایل بک‌آپ به {totalParts.ToPersianNumbers()} بخش تقسیم و ارسال شد.

                                                 🔹 **برای دریافت کل فایل:**
                                                 1. روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
                                                 2. تمام قطعات فایل (مانند `{originalFileName}.z01`، `{originalFileName}.z02` و `{originalFileName}.zip`) را در **یک پوشه** قرار دهید.
                                                 3. روی فایل `{originalFileName}.zip` (یا `{originalFileName}.z01`) **کلیک راست** کنید.
                                                 4. از منوی `7-Zip` گزینه‌ی **`Extract Here`** یا **`Extract to "{originalFileName}\"`** را انتخاب کنید.
                                                 5. ابزار به صورت خودکار تمام قطعات را شناسایی کرده و فایل اصلی را خارج می‌سازد.

                                                 ⚠️ **نکته:** حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
                                                 """.Trim(),
            _ => ""
        };

        await telegramBotClient.SendMessage(chatId, text, ParseMode.Markdown, cancellationToken: cancellationToken);
    }

    private string GetTempDirectory()
    {
        var tempDirectory = appFoldersService.BackupFolderPath.SafePathCombine("Temp");
        tempDirectory.TryDeleteDirectory(logger);
        tempDirectory.TryCreateDirectory();

        return tempDirectory!;
    }
}
