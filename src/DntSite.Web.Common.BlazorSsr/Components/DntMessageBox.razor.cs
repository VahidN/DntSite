namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     The Alert Component
/// </summary>
public partial class DntMessageBox
{
    private string AlertDivId { get; } = Guid.NewGuid().ToString(format: "N");

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     Is this component visible? Its default value is `true`.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsVisible { set; get; }

    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool ShowCloseButton { set; get; } = true;

    /// <summary>
    ///     The ChildContent to be rendered
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     The alert's type such as info, warning, etc
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required AlertType Type { get; set; } = AlertType.Info;

    /// <summary>
    ///     Its default value is `3`.
    /// </summary>
    [Parameter]
    public int MarginBottom { get; set; } = 3;

    /// <summary>
    ///     Its default value is `2`.
    /// </summary>
    [Parameter]
    public int MarginTop { get; set; } = 2;
}
