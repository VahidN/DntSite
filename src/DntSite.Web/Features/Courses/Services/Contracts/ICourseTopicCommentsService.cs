using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTopicCommentsService : IScopedService
{
    ValueTask<CourseTopicComment?> FindTopicCommentAsync(int commentId);

    Task<CourseTopicComment?> FindTopicCommentIncludeParentAsync(int commentId);

    CourseTopicComment AddTopicComment(CourseTopicComment comment);

    Task<List<CourseTopicComment>> GetRootCommentsOfTopicsAsync(int postId, int count = 1000, bool onlyActive = true);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<List<CourseTopicComment>> GetFlatCommentsOfTopicsAsync(int postId, int count = 1000, bool onlyActive = true);

    Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsOfCourseAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsAsNoTrackingAsync(string userFriendlyName,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<List<CourseTopicComment>> GetLastTopicCommentsIncludePostAndUserAsync(int count, bool onlyActives = true);

    Task DeleteCommentAsync(int? modelFormCommentId);

    Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    Task AddReplyAsync(int? modelFormCommentId, int modelFormPostId, string modelComment, int currentUserUserId);
}
