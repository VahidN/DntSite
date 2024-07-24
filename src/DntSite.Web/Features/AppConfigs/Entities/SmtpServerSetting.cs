using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

namespace DntSite.Web.Features.AppConfigs.Entities;

[ComplexType]
public class SmtpServerSetting
{
    [Display(Name = "آدرس میل‌سرور")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    [Required]
    public string Address { set; get; } = default!;

    [Display(Name = "نام کاربری میل‌سرور")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    public string? Username { set; get; }

    [Display(Name = "کلمه عبور میل‌سرور")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    [IgnoreAudit]
    public string? Password { set; get; }

    [Display(Name = "شماره پورت میل‌سرور")]
    public int Port { set; get; }

    [Display(Name = "آیا ایمیل‌ها بجای ارسال، در یک پوشه‌ی محلی ذخیره شوند؟")]
    public bool UsePickupFolder { set; get; }

    [Display(Name = "نام پوشه‌ی محلی ذخیره‌ی ایمیل‌ها")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    public string? PickupFolderName { set; get; }

    [Display(Name = "آیا اعتبار مجوز اس‌اس‌ال میل‌سرور برنامه بررسی شود؟")]
    public bool ShouldValidateServerCertificate { set; get; }
}
