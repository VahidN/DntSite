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

        return hasParts
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
    }
}
