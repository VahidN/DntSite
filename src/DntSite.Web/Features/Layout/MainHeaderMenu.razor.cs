using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Layout;

public partial class MainHeaderMenu
{
    private string? _loginUrl;
    private string? _siteName;

    [Inject] internal IFullTextSearchService FullTextSearchService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _siteName = ApplicationState.AppSetting?.BlogName;

        _loginUrl =
            $"{UserProfilesRoutingConstants.Login}?returnUrl={Uri.EscapeDataString(ApplicationState.CurrentAbsoluteUri.ToString())}";
    }
}
