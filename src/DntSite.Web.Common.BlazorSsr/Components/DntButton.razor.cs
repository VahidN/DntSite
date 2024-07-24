namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom button component
/// </summary>
public partial class DntButton
{
    private string SubmittingDivId { get; } = Guid.NewGuid().ToString("N");

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    /// <summary>
    ///     A custom UI content
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Its default value is `btn btn-primary btn-md`
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = "btn btn-primary btn-md";

    /// <summary>
    ///     Set this option if you need a confirmation before submitting the form
    /// </summary>
    [Parameter]
    public string? CancelConfirmMessage { set; get; }

    /// <summary>
    ///     Type of the button. Its default value is `button`.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ButtonType Type { get; set; } = ButtonType.Button;

    /// <summary>
    ///     The InputText's margin bottom. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int ButtonRowMarginBottom { get; set; } = 3;

    /// <summary>
    ///     This text will be displayed during the posting of the form.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string IsSubmittingText { get; set; }

    /// <summary>
    ///     The InputText's column width. Its default value is `9`.
    /// </summary>
    [Parameter]
    public int ButtonColumnWidth { get; set; } = 9;

    /// <summary>
    ///     The label's column width of the custom InputText. Its default value is `3`.
    /// </summary>
    [Parameter]
    public int LabelColumnWidth { get; set; } = 3;
}
