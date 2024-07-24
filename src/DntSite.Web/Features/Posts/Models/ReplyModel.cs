namespace DntSite.Web.Features.Posts.Models;

public class ReplyModel
{
    public int PostId { set; get; }

    public required string ReplyId { set; get; }

    [Required(ErrorMessage = "لطفا نامی را وارد نمائید")]
    [Display(Name = "نام")]
    public required string Name { set; get; }

    [Required(ErrorMessage = "لطفا آدرس ایمیلی را وارد نمائید")]
    [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    [Display(Name = "آدرس ایمیل")]
    [DataType(DataType.EmailAddress)]
    public required string Email { set; get; }

    [Display(Name = "آدرس سایت")]
    [DataType(DataType.Url)]
    public string? HomeUrl { set; get; }

    [Required(ErrorMessage = "لطفا پیامی را وارد نمائید")]
    [Display(Name = "پیغام")]
    [DataType(DataType.MultilineText)]
    public required string Message { set; get; }
}
