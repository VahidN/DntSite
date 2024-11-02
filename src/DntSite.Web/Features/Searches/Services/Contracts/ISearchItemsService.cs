using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Entities;
using DntSite.Web.Features.Searches.Models;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface ISearchItemsService : IScopedService
{
    Task<SearchItem?> AddSearchItemAsync(string? text);

    Task RemoveSearchItemAsync(int id);

    Task<PagedResultModel<SearchItemModel>> GetPagedSearchItemsAsync(int pageNumber,
        int recordsPerPage,
        bool showDeletedItems = false);

    Task DeleteOldSearchItemsAsync(int daysToKeep = 3);
}
