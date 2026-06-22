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

    public static string? GetUploadMessage(this ICollection<string>? partPaths)
    {
        var totalParts = partPaths?.Count ?? 0;

        if (partPaths is null || totalParts == 0)
        {
            return null;
        }

        var hasParts = partPaths.Any(file => file.EndsWith(value: ".part", StringComparison.OrdinalIgnoreCase));
        var safeFileName = WebUtility.HtmlEncode(Path.GetFileNameWithoutExtension(partPaths.First()));
        var safeDateTime = WebUtility.HtmlEncode(DateTime.IranNowUtc.Persian.Text.LongDateTime);

        var totalSize = partPaths.Sum(file => new FileInfo(file).Length)            
            .ToFormattedFileSize()
            .ToPersianNumbers();

        return hasParts
            ? $"""
               📦 <b>راهنمای دریافت فایل بک‌آپ تاریخ: {safeDateTime}</b>

               ✅ فایل بک‌آپ <code>{safeFileName}</code> به حجم {totalSize} به {totalParts.ToPersianNumbers()} بخش تقسیم و ارسال شد.

               🔹 <b>برای دریافت کل فایل:</b>

               • روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
               • آن‌ها را در یک پوشه قرار دهید
               • نام فایل‌ها باید به ترتیب باشند: <code>file1.part</code> <code>file2.part</code> و غیره
               • با ابزار ترکیب، فایل‌ها را به هم بچسبانید:

               <b>با خط فرمان (ویندوز):</b>

               <code>type *.part &gt; {safeFileName}.zip</code>

               <b>با خط فرمان (لینوکس، مک):</b>

               <code>cat *.part &gt; {safeFileName}.zip</code>

               ⚠️ <b>نکته:</b> حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
               """.Trim()
            : $"""
               📦 <b>راهنمای دریافت فایل بک‌آپ تاریخ: {safeDateTime}</b>

               ✅ فایل بک‌آپ <code>{safeFileName}</code> به حجم {totalSize} به {totalParts.ToPersianNumbers()} بخش تقسیم و ارسال شد.

               🔹 <b>برای دریافت کل فایل:</b>

               • روی فایل‌های آپلودشده کلیک کرده و همه را دانلود کنید
               • تمام قطعات فایل (مانند <code>file.z01</code>، <code>file.z02</code> و <code>file.zip</code>) را در یک پوشه قرار دهید.
               • روی فایل <code>file.zip</code> (یا <code>file.z01</code>) کلیک راست کنید.
               • از منوی <code>7-Zip</code> گزینه <code>Extract Here</code> یا <code>Extract to file</code> را انتخاب کنید.
               • ابزار به صورت خودکار تمام قطعات را شناسایی کرده و فایل اصلی را خارج می‌سازد.

               ⚠️ <b>نکته:</b> حتماً ابتدا همه‌ی بخش‌ها را دانلود کنید!
               """.Trim();
    }
}
