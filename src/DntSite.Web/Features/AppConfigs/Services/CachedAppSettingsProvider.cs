using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.AppConfigs.Services;

public class CachedAppSettingsProvider(IServiceProvider serviceProvider, ILockerService lockerService)
    : ICachedAppSettingsProvider
{
    private AppSetting? _appSetting;

    public async Task<AppSetting> GetAppSettingsAsync()
    {
        if (_appSetting is not null)
        {
            return _appSetting;
        }

        using var @lock = await lockerService.LockAsync<CachedAppSettingsProvider>();

        _appSetting = await serviceProvider.RunScopedServiceAsync<IUnitOfWork, AppSetting>(async uow =>
        {
            return await uow.DbSet<AppSetting>().AsNoTracking().OrderBy(x => x.Id).FirstOrDefaultAsync() ??
                   new AppSetting
                   {
                       BlogName = "DNT",
                       SiteRootUri = "/"
                   };
        });

        return _appSetting;
    }

    public void InvalidateAppSettings() => _appSetting = null;
}
