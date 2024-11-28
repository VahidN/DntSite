using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectIssuesService : IScopedService
{
    public ValueTask<ProjectIssue?> FindProjectIssueAsync(int id);

    public Task<ProjectIssue?> GetProjectIssueAsync(int id, bool showDeletedItems = false);

    public ProjectIssue AddProjectIssue(ProjectIssue data);

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedAllProjectsIssuesAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingAsync(int projectId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssuePriorityIdAsync(
        int projectId,
        int? issuePriorityId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssueStatusIdAsync(int projectId,
        int? issueStatusId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesAsNoTrackingByIssueTypeIdAsync(int projectId,
        int? issueTypeId,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<ProjectIssueDetailsModel> GetProjectIssueLastAndNextPostIncludeAuthorTagsAsync(int projectId,
        int issueId,
        bool showDeletedItems = false);

    public Task UpdateNumberOfViewsAsync(int issueId, bool fromFeed);

    public Task<int> GetAllProjectIssuesCountAsync(bool showDeletedItems = false);

    public Task<PagedResultModel<ProjectIssue>> GetLastPagedProjectIssuesOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task MarkAsDeletedAsync(ProjectIssue? projectIssue);

    public Task NotifyDeleteChangesAsync(ProjectIssue? projectIssue, User? currentUserUser);

    public Task UpdateProjectIssueAsync(ProjectIssue? projectIssue, IssueModel issueModel);

    public Task<ProjectIssue?> AddProjectIssueAsync(IssueModel issueModel, User? user, int projectId);

    public Task NotifyAddOrUpdateChangesAsync(ProjectIssue? projectIssue, IssueModel issueModel, User? user);

    public Task IndexProjectIssuesAsync();
}
