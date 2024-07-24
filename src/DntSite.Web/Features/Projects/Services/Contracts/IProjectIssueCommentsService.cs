using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Services.Contracts;

public interface IProjectIssueCommentsService : IScopedService
{
    Task<ProjectIssueComment?> FindIssueCommentIncludeParentAsync(int commentId);

    Task<List<ProjectIssueComment>> GetRootCommentsOfIssuesAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    ValueTask<ProjectIssueComment?> FindIssueCommentAsync(int commentId);

    ProjectIssueComment AddIssueComment(ProjectIssueComment comment);

    Task<PagedResultModel<ProjectIssueComment>> GetLastPagedIssueCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<List<ProjectIssueComment>> GetLastIssueCommentsIncludeBlogPostAndUserAsync(int count,
        bool showDeletedItems = false);

    Task<List<ProjectIssueComment>> GetLastProjectIssueCommentsIncludeBlogPostAndUserAsync(int projectId,
        int count,
        bool showDeletedItems = false);

    Task<PagedResultModel<ProjectIssueComment>> GetPagedLastIssueCommentsIncludeBlogPostAndUserAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectIssueComment>> GetPagedLastProjectIssueCommentsIncludeBlogPostAndUserAsync(
        int projectId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<ProjectIssueComment>> GetLastPagedProjectIssuesCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task DeleteCommentAsync(int? modelFormCommentId);

    Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    Task AddReplyAsync(int? modelFormCommentId, int modelFormPostId, string modelComment, int currentUserUserId);
}
