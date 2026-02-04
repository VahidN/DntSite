using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsItemAIBacklogService : IScopedService
{
    Task<PagedResultModel<DailyNewsItemAIBacklog>> GetLastPagedDailyNewsItemAIBacklogsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        bool showProcessedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<List<DailyNewsItemAIBacklog>> GetNotProcessedDailyNewsItemAIBacklogsAsync(CancellationToken ct = default);

    Task<List<int>> GetNotProcessedDailyNewsItemAIBacklogIdsAsync(CancellationToken ct = default);

    ValueTask<DailyNewsItemAIBacklog?> FindDailyNewsItemAIBacklogAsync(int? id);

    Task MarkAsDeletedAsync(int id);

    Task MarkAsApprovedAsync(int id);

    Task MarkAsDeletedOrApprovedAsync(IList<int>? allIds,
        IList<int>? selectedDeleteIds,
        IList<int>? selectedApproveIds);

    Task MarkAsProcessedAsync(int id);

    Task UpdateFetchRetiresAsync(int id);

    DailyNewsItemAIBacklog AddDailyNewsItemAIBacklog(DailyNewsItemAIBacklog data);

    Task AddDailyNewsItemAIBacklogsAsync(string? urls, User? user);

    Task<List<DailyNewsItemAIBacklog>> GetApprovedNotProcessedDailyNewsItemAIBacklogsAsync(CancellationToken ct =
        default);

    Task AddFeedItemsAsDailyNewsItemAIBacklogsAsync(CancellationToken ct = default);
}
