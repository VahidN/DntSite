using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.SideBar.Models;
using DntSite.Web.Features.SideBar.RoutingConstants;
using DntSite.Web.Features.SideBar.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.SideBar.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class AddCustomSidebar
{
    [CascadingParameter] internal DntAlert Alert { get; set; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { get; set; } = null!;

    [InjectComponentScoped] internal ICustomSidebarService CustomSidebarService { get; set; } = null!;

    [InjectComponentScoped] internal IEmailsFactoryService EmailsFactoryService { get; set; } = null!;

    [SupplyParameterFromForm] public CustomSidebarModel? Model { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Model ??= new CustomSidebarModel();

        if (ApplicationState.HttpContext.IsGetRequest())
        {
            await InitDataAsync();
        }

        AddBreadCrumbs();
    }

    private async Task InitDataAsync()
    {
        var cfg = await CustomSidebarService.GetCustomSidebarAsync();

        if (cfg is null)
        {
            return;
        }

        Model = new CustomSidebarModel
        {
            Description = cfg.Description ?? "",
            IsPublic = cfg.IsPublic
        };
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..SideBarBreadCrumbs.DefaultBreadCrumbs]);

    private async Task PerformAsync()
    {
        await CustomSidebarService.AddOrUpdateCustomSidebarAsync(Model);

        await EmailsFactoryService.SendTextToAllAdminsAsync(
            $"تنظیمات ساید بار سایت با موفقیت ویرایش گردید:<br>{Model?.Description}");

        Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message: " اطلاعات ذخیره شدند.");
    }
}
