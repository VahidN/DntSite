namespace DntSite.Web.Features.Posts.Models;

public class ConvertToLinkModel
{
    [Display(Name = "آدرس اصلی و خارجی لینک اشتراکی")]
    [StringLength(1000, ErrorMessage = "حداکثر طول آدرس پیام 1000 حرف می‌باشد")]
    [DataType(DataType.Url)]
    [Required(ErrorMessage = "*")]
    public string Url { set; get; } = null!;
}
