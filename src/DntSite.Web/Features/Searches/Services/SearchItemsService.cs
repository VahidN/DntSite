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
    IUAParserService uaParserService,
    ILockerService lockerService,
    ILogger<SearchItemsService> logger) : ISearchItemsService
{
    private readonly DbSet<SearchItem> _searchItems = uow.DbSet<SearchItem>();

    public async Task SaveSearchItemAsync(string? text)
    {
        text = text?.Trim();

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        if (await IgnoreSearchItemOfCurrentUserAsync())
        {
            return;
        }

        using var @lock = await lockerService.LockAsync<SearchItemsService>();

        var sanitizedHtml = antiXssService.GetSanitizedHtml(text);

        var lastTryOfCurrentUserToday = await GetLastTryOfCurrentUserTodayAsync(sanitizedHtml);

        if (lastTryOfCurrentUserToday is not null)
        {
            return;
        }

        _searchItems.Add(new SearchItem
        {
            Text = sanitizedHtml
        });

        await uow.SaveChangesAsync();
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

    public async Task RemoveSearchItemAsync(int id)
    {
        var item = await _searchItems.FindAsync(id);

        if (item is null)
        {
            return;
        }

        item.IsDeleted = true;
        await uow.SaveChangesAsync();

        logger.LogWarning(message: "Deleted a SearchItem record with Id={Id} and Text={Text}", item.Id, item.Text);
    }

    private async Task<bool> IgnoreSearchItemOfCurrentUserAsync()
        => await currentUserService.IsCurrentUserSpiderAsync() || await currentUserService.IsCurrentUserAdminAsync();

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
