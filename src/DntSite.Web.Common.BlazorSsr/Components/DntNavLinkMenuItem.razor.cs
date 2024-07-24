namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom NavLink Menu component
/// </summary>
public partial class DntNavLinkMenuItem
{
    /// <summary>
    ///     Defines the owner of this component.
    /// </summary>
    [CascadingParameter]
    internal DntNavLinkMenu? OwnerLinkMenu { get; set; }

    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool IsVisible { set; get; } = true;

    /// <summary>
    ///     The href of this menu item.
    ///     Its default value is `/`
    /// </summary>
    [Parameter]
    public string? Url { set; get; }

    /// <summary>
    ///     The css icon of this menu item.
    /// </summary>
    [Parameter]
    public string? CssIcon { set; get; }

    /// <summary>
    ///     The title of this menu item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { set; get; }

    /// <summary>
    ///     Is this a `dropdown-divider`?
    /// </summary>
    [Parameter]
    public bool IsDivider { set; get; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (OwnerLinkMenu is null)
        {
            throw new InvalidOperationException("`DntNavLinkMenuItem` should be placed inside of a `DntNavLinkMenu`.");
        }
    }
}
