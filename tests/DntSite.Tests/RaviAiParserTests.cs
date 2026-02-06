using DNTCommon.Web.Core;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Utils;

namespace DntSite.Tests;

[TestClass]
public class RaviAiParserTests
{
    [TestMethod]
    public void ParseSuccessRecordWellFormedShouldSucceed()
    {
        var input = """
                    === RAVI_AI_SUCCESS_RECORD_BEGIN ===
                    STATUS: ok
                    TITLE: معرفی نسخه جدید .NET
                    SUMMARY:
                    پاراگراف اول شامل دو جمله است. جمله دوم همین است.

                    پاراگراف دوم هم دو جمله دارد. این هم جمله دوم.

                    TAGS: DotNet, CSharp, ASPNetCore
                    === RAVI_AI_SUCCESS_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiSuccessResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(expected: "ok", result.Status);
        Assert.AreEqual(expected: "معرفی نسخه جدید .NET", result.Title);
        Assert.IsTrue(result.Summary!.Contains(value: "پاراگراف اول", StringComparison.Ordinal));
        Assert.AreEqual(expected: 3, result.Tags!.Count);
        CollectionAssert.Contains(result.Tags, element: "DotNet");
    }

    [TestMethod]
    public void ParseSuccessRecordWithShuffledOrderShouldWork()
    {
        var input = """
                    === RAVI_AI_SUCCESS_RECORD_BEGIN ===
                    SUMMARY:
                    این خلاصه است. جمله دوم.

                    STATUS: ok
                    TAGS: Blazor, Azure
                    TITLE: خبر تستی
                    === RAVI_AI_SUCCESS_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiSuccessResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(expected: "ok", result.Status);
        Assert.AreEqual(expected: "خبر تستی", result.Title);
        Assert.IsTrue(result.Summary!.StartsWith(value: "این خلاصه است", StringComparison.Ordinal));

        CollectionAssert.AreEqual((string[])["Blazor", "Azure"], result.Tags);
    }

    [TestMethod]
    public void ParseFallbackRecordShouldMapEnumCorrectly()
    {
        var input = """
                    === RAVI_AI_FALLBACK_RECORD_BEGIN ===
                    STATUS: fallback
                    REASON: LowSignalNews
                    TITLE: Null
                    SUMMARY: Null
                    TAGS: Null
                    === RAVI_AI_FALLBACK_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiFallbackResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(expected: "fallback", result.Status);
        Assert.AreEqual(GeminiFallbackReason.LowSignalNews, result.Reason);
    }

    [TestMethod]
    public void AutoRepairShouldFixStatusWithoutColon()
    {
        var input = """
                    === RAVI_AI_SUCCESS_RECORD_BEGIN ===
                    STATUS ok
                    TITLE: تست
                    SUMMARY:
                    متن تستی. جمله دوم.

                    TAGS: DotNet
                    === RAVI_AI_SUCCESS_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiSuccessResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(expected: "ok", result.Status);
        Assert.AreEqual(expected: "تست", result.Title);
    }

    [TestMethod]
    public void TagsShouldHandleBracketsAndExtraSpaces()
    {
        var input = """
                    === RAVI_AI_SUCCESS_RECORD_BEGIN ===
                    STATUS: ok
                    TITLE: تست
                    SUMMARY:
                    متن تستی. جمله دوم.

                    TAGS: [ DotNet ,  CSharp ,ASPNetCore ]
                    === RAVI_AI_SUCCESS_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiSuccessResult;

        Assert.IsNotNull(result);

        CollectionAssert.AreEqual((string[])["DotNet", "CSharp", "ASPNetCore"], result.Tags);
    }

    [TestMethod]
    public void MissingOptionalFieldsShouldNotThrow()
    {
        var input = """
                    === RAVI_AI_SUCCESS_RECORD_BEGIN ===
                    STATUS: ok
                    TITLE: فقط عنوان
                    === RAVI_AI_SUCCESS_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiSuccessResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(expected: "فقط عنوان", result.Title);
        Assert.IsTrue(result.Summary.IsEmpty());
        Assert.IsNull(result.Tags);
    }

