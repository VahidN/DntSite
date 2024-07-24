namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A loading component
/// </summary>
public partial class DntLoading
{
    /// <summary>
    ///     The actual loaded content to be displayed.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    /// <summary>
    ///     The actual loaded content to be displayed.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsLoading { get; set; }

    /// <summary>
    ///     Its default value is `5`.
    /// </summary>
    [Parameter]
    public int Margin { get; set; } = 5;
}
