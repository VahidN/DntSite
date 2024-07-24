namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntInputPersianDatePicker<T>
{
    private BlazorHtmlField<T?> ValueField
        => new(ValueExpression ?? throw new InvalidOperationException("Please use @bind-Value here."));

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    [Parameter] public int BeginningOfCentury { set; get; } = 1400;

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
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     The optional label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? OptionalLabel { get; set; }

    /// <summary>
    ///     The optional description of the custom InputText
    /// </summary>
    [Parameter]
    public RenderFragment? OptionalDescription { get; set; }

    [Parameter] public T? Value { set; get; }

    [Parameter] public EventCallback<T?> ValueChanged { get; set; }

    [Parameter] public Expression<Func<T?>> ValueExpression { get; set; } = default!;

    [CascadingParameter] internal HttpContext HttpContext { set; get; } = null!;

    private string FormatValueAsString => Value.FormatDateToShortPersianDate();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SanityCheck();

        if (HttpContext.Request.HasFormContentType &&
            HttpContext.Request.Form.TryGetValue(ValueField.HtmlFieldName, out var data))
        {
            SetDateValueCallback(data.ToString());
        }
    }

    private void SetDateValueCallback(string value)
    {
        if (!ValueChanged.HasDelegate)
        {
            return;
        }

        var valueAsDate = value.TryParsePersianDateToDateTimeOrDateTimeOffset(out T? result, BeginningOfCentury)
            ? result
            : default;

        _ = ValueChanged.InvokeAsync(valueAsDate);
    }

    private void SanityCheck()
    {
        if (!Value.IsDateTimeOrDateTimeOffsetType())
        {
            throw new InvalidOperationException(
                "The `Value` type is not a supported `date` type. DateTime, DateTime?, DateTimeOffset and DateTimeOffset? are supported.");
        }
    }
}
