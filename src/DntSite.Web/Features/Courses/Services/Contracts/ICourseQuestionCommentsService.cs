using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseQuestionCommentsService : IScopedService
{
    public Task<List<CourseQuestionComment>> GetRootCommentsOfQuestionsAsync(int postId,
        int count = 1000,
        bool onlyActives = true);

    public CourseQuestionComment AddCourseQuestionComment(CourseQuestionComment comment);

    public ValueTask<CourseQuestionComment?> FindCourseQuestionCommentAsync(int commentId);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<PagedResultModel<CourseQuestionComment>> GetLastPagedCommentsAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActives = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);
}
