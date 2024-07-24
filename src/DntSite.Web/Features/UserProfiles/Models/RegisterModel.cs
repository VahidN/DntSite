namespace DntSite.Web.Features.UserProfiles.Models;

public class RegisterModel : BaseCaptchaModel
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا نام کاربری خود را وارد نمائید")]
    [StringLength(450, MinimumLength = 3, ErrorMessage = "نام کاربری باید حداقل 3 کاراکتر باشد")]
    [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage = "لطفا تنها از اعداد و حروف انگلیسی استفاده نمائید")]
    public string Username { get; set; } = default!;

    [Display(Name = "نام مستعار")]
    [Required(ErrorMessage = "نام مستعار خالی است.")]
    [StringLength(450, MinimumLength = 3, ErrorMessage = "نام مستعار باید بین سه تا 450 کاراکتر باشد.")]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF,0-9\s]*$",
        ErrorMessage = "لطفا تنها از اعداد و حروف فارسی استفاده نمائید")]
    public string FriendlyName { get; set; } = default!;

    [Display(Name = "آدرس ایمیل")]
    [Required(ErrorMessage = "ایمیل خالی است")]
    [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
        ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید")]
    [StringLength(450, ErrorMessage = "حداکثر طول ایمیل 450 حرف است.")]
    [DataType(DataType.EmailAddress)]
    public string EMail { set; get; } = default!;

    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد نمائید")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "حداقل 6 حرف")]
    [ValidPassword(ErrorMessage = "کلمه عبور باید از حروف کوچک، بزرگ، اعداد و سمبل‌ها تشکیل شده باشد")]
    [Compare(nameof(Password2), ErrorMessage = "دو کلمه عبور وارد شده یکی نیستند")]
    public string Password1 { get; set; } = default!;

    [Display(Name = "تکرار کلمه عبور")]
    [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد نمائید")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "حداقل 6 حرف")]
    [ValidPassword(ErrorMessage = "کلمه عبور باید از حروف کوچک، بزرگ، اعداد و سمبل‌ها تشکیل شده باشد")]
    public string Password2 { get; set; } = default!;
}
