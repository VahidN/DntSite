namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom NavLink Menu component
/// </summary>
public partial class DntNavLinkMenu
{
    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool IsVisible { set; get; } = true;

    /// <summary>
    ///     The main title of the menu.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required RenderFragment MainTitle { set; get; }

    /// <summary>
    ///     The descendant components (defined in ChildContent) to receive the specified CascadingValue.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required RenderFragment MenuItems { get; set; }
}
