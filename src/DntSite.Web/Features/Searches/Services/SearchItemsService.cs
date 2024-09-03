using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Searches.Entities;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Services;

public class SearchItemsService(IUnitOfWork uow, IAntiXssService antiXssService) : ISearchItemsService
{
    private readonly DbSet<SearchItem> _searchItems = uow.DbSet<SearchItem>();

    public async Task<SearchItem?> AddSearchItemAsync(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var result = _searchItems.Add(new SearchItem
            {
                Text = antiXssService.GetSanitizedHtml(text)
            })
            .Entity;

        await uow.SaveChangesAsync();

        return result;
    }

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
