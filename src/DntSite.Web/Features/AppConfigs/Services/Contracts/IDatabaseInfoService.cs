using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IDatabaseInfoService : IScopedService
{
    Task<DatabaseInfoModel> GetDatabaseInfoAsync();
}
