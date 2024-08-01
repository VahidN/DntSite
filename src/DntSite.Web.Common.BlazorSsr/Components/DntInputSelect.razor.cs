namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom select element
/// </summary>
public partial class DntInputSelect<T>
{
    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string FieldIcon { set; get; } = default!;

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
    public T Value { get; set; } = default!;

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<T>> ValueExpression { get; set; } = default!;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     Sets the '@oninput' attribute to the provided string or delegate value.
    /// </summary>
    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }

    /// <summary>
    ///     The InputSelect items list
    /// </summary>
    [Parameter]
    public IReadOnlyDictionary<string, T> Items { get; set; } = new Dictionary<string, T>(StringComparer.Ordinal);

    private BlazorHtmlField<T> ValueField => new(ValueExpression);

    /// <summary>
    ///     The optional label name of the custom InputText
    /// </summary>
    [Parameter]
    public string? OptionalLabel { get; set; }
}
