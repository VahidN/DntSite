using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsItemCommentsService : IScopedService
{
    public Task AddReplyAsync(int? replyId, int blogPostId, string message, int userId);

    public Task DeleteCommentAsync(int? commentId);

    public Task EditReplyAsync(int? commentId, string message);

    public Task<List<DailyNewsItemComment>> GetRootCommentsOfNewsAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    public ValueTask<DailyNewsItemComment?> FindBlogNewsCommentAsync(int commentId);

    public Task<DailyNewsItemComment?> FindBlogNewsCommentIncludeParentAsync(int commentId);

    public DailyNewsItemComment AddBlogNewsComment(DailyNewsItemComment comment);

    public Task<PagedResultModel<DailyNewsItemComment>> GetLastPagedBlogNewsCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<List<DailyNewsItemComment>> GetLastBlogNewsCommentsIncludeBlogPostAndUserAsync(int count,
        bool showDeletedItems = false);

    public Task<PagedResultModel<DailyNewsItemComment>> GetLastPagedDailyNewsItemCommentsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task IndexDailyNewsItemCommentsAsync();
}
