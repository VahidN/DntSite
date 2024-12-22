using SkiaSharp;

namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A Blazor SSR captcha component
/// </summary>
public partial class DntTextCaptcha
{
    private const string GuidFormat = "D";
    private const string CaptchaHiddenFieldKey = "captcha-key";
    private const string AntiDosCacheKeyPrefix = "__Captcha__Anti__Dos__";
    private const string CaptchaCacheKeyPrefix = "__Dnt__Captcha__";

    private static readonly Dictionary<OperatorType, string> OperatorTypeMeaning = new()
    {
        {
            OperatorType.Addition, "به‌علاوه"
        },
        {
            OperatorType.Multiplication, "ضربدر"
        },
        {
            OperatorType.Subtraction, "منهای"
        },
        {
            OperatorType.Division, "تقسیم‌بر"
        }
    };

    private string? _cacheKey;

    private string? _captchaImageSrc;

    private bool _isDosAttack;

    [Inject] internal IPasswordHasherService PasswordHasherService { set; get; } = null!;

    [Inject] internal ICacheService CacheService { set; get; } = null!;

    [Inject] internal IRandomNumberProvider RandomNumberProvider { set; get; } = null!;

    [CascadingParameter] private HttpContext? HttpContext { get; set; }

    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputText's column width. Its default value is `9`.
    /// </summary>
    [Parameter]
    public int InputTextColumnWidth { get; set; } = 9;

    /// <summary>
    ///     The label's column width of the custom InputText. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 3;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? LabelName { get; set; }

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string? BanErrorFieldIcon { set; get; }

    /// <summary>
    ///     The max value of the captcha. It's default value is 12.
    /// </summary>
    [Parameter]
    public int Max { set; get; } = 12;

    /// <summary>
    ///     The min value of the captcha. It's default value is 1.
    /// </summary>
    [Parameter]
    public int Min { set; get; } = 1;

    private BlazorHtmlField<string?> ValueField => new(InputCaptchaValueExpression);

    [Parameter] public Expression<Func<string?>> InputCaptchaValueExpression { get; set; } = default!;

    [Parameter] [EditorRequired] public string? InputCaptchaValue { get; set; }

    [Parameter] public EventCallback<string?> InputCaptchaValueChanged { get; set; }

    [Parameter] [EditorRequired] public string? RealCaptchaValue { get; set; }

    [Parameter] public EventCallback<string?> RealCaptchaValueChanged { get; set; }

    /// <summary>
    ///     Its default value is 2 minutes
    /// </summary>
    [Parameter]
    public TimeSpan AbsoluteCaptchaExpirationRelativeToNow { set; get; } = TimeSpan.FromMinutes(minutes: 2);

    [SupplyParameterFromForm(Name = CaptchaHiddenFieldKey)]
    public Guid? CaptchaHiddenCacheKey { get; set; }

    /// <summary>
    ///     How long a client should be banned in minutes? Its default value is `2`.
    /// </summary>
    [Parameter]
    public TimeSpan BanDurationRelativeToNow { set; get; } = TimeSpan.FromMinutes(minutes: 2);

    /// <summary>
    ///     Maximum number of allowed requests per `BanDurationRelativeToNow`. Its default value is `10`.
    /// </summary>
    [Parameter]
    public int AllowedTriesPermitLimit { set; get; } = 10;

    /// <summary>
    ///     An error message for the banned users.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string BanErrorMessage { set; get; }

    /// <summary>
    ///     A custom local .ttf font to draw the captcha's text
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string CustomFontPath { set; get; }

    /// <summary>Its default value is `18`.</summary>
    [Parameter]
    public int FontSize { set; get; } = 18;

    /// <summary>Its default value is `SKColors.Black`.</summary>
    [Parameter]
    public SKColor FontColor { set; get; } = SKColors.Black;

    /// <summary>Its default value is `SKColors.White`.</summary>
    [Parameter]
    public SKColor BgColor { set; get; } = SKColors.White;

    /// <summary>Its default value is `SKColors.LightGray`.</summary>
    [Parameter]
    public SKColor ShadowColor { set; get; } = SKColors.LightGray;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _isDosAttack = IsDosAttack();

        if (_isDosAttack)
        {
            return;
        }

