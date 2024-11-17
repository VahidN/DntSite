using DntSite.Web.Features.AppConfigs.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace DntSite.Web.Features.AppConfigs.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class ServerInfo
{
    private WebServerInfo? _webServerInfo;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Inject] internal IKeyManager KeyManager { set; get; } = null!;

    private List<IKey> GetKeysList() => [..KeyManager.GetAllKeys().OrderByDescending(key => key.CreationDate)];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();
        _webServerInfo = await WebServerInfoProvider.GetServerInfoAsync();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([AppConfigsBreadCrumbs.ServerInfoBreadCrumb]);

    private static string? GetVersionInfo() => Assembly.GetExecutingAssembly().GetBuildDateTime();
}
