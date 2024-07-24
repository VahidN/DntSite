using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.PrivateMessages.Models;

public class PublicContactUsModel : BaseCaptchaModel
{
    [Display(Name = "از طرف")]
    [Required(ErrorMessage = "نام ارسال کننده پیام خالی است")]
    public string FromUserName { set; get; } = default!;

    [Display(Name = "آدرس ایمیل")]
    [Required(ErrorMessage = "ایمیل خالی است")]
    [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    [StringLength(450, ErrorMessage = "حداکثر طول ایمیل 450 حرف است.")]
    [DataType(DataType.EmailAddress)]
    public string FromUserNameEmail { set; get; } = default!;

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "متن عنوان خالی است")]
    [StringLength(450, MinimumLength = 1, ErrorMessage = "حداکثر طول عنوان پیام 450 حرف و حداقل آن 1 حرف می‌باشد")]
    public string Title { set; get; } = default!;

    [Display(Name = "پیام")]
    [Required(ErrorMessage = "متن پیام خالی است")]
    [MaxLength]
    [DataType(DataType.MultilineText)]
    public string DescriptionText { set; get; } = default!;
}
