using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTopicsService : IScopedService
{
    Task<bool> CanUserAddCourseTopicAsync(CurrentUserModel? user, int courseId);

    ValueTask<CourseTopic?> FindCourseTopicAsync(int id);

    Task<CourseTopic?> FindCourseTopicAsync(Guid id);

    Task<PagedResultModel<CourseTopic>> GetPagedAllCoursesTopicsAsync(int pageNumber,
        bool isMain,
        int recordsPerPage = 5,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    CourseTopic AddCourseTopic(CourseTopic topic);

    Task<PagedResultModel<CourseTopic>> GetPagedCourseTopicsAsync(int courseId,
        int pageNumber,
        bool isMain,
        int recordsPerPage = 5,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<CourseTopicModel?> GetTopicAsync(Guid topicId, bool onlyActive = true);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<CourseTopic?> FindTopicAsync(Guid topicId, bool onlyActive = true);

    Task UpdateNumberOfViewsAsync(Guid topicId, bool fromFeed, bool onlyActive = true);

    Task<List<CourseTopic>> GetAllPublicTopicsOfDateAsync(DateTime date);

    Task<List<CourseTopic>> GetPagedAllActiveCoursesTopicsAsync();

    Task<List<CourseTopic>> GetPagedAllActiveCoursesTopicsAsync(int count, bool onlyActive = true);

    Task<List<CourseTopic>> GetAllCourseTopicsAsync(int courseId, bool onlyActive = true);

    Task<int> GetAllCourseTopicsCountAsync(bool onlyActive = true);

    Task MarkAsDeletedAsync(CourseTopic? courseTopic);

    Task UpdateCourseTopicItemAsync(CourseTopic? courseTopic, CourseTopicItemModel writeCourseItemModel);

    Task<CourseTopic?> AddCourseTopicItemAsync(CourseTopicItemModel writeCourseItemModel, User? user, int courseId);

    Task NotifyAddOrUpdateChangesAsync(CourseTopic? courseTopic);

    Task IndexCourseTopicsAsync();
}
