using DntSite.Web.Features.SideBar.Entities;
using DntSite.Web.Features.SideBar.Services.Contracts;

namespace DntSite.Web.Features.SideBar.Components;

public partial class RenderCustomSidebar
{
    private CustomSidebar? _customSidebar;

    [InjectComponentScoped] internal ICustomSidebarService CustomSidebarService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
        => _customSidebar = await CustomSidebarService.GetCustomSidebarAsync();
}
