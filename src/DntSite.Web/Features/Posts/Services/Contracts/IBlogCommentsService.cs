using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogCommentsService : IScopedService
{
    Task<List<BlogPostComment>> GetLastBlogCommentsIncludeBlogPostAndUserAsync(int count,
        bool showDeletedItems = false);

    Task<List<BlogPostComment>> GetRootCommentsOfPostAsync(int postId, int count = 1000, bool showDeletedItems = false);

    BlogPostComment AddBlogComment(BlogPostComment data);

    Task<List<BlogPostComment>> GetLastBlogCommentsOnlyAsync(int count, bool showDeletedItems = false);

    Task<PagedResultModel<BlogPostComment>> GetLastPagedBlogCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<BlogPostComment>> GetLastPagedBlogCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<BlogPostComment>> GetMyPostsLastCommentsAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    ValueTask<BlogPostComment?> FindBlogCommentAsync(int id);

    Task<BlogPostComment?> FindBlogCommentIncludeParentAsync(int id);

    Task<string> FindBlogCommentPostTitleAsync(int commentId);

    ValueTask<BlogPost?> FindCommentPostAsync(int? blogPostId);

    Task MarkAllOfPostCommentsAsDeletedAsync(int postId);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task DeleteCommentAsync(int? commentId);

    Task EditReplyAsync(int? commentId, string message);

    Task AddReplyAsync(int? replyId, int? blogPostId, string message, int userId);

    Task IndexBlogPostCommentsAsync();
}
