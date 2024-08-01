namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom InputCheckbox component
/// </summary>
public partial class DntInputCheckBox
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
    ///     Sets the '@bind-Value' attribute to the provided string or object.
    /// </summary>
    [Parameter]
    public bool Value { get; set; }

    /// <summary>
    ///     Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<bool>> ValueExpression { get; set; } = default!;

    /// <summary>
    ///     The label name of the custom InputText
    /// </summary>
    [Parameter]
    public string LabelName { get; set; } = default!;

    /// <summary>
    ///     Sets the '@oninput' attribute to the provided string or delegate value.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    private BlazorHtmlField<bool> ValueField => new(ValueExpression);
}
