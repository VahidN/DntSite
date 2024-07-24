using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppLogItemsService : IScopedService
{
    Task DeleteAllAsync(LogLevel? logLevel);

    Task DeleteAsync(int logItemId);

    Task DeleteOlderThanAsync(DateTime cutoffDateUtc, LogLevel? logLevel);

    Task<int> GetCountAsync(LogLevel? logLevel);

    Task<PagedResultModel<AppLogItemModel>> GetPagedAppLogItemsAsync(DntQueryBuilderModel state, LogLevel? logLevel);
}
