using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsItemCommentsService : IScopedService
{
    Task AddReplyAsync(int? replyId, int blogPostId, string message, int userId);

    Task DeleteCommentAsync(int? commentId);

    Task EditReplyAsync(int? commentId, string message);

    Task<List<DailyNewsItemComment>> GetRootCommentsOfNewsAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    ValueTask<DailyNewsItemComment?> FindBlogNewsCommentAsync(int commentId);

    DailyNewsItemComment AddBlogNewsComment(DailyNewsItemComment comment);

    Task<PagedResultModel<DailyNewsItemComment>> GetLastPagedBlogNewsCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<List<DailyNewsItemComment>> GetLastBlogNewsCommentsIncludeBlogPostAndUserAsync(int count,
        bool showDeletedItems = false);

    Task<PagedResultModel<DailyNewsItemComment>> GetLastPagedDailyNewsItemCommentsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);
}
