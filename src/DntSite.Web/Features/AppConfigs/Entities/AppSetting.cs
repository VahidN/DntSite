using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.AppConfigs.Entities;

public class AppSetting : BaseAuditedEntity
{
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    [Display(Name = "نام سایت")]
    public required string BlogName { get; set; }

    [Display(Name = "سایت فعال است؟")] public bool SiteIsActive { get; set; }

    [Display(Name = "سایت، پس از چند روز عدم سرزدن ادمین، غیرفعال شود؟")]
    public int DeactivateSiteAfterDaysOfInactivity { get; set; }

    [Display(Name = "امضای ایمیل‌های برنامه")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? SiteEmailsSig { get; set; }

    public SmtpServerSetting SmtpServerSetting { get; set; } = new();

    [Display(Name = "آدرس ایمیل سایت")] public string? SiteFromEmail { get; set; }

    [Display(Name = "آیا ثبت نام در سایت باز است؟")]
    public bool CanUsersRegister { get; set; }

    [Display(Name = "آدرس کامل سایت‌های بسته شده (دومین و زیر دومین‌ها) در قسمت ارسال لینک (هر سطر یک آدرس)")]
    public IList<string> BannedUrls { get; set; } = [];

    [Display(Name = "آدرس کامل سایت‌های بسته شده (فقط اصل هاست) در قسمت ارسال لینک (هر سطر یک آدرس)")]
    public IList<string> BannedSites { get; set; } = [];

    [Display(Name = "الگوهای ارجاع دهنده‌های ممنوع (هر سطر یک الگو)")]
    public IList<string> BannedReferrers { get; set; } = [];

    [Display(Name = "ایمیل‌های ممنوع")] public IList<string> BannedEmails { get; set; } = [];

    [Display(Name = "کلمات عبور ممنوع")] public IList<string> BannedPasswords { get; set; } = [];

    public UsedPasswordsSetting UsedPasswords { get; set; } = new();

    [Display(Name = "آدرس ریشه سایت")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public required string SiteRootUri { get; set; }

    public MinimumRequiredPosts MinimumRequiredPosts { get; set; } = new();

    public GeminiNewsFeeds GeminiNewsFeeds { get; set; } = new();

    public TelegramBackupGroup TelegramBackupGroup { get; set; } = new();

    public TelegramBackupGroup TelegramEPubGroup { get; set; } = new();

    public TelegramBackupGroup BaleBackupGroup { get; set; } = new();

    public TelegramBackupGroup BaleEPubGroup { get; set; } = new();

    [Display(Name = "فقط خلاصه توضیحات در فیدها نمایش داده شود")]
    public bool ShowRssBriefDescription { get; set; }

    [Display(Name = "برای مطالب خبری، اسکرین‌شات تهیه شود")]
    public bool ShouldCreateNewsScreenshots { get; set; }

    [Display(Name = "برای گروه‌های مطالب و خبرها، فایل PDF تهیه شود")]
    public bool ShouldCreatePdfsForTags { get; set; }

    [Display(Name = "کلید ای‌پی‌آی YouTube")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? YouTubeDataApikey { get; set; }

    public MegaNzBackup MegaNzBackup { get; set; } = new();
}
