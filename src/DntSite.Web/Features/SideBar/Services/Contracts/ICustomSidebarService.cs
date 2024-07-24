using DntSite.Web.Features.SideBar.Entities;
using DntSite.Web.Features.SideBar.Models;

namespace DntSite.Web.Features.SideBar.Services.Contracts;

public interface ICustomSidebarService : IScopedService
{
    Task<CustomSidebar?> GetCustomSidebarAsync();

    Task AddOrUpdateCustomSidebarAsync(CustomSidebarModel formData);
}
