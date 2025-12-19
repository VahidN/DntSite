using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.SideBar.Entities;
using DntSite.Web.Features.SideBar.Models;
using DntSite.Web.Features.SideBar.Services.Contracts;

namespace DntSite.Web.Features.SideBar.Services;

public class CustomSidebarService(IUnitOfWork uow, IAppAntiXssService antiXssService) : ICustomSidebarService
{
    private readonly DbSet<CustomSidebar> _customSidebar = uow.DbSet<CustomSidebar>();

    public Task<CustomSidebar?> GetCustomSidebarAsync() => _customSidebar.OrderBy(x => x.Id).FirstOrDefaultAsync();

    public async Task AddOrUpdateCustomSidebarAsync(CustomSidebarModel? formData)
    {
        ArgumentNullException.ThrowIfNull(formData);

        var cfg = await GetCustomSidebarAsync();

        var description = antiXssService.GetSanitizedHtml(formData.Description);

        if (cfg is null)
        {
            _customSidebar.Add(new CustomSidebar
            {
                Description = description,
                IsPublic = formData.IsPublic
            });
        }
        else
        {
            cfg.Description = description;
            cfg.IsPublic = formData.IsPublic;
        }

        await uow.SaveChangesAsync();
    }
}
