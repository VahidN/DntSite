namespace DntSite.Web.Features.UserProfiles.Models;

public class ForgottenPasswordModel : BaseCaptchaModel
{
    [Display(Name = "آدرس ایمیل")]
    [Required(ErrorMessage = "ایمیل خالی است")]
    [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    [StringLength(450, ErrorMessage = "حداکثر طول ایمیل 450 حرف است.")]
    [DataType(DataType.EmailAddress)]
    public string EMail { set; get; } = default!;
}
