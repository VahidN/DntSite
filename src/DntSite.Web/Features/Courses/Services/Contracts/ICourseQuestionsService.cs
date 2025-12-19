using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseQuestionsService : IScopedService
{
    CourseQuestion AddCourseQuestion(CourseQuestion courseQuestion);

    ValueTask<CourseQuestion?> FindCourseQuestionAsync(int courseQuestionId);

    Task<PagedResultModel<CourseQuestion>> GetLastPagedCourseQuestionsAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<CourseQuestionDetailsModel> GetCourseQuestionLastAndNextPostIncludeAuthorTagsAsync(int courseId,
        int questionId,
        bool onlyShowActives = true);

    Task UpdateNumberOfViewsAsync(int id, bool fromFeed);
}
