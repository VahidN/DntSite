namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom fieldset component
/// </summary>
public partial class DntCardFrame
{
    /// <summary>
    ///     The ChildContent to be rendered
    /// </summary>
    [Parameter]
    public RenderFragment? FrameBody { get; set; }

    /// <summary>
    ///     The ChildContent to be rendered
    /// </summary>
    [Parameter]
    public RenderFragment? FrameHeader { get; set; }

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    ///     Its default value is `1`.
    /// </summary>
    [Parameter]
    public int MarginTop { get; set; } = 1;

    /// <summary>
    ///     Its default value is `1`.
    /// </summary>
    [Parameter]
    public int MarginBottom { get; set; } = 1;
}
