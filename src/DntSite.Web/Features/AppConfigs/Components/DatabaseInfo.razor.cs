using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.AppConfigs.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class DatabaseInfo
{
    private DatabaseInfoModel? _databaseInfo;

    private bool _needsShrinkDatabase;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IDatabaseInfoService DatabaseInfoService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();
        _databaseInfo = await DatabaseInfoService.GetDatabaseInfoAsync();
        _needsShrinkDatabase = await DatabaseInfoService.NeedsShrinkDatabaseAsync();
    }

    private void OnShrinkDatabase()
    {
        DatabaseInfoService.ShrinkDatabase();
        ApplicationState.NavigateTo(AppConfigsRoutingConstants.DatabaseInfo);
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([AppConfigsBreadCrumbs.DatabaseInfoBreadCrumb]);
}
