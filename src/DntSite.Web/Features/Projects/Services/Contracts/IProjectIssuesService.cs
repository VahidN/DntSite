using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectIssuesService : IScopedService
{
    ValueTask<ProjectIssue?> FindProjectIssueAsync(int id);

    Task<ProjectIssue?> GetProjectIssueAsync(int id, bool showDeletedItems = false);

    ProjectIssue AddProjectIssue(ProjectIssue data);

    Task<PagedResultModel<ProjectIssue>> GetLastPagedAllProjectsIssuesAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingAsync(int projectId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssuePriorityIdAsync(int projectId,
        int? issuePriorityId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssueStatusIdAsync(int projectId,
        int? issueStatusId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssueTypeIdAsync(int projectId,
        int? issueTypeId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<ProjectIssueDetailsModel> GetProjectIssueLastAndNextPostIncludeAuthorTagsAsync(int projectId,
        int issueId,
        bool showDeletedItems = false);

    Task UpdateNumberOfViewsAsync(int issueId, bool fromFeed);

    Task<int> GetAllProjectIssuesCountAsync(bool showDeletedItems = false);

    Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task MarkAsDeletedAsync(ProjectIssue? projectIssue);

    Task NotifyDeleteChangesAsync(ProjectIssue? projectIssue, User? currentUserUser);

    Task UpdateProjectIssueAsync(ProjectIssue? projectIssue, IssueModel? issueModel);

    Task<ProjectIssue?> AddProjectIssueAsync(IssueModel? issueModel, User? user, int projectId);

    Task NotifyAddOrUpdateChangesAsync(ProjectIssue? projectIssue, IssueModel? issueModel, User? user);

    Task IndexProjectIssuesAsync();
}
