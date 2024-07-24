namespace DntSite.Web.Features.UserProfiles.Models;

public class ChangePasswordModel : ChangeUserPasswordModel
{
    [Display(Name = "کلمه‌ی عبور فعلی")]
    [Required(ErrorMessage = "(*)")]

    public string OldPassword { get; set; } = default!;
}
