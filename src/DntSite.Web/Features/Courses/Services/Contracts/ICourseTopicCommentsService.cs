using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTopicCommentsService : IScopedService
{
    public ValueTask<CourseTopicComment?> FindTopicCommentAsync(int commentId);

    public Task<CourseTopicComment?> FindTopicCommentIncludeParentAsync(int commentId);

    public CourseTopicComment AddTopicComment(CourseTopicComment comment);

    public Task<List<CourseTopicComment>> GetRootCommentsOfTopicsAsync(int postId,
        int count = 1000,
        bool onlyActive = true);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<List<CourseTopicComment>> GetFlatCommentsOfTopicsAsync(int postId,
        int count = 1000,
        bool onlyActive = true);

    public Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsOfCourseAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsAsNoTrackingAsync(string userFriendlyName,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<CourseTopicComment>> GetLastPagedCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<List<CourseTopicComment>> GetLastTopicCommentsIncludePostAndUserAsync(int count,
        bool onlyActives = true);

    public Task DeleteCommentAsync(int? modelFormCommentId);

    public Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    public Task AddReplyAsync(int? modelFormCommentId, int modelFormPostId, string modelComment, int currentUserUserId);

    public Task IndexCourseTopicCommentsAsync();
}
