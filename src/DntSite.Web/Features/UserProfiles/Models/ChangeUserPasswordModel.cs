namespace DntSite.Web.Features.UserProfiles.Models;

public class ChangeUserPasswordModel
{
    [Display(Name = "کلمه‌ی عبور جدید")]
    [Required(ErrorMessage = "(*)")]
    [MinLength(6, ErrorMessage = "حداقل 6 حرف")]
    [ValidPassword(ErrorMessage = "کلمه عبور باید از حروف کوچک و بزرگ انگلیسی، اعداد و سمبل‌ها تشکیل شده باشد")]
    public string NewPassword { get; set; } = default!;

    [Display(Name = "تکرار کلمه‌ی عبور جدید")]
    [Required(ErrorMessage = "(*)")]
    [MinLength(6, ErrorMessage = "حداقل 6 حرف")]
    [Compare(nameof(NewPassword), ErrorMessage = "کلمات عبور وارد شده تطابق ندارد")]
    [ValidPassword(ErrorMessage = "کلمه عبور باید از حروف کوچک و بزرگ انگلیسی، اعداد و سمبل‌ها تشکیل شده باشد")]
    public string ConfirmPassword { get; set; } = default!;
}
