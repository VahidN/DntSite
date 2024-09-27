using AsyncKeyedLock;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Searches.Entities;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Searches.Services;

public class SearchItemsService(
    IUnitOfWork uow,
    IAppAntiXssService antiXssService,
    ICurrentUserService currentUserService,
    IHttpContextAccessor httpContextAccessor,
    IUAParserService uaParserService) : ISearchItemsService
{
    private static readonly AsyncNonKeyedLocker Locker = new(maxCount: 1);
    private static readonly TimeSpan LockTimeout = TimeSpan.FromSeconds(value: 3);

    private readonly DbSet<SearchItem> _searchItems = uow.DbSet<SearchItem>();

    public async Task<SearchItem?> AddSearchItemAsync(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        using var @lock = await Locker.LockAsync(LockTimeout);

        var sanitizedHtml = antiXssService.GetSanitizedHtml(text);

        var lastTryOfCurrentUserToday = await GetLastTryOfCurrentUserTodayAsync(sanitizedHtml);

        if (lastTryOfCurrentUserToday is not null)
        {
            return lastTryOfCurrentUserToday;
        }

        var result = _searchItems.Add(new SearchItem
            {
                Text = sanitizedHtml
            })
            .Entity;

        await uow.SaveChangesAsync();

        return result;
    }

    public async Task<PagedResultModel<SearchItemModel>> GetPagedSearchItemsAsync(int pageNumber,
        int recordsPerPage,
        bool showDeletedItems = false)
    {
        var query = _searchItems.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.IsDeleted == showDeletedItems)
            .OrderByDescending(x => x.Id);

        var pagedItems = await query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);

        var data = new List<SearchItemModel>();

        foreach (var pagedItem in pagedItems.Data)
        {
            data.Add(new SearchItemModel
            {
                SearchItem = pagedItem,
                ClientInfo = await uaParserService.GetClientInfoAsync(pagedItem.Audit.CreatedByUserAgent)
            });
        }

        return new PagedResultModel<SearchItemModel>
        {
            TotalItems = pagedItems.TotalItems,
            Data = data
        };
    }

    public async Task DeleteOldSearchItemsAsync(int daysToKeep = 3)
    {
        var date = DateTime.UtcNow.AddDays(-daysToKeep);
        var list = await _searchItems.Where(x => x.Audit.CreatedAt < date).ToListAsync();
        _searchItems.RemoveRange(list);
        await uow.SaveChangesAsync();
    }

    private Task<SearchItem?> GetLastTryOfCurrentUserTodayAsync(string text)
    {
        var userId = currentUserService.GetCurrentUserId();

        var today = DateTime.UtcNow.Date;

        if (userId.HasValue)
        {
            return _searchItems.FirstOrDefaultAsync(item => item.Text == text && item.UserId == userId.Value &&
                                                            item.Audit.CreatedAt.Date == today);
        }

        var ip = httpContextAccessor.HttpContext?.GetIP();

        return _searchItems.FirstOrDefaultAsync(item => item.Text == text && item.Audit.CreatedByUserIp == ip &&
                                                        item.Audit.CreatedAt.Date == today);
    }
}
