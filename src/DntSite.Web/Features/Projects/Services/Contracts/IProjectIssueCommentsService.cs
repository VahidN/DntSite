using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectIssueCommentsService : IScopedService
{
    public Task<ProjectIssueComment?> FindIssueCommentIncludeParentAsync(int commentId);

    public Task<List<ProjectIssueComment>> GetRootCommentsOfIssuesAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    public ValueTask<ProjectIssueComment?> FindIssueCommentAsync(int commentId);

    public ProjectIssueComment AddIssueComment(ProjectIssueComment comment);

    public Task<PagedResultModel<ProjectIssueComment>> GetLastPagedIssueCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<List<ProjectIssueComment>> GetLastIssueCommentsIncludeBlogPostAndUserAsync(int count,
        bool showDeletedItems = false);

    public Task<List<ProjectIssueComment>> GetLastProjectIssueCommentsIncludeBlogPostAndUserAsync(int projectId,
        int count,
        bool showDeletedItems = false);

    public Task<PagedResultModel<ProjectIssueComment>> GetPagedLastIssueCommentsIncludeBlogPostAndUserAsync(
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectIssueComment>> GetPagedLastProjectIssueCommentsIncludeBlogPostAndUserAsync(
        int projectId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<ProjectIssueComment>> GetLastPagedProjectIssuesCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task DeleteCommentAsync(int? modelFormCommentId);

    public Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    public Task AddReplyAsync(int? modelFormCommentId, int modelFormPostId, string modelComment, int currentUserUserId);

    public Task IndexProjectIssueCommentsAsync();
}
