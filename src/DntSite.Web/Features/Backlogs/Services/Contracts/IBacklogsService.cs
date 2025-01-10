using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Backlogs.Services.Contracts;

public interface IBacklogsService : IScopedService
{
    public ValueTask<Backlog?> FindBacklogAsync(int id);

    public Task<Backlog?> GetFullBacklogAsync(int id, bool showDeletedItems = false);

    public Backlog AddBacklog(Backlog data);

    public Task<PagedResultModel<Backlog>> GetBacklogsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Backlog>> GetLastPagedBacklogsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    public Task<PagedResultModel<Backlog>> GetLastPagedBacklogsAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Backlog>> GetBacklogsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false,
        bool isInProgress = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<BacklogDetailsModel> BacklogDetailsAsync(int id, bool showDeletedItems = false);

    public Task<BacklogsListModel> GetCountsAsync();

    public Task<bool> HasUserAnotherHalfFinishedAssignedBacklogAsync(int userId);

    public Task CancelOldOnesAsync(CancellationToken cancellationToken);

    public Task<List<Backlog>> GetAllPublicBacklogsOfDateAsync(DateTime date);

    public Task<Backlog?> GetLastActiveBacklogAsync();

    public Task MarkAsDeletedAsync(Backlog? backlog);

    public Task NotifyDeleteChangesAsync(Backlog? backlog, BacklogModel? writeBacklogModel);

    public Task UpdateBacklogAsync(Backlog? backlog, BacklogModel writeBacklogModel);

    public Task<Backlog?> AddBacklogAsync(BacklogModel writeBacklogModel, User? user);

    public Task NotifyAddOrUpdateChangesAsync(Backlog? backlog, BacklogModel? writeBacklogModel);

    public Task UpdateStatAsync(int backlogId, bool isFromFeed);

    public Task<OperationResult>
        TakeBacklogAsync(ManageBacklogModel? data, CurrentUserModel? user, string? siteRootUri);

    public Task<OperationResult> CancelBacklogAsync(ManageBacklogModel? data,
        CurrentUserModel? user,
        string? siteRootUri);

    public Task<OperationResult>
        DoneBacklogAsync(ManageBacklogModel? data, CurrentUserModel? user, string? siteRootUri);

    public Task IndexBackLogsAsync();
}
