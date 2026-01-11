namespace DntSite.Web.Features.AppConfigs.Models;

public class GeminiNewsFeedsModel
{
    [Display(Name = "جمینی فعال است؟")] public bool IsActive { set; get; }

    [Display(Name = "کلید ای‌پی‌آی جمینی")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    [Required]
    public string? ApiKey { set; get; }

    [Display(Name = "آدرس‌های آراس‌اس خبری (هر سطر یک مورد)")]
    public string? NewsFeeds { set; get; }
}
