using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.UserProfiles.Components;

public partial class RedirectToLogin
{
    [Inject] internal NavigationManager NavigationManager { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var returnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

        NavigationManager.NavigateTo(
            string.IsNullOrEmpty(returnUrl)
                ? UserProfilesRoutingConstants.Login
                : $"{UserProfilesRoutingConstants.Login}?returnUrl={Uri.EscapeDataString(returnUrl)}", forceLoad: true);
    }
}
