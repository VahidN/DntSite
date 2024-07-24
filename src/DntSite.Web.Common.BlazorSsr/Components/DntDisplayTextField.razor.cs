namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom DisplayTextField
/// </summary>
public partial class DntDisplayTextField
{
    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

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
    ///     The optional description of the custom InputText
    /// </summary>
    [Parameter]
    public RenderFragment? OptionalDescription { get; set; }

    /// <summary>
    ///     Input field's icon from https://icons.getbootstrap.com/.
    /// </summary>
    [Parameter]
    public string FieldIcon { set; get; } = default!;

    [Parameter] public string? Value { get; set; }
}
