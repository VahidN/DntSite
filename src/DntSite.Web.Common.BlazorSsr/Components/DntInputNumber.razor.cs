namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom InputNumber
/// </summary>
public partial class DntInputNumber<TValue>
{
    private BlazorHtmlField<TValue?> ValueField => new(ValueExpression);

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

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string FieldIcon { set; get; } = default!;

    /// <summary>
    ///     The input type. Its default value is `text`.
    /// </summary>
    [Parameter]
    public string InputType { set; get; } = "text";

    /// <summary>
    ///     Allows the browser to choose a correct autocomplete for the password fields.
    ///     Make sure `sync` is on for the Chrome, otherwise you won't see the `Suggest Strong Password` option in Chrome.
    /// </summary>
    [Parameter]
    public AutoCompleteType AutoCompleteType { get; set; } = AutoCompleteType.Off;

    [Parameter] public Expression<Func<TValue?>> ValueExpression { get; set; } = default!;

    [Parameter] public TValue? Value { get; set; }

    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
}
