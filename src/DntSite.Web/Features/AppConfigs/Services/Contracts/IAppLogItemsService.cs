using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.AppConfigs.Services.Contracts;

public interface IAppLogItemsService : IScopedService
{
    public Task DeleteAllAsync(LogLevel? logLevel);

    public Task DeleteAsync(int logItemId);

    public Task DeleteOlderThanAsync(DateTime cutoffDateUtc, LogLevel? logLevel);

    public Task<int> GetCountAsync(LogLevel? logLevel);

    public Task<PagedResultModel<AppLogItemModel>> GetPagedAppLogItemsAsync(DntQueryBuilderModel state,
        LogLevel? logLevel);
}
