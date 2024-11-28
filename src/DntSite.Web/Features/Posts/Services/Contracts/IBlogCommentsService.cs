using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogCommentsService : IScopedService
{
    public Task<List<BlogPostComment>> GetLastBlogCommentsIncludeBlogPostAndUserAsync(int count,
        bool showDeletedItems = false);

    public Task<List<BlogPostComment>> GetRootCommentsOfPostAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    public BlogPostComment AddBlogComment(BlogPostComment data);

    public Task<List<BlogPostComment>> GetLastBlogCommentsOnlyAsync(int count, bool showDeletedItems = false);

    public Task<PagedResultModel<BlogPostComment>> GetLastPagedBlogCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<BlogPostComment>> GetLastPagedBlogCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<BlogPostComment>> GetMyPostsLastCommentsAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public ValueTask<BlogPostComment?> FindBlogCommentAsync(int id);

    public Task<BlogPostComment?> FindBlogCommentIncludeParentAsync(int id);

    public Task<string> FindBlogCommentPostTitleAsync(int commentId);

    public ValueTask<BlogPost?> FindCommentPostAsync(int blogPostId);

    public Task MarkAllOfPostCommentsAsDeletedAsync(int postId);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task DeleteCommentAsync(int? commentId);

    public Task EditReplyAsync(int? commentId, string message);

    public Task AddReplyAsync(int? replyId, int blogPostId, string message, int userId);

    public Task IndexBlogPostCommentsAsync();
}
