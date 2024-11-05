using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Layout;

public partial class MainHeaderMenu
{
    private string LoginUrl
        => $"{UserProfilesRoutingConstants.Login}?returnUrl={Uri.EscapeDataString(ApplicationState.CurrentAbsoluteUri.ToString())}";

    private string? SiteName => ApplicationState.AppSetting?.BlogName;

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;
}
