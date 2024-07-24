namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom card component
/// </summary>
public partial class DntCard
{
    /// <summary>
    ///     The ChildContent to be rendered
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }

    /// <summary>
    ///     The ChildContent to be rendered
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    ///     The ChildContent to be rendered
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     The ChildContent to be rendered
    /// </summary>
    [Parameter]
    public RenderFragment? Footer { get; set; }

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    /// <summary>
    ///     Its default value is `3`.
    /// </summary>
    [Parameter]
    public int MarginTop { get; set; } = 3;
}
