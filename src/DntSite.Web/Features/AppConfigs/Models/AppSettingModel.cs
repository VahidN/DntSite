using DntSite.Web.Features.AppConfigs.Entities;

namespace DntSite.Web.Features.AppConfigs.Models;

public class AppSettingModel
{
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    [Display(Name = "نام سایت")]
    [Required(ErrorMessage = "*")]
    public string BlogName { set; get; } = default!;

    [Display(Name = "سایت فعال است؟")] public bool SiteIsActive { set; get; }

    [Display(Name = "امضای ایمیل‌های برنامه")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? SiteEmailsSig { set; get; }

    public SmtpServerSetting SmtpServerSetting { set; get; } = new();

    [Display(Name = "آدرس ایمیل سایت")] public string? SiteFromEmail { set; get; }

    [Display(Name = "آیا ثبت نام در سایت باز است؟")]
    public bool CanUsersRegister { set; get; }

    [Display(Name = "آدرس کامل سایت‌های بسته شده (دومین و زیر دومین‌ها) در قسمت ارسال لینک (هر سطر یک آدرس)")]
    public string? BannedUrls { set; get; }

    [Display(Name = "آدرس کامل سایت‌های بسته شده (فقط اصل هاست) در قسمت ارسال لینک (هر سطر یک آدرس)")]
    public string? BannedSites { set; get; }

    [Display(Name = "الگوهای ارجاع دهنده‌های ممنوع (هر سطر یک الگو)")]
    public string? BannedReferrers { set; get; }

    [Display(Name = "ایمیل‌های ممنوع (هر سطر یک آدرس)")]
    public string? BannedEmails { set; get; }

    [Display(Name = "کلمات عبور ممنوع (هر سطر یک مورد)")]
    public string? BannedPasswords { set; get; }

    public UsedPasswordsSetting UsedPasswords { get; set; } = new();

    [Display(Name = "آدرس ریشه سایت")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    [Required(ErrorMessage = "*")]
    public string SiteRootUri { get; set; } = default!;

    public MinimumRequiredPosts MinimumRequiredPosts { get; set; } = new();
}
