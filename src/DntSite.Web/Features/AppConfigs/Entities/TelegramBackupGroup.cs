namespace DntSite.Web.Features.AppConfigs.Entities;

[ComplexType]
public class TelegramBackupGroup
{
    [Display(Name = "ارسال بک‌آپ به تلگرام فعال است؟")]
    public bool IsActive { set; get; }

    [Display(Name = "توکن دسترسی")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? AccessToken { get; set; }

    [Display(Name = "شناسه گفتگو")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? ChatId { get; set; }
}
