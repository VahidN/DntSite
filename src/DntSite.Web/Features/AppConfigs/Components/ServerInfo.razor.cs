using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.AppConfigs.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class ServerInfo
{
    private WebServerInfoModel? _info;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IWebServerInfoService ServerInfoService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();
        _info = await ServerInfoService.GetWebServerInfoAsync();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([AppConfigsBreadCrumbs.ServerInfoBreadCrumb]);
}
