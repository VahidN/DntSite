namespace DntSite.Web.Features.AppConfigs.Entities;

[ComplexType]
public class UsedPasswordsSetting
{
    [Display(Name = "تا چه تعداد کلمه عبور قبلی قابل قبول نیست؟")]
    public int NotAllowedPreviouslyUsedPasswords { get; set; }

    [Display(Name = "هر چند روز یکبار،‌ نیاز به تغییر کلمه عبور یادآوری شود؟")]
    public int ChangePasswordReminderDays { get; set; }
}
