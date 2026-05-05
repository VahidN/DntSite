using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace DntSite.Web.Features.SiteBackup.Services;

public class TelegramUploadBackupService(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IHttpClientFactory httpClientFactory,
    ILogger<TelegramUploadBackupService> logger) : ITelegramUploadBackupService
{
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(seconds: 15);

    public async Task UploadSiteBackupFileToTelegramAsync(IList<string>? partPaths,
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

        if (partPaths?.Count == 0)
        {
            return;
        }

        using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);
        var telegramBotClient = new TelegramBotClient(telegramBackupGroup.AccessToken, httpClient, cancellationToken);

        await UploadPartsAsync(telegramBotClient, telegramBackupGroup.ChatId, partPaths, cancellationToken);
        await SendMessageAsync(telegramBotClient, telegramBackupGroup.ChatId, partPaths, cancellationToken);
    }

    public async Task UploadSiteEPubFileToTelegramAsync(IList<string>? partPaths,
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

        if (partPaths?.Count == 0)
        {
            return;
        }

        using var httpClient = httpClientFactory.CreateClient(NamedHttpClient.BaseHttpClient);
        var telegramBotClient = new TelegramBotClient(telegramEPubGroup.AccessToken, httpClient, cancellationToken);

        await UploadPartsAsync(telegramBotClient, telegramEPubGroup.ChatId, partPaths, cancellationToken);
        await SendMessageAsync(telegramBotClient, telegramEPubGroup.ChatId, partPaths, cancellationToken);
    }

    private async Task UploadPartsAsync(TelegramBotClient telegramBotClient,
        string chatId,
        IList<string>? partPaths,
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
                     📏 حجم بخش: {uploadedSize.ToFormattedFileSize()}
                     """, cancellationToken: cancellationToken);
            }

            partNumber++;

            await Task.Delay(_delay, cancellationToken);
        }

        await Task.Delay(_delay, cancellationToken);
    }

    private static async Task SendMessageAsync(TelegramBotClient telegramBotClient,
        string chatId,
        IList<string>? partPaths,
        CancellationToken cancellationToken)
    {
        var totalParts = partPaths?.Count ?? 0;

        if (partPaths is null || totalParts == 0)
        {
            return;
        }

        var hasParts = partPaths.Any(file => file.EndsWith(value: ".part", StringComparison.OrdinalIgnoreCase));

        var text = hasParts
            ? $"""
               📦 **راهنمای دریافت فایل بک‌آپ تاریخ {DateTime.IranNowUtc.Persian.Text.LongDateTime} **

               ✅ فایل بک‌آپ به {totalParts.ToPersianNumbers()} بخش تقسیم و ارسال شد.

               🔹 **برای دریافت کل فایل:**
               1. روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
               2. آن‌ها را در یک پوشه قرار دهید
               3. نام فایل‌ها باید به ترتیب باشند: file_1.part، file_2.part، و غیره
               4. با ابزار ترکیب، فایل‌ها را به هم بچسبانید:

               **با خط فرمان (ویندوز):**
               type *.part > file.zip

               **با خط فرمان (لینوکس/مک):**
               cat *.part > file.zip

               ⚠️ **نکته:** حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
               """.Trim()
            : $"""
               📦 **راهنمای دریافت فایل بک‌آپ تاریخ {DateTime.IranNowUtc.Persian.Text.LongDateTime} **

               ✅ فایل بک‌آپ به {totalParts.ToPersianNumbers()} بخش تقسیم و ارسال شد.

               🔹 **برای دریافت کل فایل:**
               1. روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
               2. تمام قطعات فایل (مانند `file.z01`، `file.z02` و `file.zip`) را در **یک پوشه** قرار دهید.
               3. روی فایل `file.zip` (یا `file.z01`) **کلیک راست** کنید.
               4. از منوی `7-Zip` گزینه‌ی **`Extract Here`** یا **`Extract to "file\"`** را انتخاب کنید.
               5. ابزار به صورت خودکار تمام قطعات را شناسایی کرده و فایل اصلی را خارج می‌سازد.

               ⚠️ **نکته:** حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
               """.Trim();

        await telegramBotClient.SendMessage(chatId, text, ParseMode.Markdown, cancellationToken: cancellationToken);
    }
}
