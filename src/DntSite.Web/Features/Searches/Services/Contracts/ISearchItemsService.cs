using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface ISearchItemsService : IScopedService
{
    public Task SaveSearchItemAsync(string? text);

    public Task RemoveSearchItemAsync(int id);

    public Task<PagedResultModel<SearchItemModel>> GetPagedSearchItemsAsync(int pageNumber,
        int recordsPerPage,
        bool showDeletedItems = false);

    public Task DeleteOldSearchItemsAsync(int daysToKeep = 3);
}
