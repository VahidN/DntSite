namespace DntSite.Web.Features.AppConfigs.Entities;

[ComplexType]
public class TelegramBackupGroup
{
    [Display(Name = "ارسال به تلگرام فعال است؟")]
    public bool IsActive { set; get; }

    [Display(Name = "توکن دسترسی")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? AccessToken { get; set; }

    [Display(Name = "شناسه گفتگو")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? ChatId { get; set; }

    [Display(Name = "کلمه عبور فایل Zip")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? ZipPassword { get; set; }

    [Display(Name = "حداکثر اندازه فایل قابل ارسال به مگابایت")]
    public int MaxZipPartSize { get; set; }
}
