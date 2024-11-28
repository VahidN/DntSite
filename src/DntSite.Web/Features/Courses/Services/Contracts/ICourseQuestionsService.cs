using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseQuestionsService : IScopedService
{
    public CourseQuestion AddCourseQuestion(CourseQuestion courseQuestion);

    public ValueTask<CourseQuestion?> FindCourseQuestionAsync(int courseQuestionId);

    public Task<PagedResultModel<CourseQuestion>> GetLastPagedCourseQuestionsAsNoTrackingAsync(int courseId,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<CourseQuestionDetailsModel> GetCourseQuestionLastAndNextPostIncludeAuthorTagsAsync(int courseId,
        int questionId,
        bool onlyShowActives = true);

    public Task UpdateNumberOfViewsAsync(int id, bool fromFeed);
}
