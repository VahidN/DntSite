using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Searches.Entities;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Services;

public class SearchItemsService(IUnitOfWork uow) : ISearchItemsService
{
    private readonly DbSet<SearchItem> _searchItems = uow.DbSet<SearchItem>();

    public SearchItem AddSearchItem(SearchItem data) => _searchItems.Add(data).Entity;

    public Task<List<SearchItem>> GetLastSearchItemsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        return _searchItems.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public async Task DeleteOldSearchItemsAsync(int daysToKeep = 3)
    {
        var date = DateTime.UtcNow.AddDays(-daysToKeep);
        var list = await _searchItems.Where(x => x.Audit.CreatedAt < date).ToListAsync();
        _searchItems.RemoveRange(list);
        await uow.SaveChangesAsync();
    }
}
