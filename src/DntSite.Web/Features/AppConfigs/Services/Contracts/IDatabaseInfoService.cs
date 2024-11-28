using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IDatabaseInfoService : IScopedService
{
    public Task<DatabaseInfoModel> GetDatabaseInfoAsync();

    public Task<bool> NeedsShrinkDatabaseAsync();

    public void ShrinkDatabase();
}
