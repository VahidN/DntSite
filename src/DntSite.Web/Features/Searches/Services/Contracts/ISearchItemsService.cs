using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface ISearchItemsService : IScopedService
{
    Task<SearchItem?> AddSearchItemAsync(string? text);

    Task<PagedResultModel<SearchItem>> GetPagedSearchItemsAsync(int pageNumber,
        int recordsPerPage,
        bool showDeletedItems = false);

    Task DeleteOldSearchItemsAsync(int daysToKeep = 3);
}
