namespace DntSite.Web.Features.UserProfiles.Models;

public abstract class BaseCaptchaModel
{
    private string? _inputCaptchaValue;

    [RegularExpression(@"^\d+$", ErrorMessage = "مقدار محاسباتی مدنظر، عددی است.")]
    [Required(ErrorMessage = "لطفا این مقدار محاسباتی را تکمیل کنید.")]
    [Compare(nameof(RealCaptchaValue), ErrorMessage = "حاصل نهایی محاسبات وارد شده، صحیح نیست. لطفا مجددا سعی کنید.")]
    public string? InputCaptchaValue
    {
        get => _inputCaptchaValue;
        set => _inputCaptchaValue = value.ToEnglishNumbers();
    }

    public string? RealCaptchaValue { get; set; }
}