    [TestMethod]
    public void NoiseBeforeRecordShouldBeIgnored()
    {
        var input = """
                    Some random AI explanation text…

                    === RAVI_AI_FALLBACK_RECORD_BEGIN ===
                    STATUS: fallback
                    REASON: NotDotNetRelated
                    TITLE: Null
                    SUMMARY: Null
                    TAGS: Null
                    === RAVI_AI_FALLBACK_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiFallbackResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(GeminiFallbackReason.NotDotNetRelated, result.Reason);
    }

    [TestMethod]
    public void ParsedValuesShouldNotContainRecordMarkers()
    {
        var input = """
                    === RAVI_AI_SUCCESS_RECORD_BEGIN ===
                    STATUS: ok
                    TITLE: تست
                    SUMMARY:
                    متن تستی. جمله دوم.

                    TAGS: DotNet
                    === RAVI_AI_SUCCESS_RECORD_END ===
                    """;

        var result = input.ParseGeminiOutput() as GeminiSuccessResult;

        Assert.IsNotNull(result);

        Assert.IsFalse(result.Title!.Contains(value: "RAVI_AI", StringComparison.Ordinal));
        Assert.IsFalse(result.Summary!.Contains(value: "RAVI_AI", StringComparison.Ordinal));
    }

    [TestMethod]
    public void ParseSuccessRecordWithRealDataShouldSucceed()
    {
        var input = """
                    === RAVI_AI_SUCCESS_RECORD_BEGIN ===
                    STATUS: ok
                    TITLE: بررسی شرکت‌های پشت توسعه Postgres 18
                    SUMMARY: بررسی جدیدی بر روی شرکت‌هایی که بیشترین سهم را در توسعه نسخه 18 پایگاه داده Postgres داشته‌اند انجام شده است. این تحلیل به بررسی میزان مشارکت شرکت‌ها در توسعه سرور Postgres می‌پردازد و نه پروژه‌های مرتبط با اکوسیستم گسترده‌تر Postgres مانند pgbouncer، pgjdbc، pgvector، pgadmin، postgrest و postgis.

                    اندازه‌گیری مشارکت در توسعه نرم‌افزار کار دشواری است. صرفاً شمارش خطوط کد اضافه یا حذف شده، می‌تواند گمراه‌کننده باشد، زیرا این آمار شامل مستندات یا کدهای تولید شده نیز می‌شود. همچنین، تعداد دفعات commit کردن توسط توسعه‌دهندگان نیز می‌تواند متفاوت باشد. این تحلیل تنها به مشارکت در توسعه سرور Postgres و نه پروژه‌های مرتبط با آن می‌پردازد.

                    بر اساس این تحلیل، شرکت EnterpriseDB بیشترین تعداد commit را داشته است، در حالی که Amazon بیشترین تعداد افراد مشارکت‌کننده را در اختیار داشته است. در زیر لیستی از 20 شرکت برتر بر اساس تعداد commit، میزان درج و حذف خطوط کد و تعداد مشارکت‌کنندگان ارائه شده است. این اطلاعات می‌تواند به درک بهتر ساختار توسعه Postgres کمک کند.

                    یکی از نکات جالب توجه، commit انفرادی توسط یک توسعه‌دهنده متعلق به Intel بود که به بازسازی نحوه بررسی پشتیبانی از SSE4.2 در محاسبات CRC-32C می‌پرداخت. همچنین، Sophie Alpert، یک مشارکت‌کننده جدید، یک باگ مربوط به فیلتر کردن محدوده ctid را برطرف کرد، باگی که به نظر می‌رسد از سال 1999 وجود داشته است.

                    این تحلیل نشان می‌دهد که توسعه Postgres یک تلاش جمعی است که توسط طیف گسترده‌ای از شرکت‌ها و افراد انجام می‌شود. نویسنده قصد دارد در پست‌های بعدی به بررسی بیشتر جنبه‌های توسعه Postgres بپردازد و از خوانندگان دعوت می‌کند تا موضوعات مورد علاقه خود را برای بررسی بیشتر مطرح کنند.
                    TAGS: DotNet, Postgres, Database, Development, Analysis
                    """;

        var result = input.ParseGeminiOutput() as GeminiSuccessResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(expected: "ok", result.Status);
        Assert.AreEqual(expected: "بررسی شرکت\u200cهای پشت توسعه Postgres 18", result.Title);
        Assert.IsTrue(result.Summary!.Contains(value: "بررسی میزان مشارکت", StringComparison.Ordinal));
        Assert.AreEqual(expected: 5, result.Tags!.Count);
        CollectionAssert.Contains(result.Tags, element: "Database");
    }
}
