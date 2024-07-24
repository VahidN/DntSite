using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface ISearchItemsService : IScopedService
{
    SearchItem AddSearchItem(SearchItem data);

    Task<List<SearchItem>> GetLastSearchItemsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false);

    Task DeleteOldSearchItemsAsync(int daysToKeep = 3);
}