        ValidateCaptcha();
        GenerateCaptcha();
    }

    private void GenerateCaptcha()
    {
        var (captchaText, captchaNumber) = GenerateMathematicalQuestion();

        _captchaImageSrc = captchaText.TextToBase64DataImage(new TextToImageOptions
        {
            CustomFontPath = CustomFontPath,
            FontSize = FontSize,
            FontColor = FontColor,
            CaptchaNoise = new CaptchaNoise(),
            AddDropShadow = true,
            BgColor = BgColor,
            ShadowColor = ShadowColor
        });

        _cacheKey = PasswordHasherService.CreateCryptographicallySecureGuid().ToString(GuidFormat);

        CacheService.Add($"{CaptchaCacheKeyPrefix}{_cacheKey}", nameof(DntTextCaptcha), captchaNumber,
            AbsoluteCaptchaExpirationRelativeToNow);
    }

    private void ValidateCaptcha()
    {
        if (!CaptchaHiddenCacheKey.HasValue)
        {
            return;
        }

        var captchaCacheKey = $"{CaptchaCacheKeyPrefix}{CaptchaHiddenCacheKey.Value.ToString(GuidFormat)}";

        if (!CacheService.TryGetValue(captchaCacheKey, out int realCaptchaValue))
        {
            return;
        }

        CacheService.Remove(captchaCacheKey);

        if (RealCaptchaValueChanged.HasDelegate)
        {
            _ = RealCaptchaValueChanged.InvokeAsync(realCaptchaValue.ToString(CultureInfo.InvariantCulture));
        }
    }

    private (string captchaText, int captchaNumber) GenerateMathematicalQuestion()
    {
        var firstNumber = RandomNumberProvider.GetSecureRandomInt32(Min, Max);
        var firstNumberDisplayType = RandomNumberProvider.GetRandomEnumItem<OperandDisplayType>();
        var firstNumberText = ConvertNumberToText(firstNumber, firstNumberDisplayType);

        var secondNumber = RandomNumberProvider.GetSecureRandomInt32(firstNumber, Max);
        var secondNumberDisplayType = RandomNumberProvider.GetRandomEnumItem<OperandDisplayType>();
        var secondNumberText = ConvertNumberToText(secondNumber, secondNumberDisplayType);

        var currentOperator = RandomNumberProvider.GetRandomEnumItem<OperatorType>();
        var operatorMeaning = OperatorTypeMeaning[currentOperator];

        var captchaText = $"{secondNumberText} {operatorMeaning} {firstNumberText}";

        var captchaNumber = currentOperator switch
        {
            OperatorType.Addition => firstNumber + secondNumber,
            OperatorType.Subtraction => secondNumber - firstNumber,
            OperatorType.Multiplication => firstNumber * secondNumber,
            OperatorType.Division => secondNumber / firstNumber,
            _ => throw new NotSupportedException($"{currentOperator} is not supported.")
        };

        return (captchaText, captchaNumber);
    }

    private static string ConvertNumberToText(int number, OperandDisplayType operandDisplayType)
        => operandDisplayType switch
        {
            OperandDisplayType.Word => number.NumberToText(Language.Persian),
            OperandDisplayType.Number => number.ToString(CultureInfo.InvariantCulture),
            _ => throw new ArgumentOutOfRangeException(nameof(operandDisplayType), operandDisplayType, message: null)
        };

    private bool IsDosAttack()
    {
        if (HttpContext is null)
        {
            return false;
        }

        var antiDosCacheKey = $"{AntiDosCacheKeyPrefix}{HttpContext.GetIP()}";
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(BanDurationRelativeToNow.Minutes);

        if (!CacheService.TryGetValue<ThrottleInfo>(antiDosCacheKey, out var clientThrottleInfo) ||
            clientThrottleInfo is null)
        {
            clientThrottleInfo = new ThrottleInfo
            {
                RequestsCount = 1,
                ExpiresAt = expiresAt
            };

            CacheService.Add(antiDosCacheKey, nameof(DntTextCaptcha), clientThrottleInfo, expiresAt);

            return false;
        }

        if (clientThrottleInfo.RequestsCount > AllowedTriesPermitLimit)
        {
            CacheService.Add(antiDosCacheKey, nameof(DntTextCaptcha), clientThrottleInfo, expiresAt);

            return true;
        }

        clientThrottleInfo.RequestsCount++;
        CacheService.Add(antiDosCacheKey, nameof(DntTextCaptcha), clientThrottleInfo, expiresAt);

        return false;
    }

    private enum OperatorType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    private enum OperandDisplayType
    {
        Word,
        Number
    }
}
