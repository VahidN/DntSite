using System.Security.Claims;

namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom security trimming component
/// </summary>
public partial class DntSecurityTrim
{
    [CascadingParameter] internal HttpContext? HttpContext { set; get; }

    private bool IsVisible => HttpContext.HasUserAccess(AllowAnonymous, AllowedClaims, AllowedRoles);

    /// <summary>
    ///     The descendant components (defined in ChildContent) to receive the specified CascadingValue.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Is this item visible to anonymous/unauthenticated users?
    ///     Its default value is `true`.
    /// </summary>
    [Parameter]
    public bool AllowAnonymous { set; get; } = true;

    /// <summary>
    ///     Which comma separated roles are allowed to see this menu item?
    /// </summary>
    [Parameter]
    public string? AllowedRoles { set; get; }

    /// <summary>
    ///     Which user-claims are allowed to see this menu item?
    /// </summary>
    [Parameter]
    public IReadOnlyList<Claim>? AllowedClaims { set; get; }
}
