namespace DntSite.Web.Features.AppConfigs.Entities;

[ComplexType]
public class MegaNzBackup
{
    [Display(Name = "ارسال به مگاان‌زد فعال است؟")]
    public bool IsActive { get; set; }

    [Display(Name = "ایمیل کاربری در مگاان‌زد")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    public string? MegaEmail { get; set; }

    [Display(Name = "کلمه عبور کاربری در مگاان‌زد")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    public string? MegaPassword { get; set; }

    [Display(Name = "نام پوشه بک‌آپ در مگاان‌زد")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    public string? MegaBackupFolder { get; set; }

    [Display(Name = "نام پوشه ایی‌پاب در مگاان‌زد")]
    [StringLength(maximumLength: 400, ErrorMessage = "حداکثر 400 کاراکتر")]
    public string? MegaEPubFolder { get; set; }

    [Display(Name = "حداکثر تعداد فایل قابل نگهداری در پوشه‌ی مگاان‌زد")]
    public int KeepLastNFilesOnMegaNz { get; set; }

    [Display(Name = "کلمه عبور فایل Zip")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    public string? ZipPassword { get; set; }
}
