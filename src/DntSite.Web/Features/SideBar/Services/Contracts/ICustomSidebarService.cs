using DntSite.Web.Features.SideBar.Entities;
using DntSite.Web.Features.SideBar.Models;

namespace DntSite.Web.Features.SideBar.Services.Contracts;

public interface ICustomSidebarService : IScopedService
{
    public Task<CustomSidebar?> GetCustomSidebarAsync();

    public Task AddOrUpdateCustomSidebarAsync(CustomSidebarModel formData);
}
