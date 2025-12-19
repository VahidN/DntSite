using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.AppConfigs.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class SiteConfig
{
    [InjectComponentScoped] internal IAppSettingsService AppSettingsService { set; get; } = null!;

    [SupplyParameterFromForm] internal AppSettingModel? Model { set; get; }

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Model ??= new AppSettingModel();

        if (ApplicationState.HttpContext.IsGetRequest())
        {
            Model = await AppSettingsService.GetAppSettingModelAsync();
        }

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([AppConfigsBreadCrumbs.SiteConfigBreadCrumb]);

    private async Task PerformAsync()
    {
        await AppSettingsService.AddOrUpdateAppSettingsAsync(Model);
        Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message: " ثبت تنظیمات با موفقیت انجام شد! ");
    }
}
