using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Backlogs.Services.Contracts;

public interface IBacklogsService : IScopedService
{
    ValueTask<Backlog?> FindBacklogAsync(int id);

    Task<Backlog?> GetFullBacklogAsync(int id, bool showDeletedItems = false);

    Backlog AddBacklog(Backlog data);

    Task<PagedResultModel<Backlog>> GetBacklogsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Backlog>>
        GetLastPagedBacklogsAsync(DntQueryBuilderModel state, bool showDeletedItems = false);

    Task<PagedResultModel<Backlog>> GetLastPagedBacklogsAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Backlog>> GetBacklogsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false,
        bool isInProgress = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<BacklogDetailsModel> BacklogDetailsAsync(int id, bool showDeletedItems = false);

    Task<BacklogsListModel> GetCountsAsync();

    Task<bool> HasUserAnotherHalfFinishedAssignedBacklogAsync(int userId);

    Task CancelOldOnesAsync();

    Task<List<Backlog>> GetAllPublicBacklogsOfDateAsync(DateTime date);

    Task<Backlog?> GetLastActiveBacklogAsync();

    Task MarkAsDeletedAsync(Backlog? backlog);

    Task NotifyDeleteChangesAsync(Backlog? backlog, BacklogModel? writeBacklogModel);

    Task UpdateBacklogAsync(Backlog? backlog, BacklogModel writeBacklogModel);

    Task<Backlog?> AddBacklogAsync(BacklogModel writeBacklogModel, User? user);

    Task NotifyAddOrUpdateChangesAsync(Backlog? backlog, BacklogModel? writeBacklogModel);

    Task UpdateStatAsync(int backlogId, bool isFromFeed);

    Task<OperationResult> TakeBacklogAsync(ManageBacklogModel? data, CurrentUserModel? user, string? siteRootUri);

    Task<OperationResult> CancelBacklogAsync(ManageBacklogModel? data, CurrentUserModel? user, string? siteRootUri);

    Task<OperationResult> DoneBacklogAsync(ManageBacklogModel? data, CurrentUserModel? user, string? siteRootUri);
}
