namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom radio component
/// </summary>
public partial class DntInputRadioEnumType<TEnum>
    where TEnum : Enum
{
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
    ///     Sets the '@bind-Value' attribute to the provided string or object.
    /// </summary>
    [Parameter]
    public TEnum Value { get; set; } = default!;

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<TEnum>> ValueExpression { get; set; } = default!;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     Sets the '@oninput' attribute to the provided string or delegate value.
    /// </summary>
    [Parameter]
    public EventCallback<TEnum> ValueChanged { get; set; }

    private IReadOnlyDictionary<string, TEnum> Items { get; set; } =
        new Dictionary<string, TEnum>(StringComparer.Ordinal);

    private BlazorHtmlField<TEnum> ValueField => new(ValueExpression);

    /// <summary>
    ///     Method invoked when the component has received parameters from its parent.
    /// </summary>
    protected override void OnParametersSet() => SetItems();

    private void SetItems() => Items = ReflectionExtensions.GetEnumItems<TEnum>();
}
