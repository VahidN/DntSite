namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntHtmlEditor
{
    private BlazorHtmlField<string?> ValueField => new(ValueExpression);

    private string UniqueId { get; } = Guid.NewGuid().ToString(format: "N");

    /// <summary>
    ///     The optional label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? OptionalLabel { get; set; }

    [Parameter] public bool IsReadOnly { get; set; }

    [Parameter] public bool AllowUploadFile { get; set; }

    [Parameter] public string? UploadFileApiPath { get; set; }

    [Parameter] public bool AllowUploadImageFile { get; set; }

    [Parameter] public string? UploadImageFileApiPath { get; set; }

    [Parameter] public string? AcceptedUploadImageFormats { get; set; } = "image/png, image/jpeg";

    [Parameter] public int? MaximumUploadImageSizeInBytes { get; set; } = 614_400;

    [Parameter]
    public string? MaximumUploadImageSizeErrorMessage { get; set; } = "حداکثر اندازه فایل تصویری مجاز برای ارسال: ";

    [Parameter] public string? AcceptedUploadFileFormats { get; set; } = ".zip,.rar,.7z";

    [Parameter] public int? MaximumUploadFileSizeInBytes { get; set; } = 104_857_600;

    [Parameter]
    public string? MaximumUploadFileSizeErrorMessage { get; set; } = "حداکثر اندازه فایل عمومی مجاز برای ارسال: ";

    [Parameter] public string? AdditionalJsonDataDuringImageFileUpload { get; set; }

    [Parameter] public string? AdditionalJsonDataDuringFileUpload { get; set; }

    [Parameter] public bool AllowInsertImageUrl { get; set; }

    [Parameter] public string InsertImageUrlLabel { get; set; } = "لطفا آدرس کامل اینترنتی تصویر را وارد کنید";

    [Parameter] public string UploadOnlyImageErrorMessage { get; set; } = "لطفا تنها یک فایل تصویری را انتخاب کنید";

    [Parameter] public string Height { get; set; } = "220px";

    [Parameter] public string Placeholder { get; set; } = "";

    [Parameter] public string Theme { get; set; } = "snow";

    [Parameter] public string Direction { set; get; } = "rtl";

    [Parameter] public RenderFragment? AdditionalToolbarContent { get; set; }

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int InputRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     The InputText's column width. Its default value is `9`.
    /// </summary>10
    [Parameter]
    public int InputTextColumnWidth { get; set; } = 10;

    /// <summary>
    ///     The label's column width of the custom InputText. Its default value is `2`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 2;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? LabelName { get; set; }

    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool ShowLabel { get; set; } = true;

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string? FieldIcon { set; get; }

    [Parameter] public Expression<Func<string?>> ValueExpression { get; set; } = default!;

    [Parameter] public string? Value { get; set; }

    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    [Parameter] public string? DirectionTooltip { set; get; } = "تعیین جهت ورودی؛ از راست به چپ و یا برعکس";

    [Parameter] public string? AlignTooltip { set; get; } = "تعیین محل قرارگیری متن";

    [Parameter] public string? Header1Tooltip { set; get; } = "تبدیل متن انتخابی به هدر اولیه";

    [Parameter] public string? Header2Tooltip { set; get; } = "تبدیل متن انتخابی به هدر ثانویه";

    [Parameter] public string? BoldTooltip { set; get; } = "متن ضخیم";

    [Parameter] public string? ItalicTooltip { set; get; } = "متن ایتالیک";

    [Parameter] public string? UnderlineTooltip { set; get; } = "خط کشیدن زیر متن انتخابی";

    [Parameter] public string? StrikeTooltip { set; get; } = "خط کشیدن بر روی متن انتخابی";

    [Parameter]
    public string? CodeBlockTooltip { set; get; } =
        "برای تبدیل یک متن انتخاب شده، به یک قطعه کد،‌ ابتدا با استفاده از دکمه‌ی تعیین جهت، قطعه‌ی انتخاب شده را چپ به راست کنید و سپس بر روی دکمه‌ی جاری کلیک کنید تا گزینه انتخاب زبان کد مدنظر، ظاهر شود";

    [Parameter]
    public string? InlineCodeTooltip { set; get; } =
        "تبدیل یک قطعه‌ی کوچک متنی انتخاب شده‌ی درون یک سطر، به کد درون سطری";

    [Parameter] public bool AllowChangeTextColor { set; get; }

    [Parameter] public string? ColorTooltip { set; get; } = "رنگ قلم متن انتخابی";

    [Parameter] public string? BackgroundTooltip { set; get; } = "رنگ پس‌زمینه متن انتخابی";

    [Parameter] public string? BlockquoteTooltip { set; get; } = "تبدیل متن انتخابی به نقل قول";

    [Parameter] public string? OrderedListTooltip { set; get; } = "لیست شماره دار";

    [Parameter] public string? BulletListTooltip { set; get; } = "لیست بولت‌دار";

    [Parameter] public string? IndentMinus1Tooltip { set; get; } = "ایجاد فرورفتگی به سمت چپ";

    [Parameter] public string? IndentPlus1Tooltip { set; get; } = "ایجاد فرورفتگی به سمت راست";

    [Parameter] public string? LinkTooltip { set; get; } = "افزودن پیوند به متن انتخابی";

    [Parameter] public string? InsertImageUrlTooltip { set; get; } = "درج تصویر از طریق آدرس اینترنتی آن";

    [Parameter] public string? UploadImageFileTooltip { set; get; } = "درج تصویر از طریق ارسال فایل آن به سرور";

    [Parameter] public string? UploadFileTooltip { set; get; } = "درج آدرس فایل از طریق ارسال آن فایل به سرور";

    [Parameter] public string? CleanTooltip { set; get; } = "حذف فرمت متن انتخابی";

    [Parameter] public string? CleanStylesTooltip { set; get; } = "حذف تمام ویژگی‌های استایل درج شده‌ی در ادیتور";
}
