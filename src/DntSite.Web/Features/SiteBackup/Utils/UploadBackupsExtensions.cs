using DntSite.Web.Features.SiteBackup.Models;

namespace DntSite.Web.Features.SiteBackup.Utils;

public static class UploadBackupsExtensions
{
    public const int MaxPartSizeMB = 45;

    public static void DeleteParts(this IList<string>? partPaths, ILogger logger)
    {
        if (partPaths is null || partPaths.Count == 0)
        {
            return;
        }

        foreach (var partPath in partPaths)
        {
            partPath.TryDeleteFile(logger);
        }
    }

    public static bool UseProvidedParts(this PartsInfo? parts, string? currentZipPassword)
        => parts?.Parts is not null && parts.Parts.Count != 0 &&
           string.Equals(parts.Password, currentZipPassword, StringComparison.Ordinal);

    public static string? GetUploadMessage(this IList<string>? partPaths)
    {
        var totalParts = partPaths?.Count ?? 0;

        if (partPaths is null || totalParts == 0)
        {
            return null;
        }

        var hasParts = partPaths.Any(file => file.EndsWith(value: ".part", StringComparison.OrdinalIgnoreCase));
        var fileName = Path.GetFileNameWithoutExtension(partPaths[index: 0]);
        var dateTime = DateTime.IranNowUtc.Persian.Text.LongDateTime;

        return hasParts
            ? $"""
               📦 **راهنمای دریافت فایل بک‌آپ تاریخ** `{dateTime}`

               ✅ فایل بک‌آپ `{fileName}` به `{totalParts.ToPersianNumbers()}` بخش تقسیم و ارسال شد.

               🔹 **برای دریافت کل فایل:**
               - روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
               - آن‌ها را در یک پوشه قرار دهید
               - نام فایل‌ها باید به ترتیب باشند: `file1.part` `file2.part` و غیره
               - با ابزار ترکیب، فایل‌ها را به هم بچسبانید:

               **با خط فرمان (ویندوز):**
               `type *.part > {fileName}.zip`

               **با خط فرمان (لینوکس، مک):**
               `cat *.part > {fileName}.zip`

               ⚠️ **نکته:** حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
               """.Trim()
            : $"""
               📦 **راهنمای دریافت فایل بک‌آپ تاریخ** `{dateTime}`

               ✅ فایل بک‌آپ `{fileName}` به `{totalParts.ToPersianNumbers()}` بخش تقسیم و ارسال شد.

               🔹 **برای دریافت کل فایل:**
               - روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
               - تمام قطعات فایل (مانند `file.z01` `file.z02` و `file.zip`) را در یک پوشه قرار دهید.
               - روی فایل `file.zip` (یا `file.z01`) کلیک راست کنید.
               - از منوی `7-Zip` گزینه‌ی `Extract Here` یا `Extract to file` را انتخاب کنید.
               - ابزار به صورت خودکار تمام قطعات را شناسایی کرده و فایل اصلی را خارج می‌سازد.

               ⚠️ **نکته:** حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
               """.Trim();
    }
}
