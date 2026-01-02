using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.AppConfigs.Entities;

public class AppSetting : BaseAuditedEntity
{
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    [Display(Name = "نام سایت")]
    public required string BlogName { set; get; }

    [Display(Name = "سایت فعال است؟")] public bool SiteIsActive { set; get; }

    [Display(Name = "امضای ایمیل‌های برنامه")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? SiteEmailsSig { set; get; }

    public SmtpServerSetting SmtpServerSetting { set; get; } = new();

    [Display(Name = "آدرس ایمیل سایت")] public string? SiteFromEmail { set; get; }

    [Display(Name = "آیا ثبت نام در سایت باز است؟")]
    public bool CanUsersRegister { set; get; }

    [Display(Name = "آدرس کامل سایت‌های بسته شده (دومین و زیر دومین‌ها) در قسمت ارسال لینک (هر سطر یک آدرس)")]
    public IList<string> BannedUrls { set; get; } = [];

    [Display(Name = "آدرس کامل سایت‌های بسته شده (فقط اصل هاست) در قسمت ارسال لینک (هر سطر یک آدرس)")]
    public IList<string> BannedSites { set; get; } = [];

    [Display(Name = "الگوهای ارجاع دهنده‌های ممنوع (هر سطر یک الگو)")]
    public IList<string> BannedReferrers { set; get; } = [];

    [Display(Name = "ایمیل‌های ممنوع")] public IList<string> BannedEmails { set; get; } = [];

    [Display(Name = "کلمات عبور ممنوع")] public IList<string> BannedPasswords { set; get; } = [];

    public UsedPasswordsSetting UsedPasswords { get; set; } = new();

    [Display(Name = "آدرس ریشه سایت")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public required string SiteRootUri { get; set; }

    public MinimumRequiredPosts MinimumRequiredPosts { get; set; } = new();

    public GeminiNewsFeeds GeminiNewsFeeds { get; set; } = new();
}
