namespace DntSite.Web.Features.AppConfigs.Entities;

[ComplexType]
public class GeminiNewsFeeds
{
    [Display(Name = "کلید ای‌پی‌آی جمینی")]
    [StringLength(maximumLength: 1000, ErrorMessage = "حداکثر 1000 کاراکتر")]
    [Required]
    public string? ApiKey { set; get; }

    [Display(Name = "لیست آدرس‌های آراس‌اس خبری")]
    public IList<string> NewsFeeds { set; get; } = [];
}
