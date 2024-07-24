namespace DntSite.Web.Features.UserProfiles.Models;

public class AccountModel : BaseCaptchaModel
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا نام کاربری خود را وارد نمائید")]
    [StringLength(450)]
    public string Username { get; set; } = default!;

    [Display(Name = "کلمه‌ی عبور")]
    [Required(ErrorMessage = "لطفا کلمه‌ی عبور خود را وارد نمائید")]
    [DataType(DataType.Password)]
    [StringLength(50)]
    public string Password { get; set; } = default!;

    [Display(Name = "مرا به‌خاطر بسپار")] public bool RememberMe { get; set; }
}
