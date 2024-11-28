using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTopicsService : IScopedService
{
    public Task<bool> CanUserAddCourseTopicAsync(CurrentUserModel? user, int courseId);

    public ValueTask<CourseTopic?> FindCourseTopicAsync(int id);

    public Task<CourseTopic?> FindCourseTopicAsync(Guid id);

    public Task<PagedResultModel<CourseTopic>> GetPagedAllCoursesTopicsAsync(int pageNumber,
        bool isMain,
        int recordsPerPage = 5,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public CourseTopic AddCourseTopic(CourseTopic topic);

    public Task<PagedResultModel<CourseTopic>> GetPagedCourseTopicsAsync(int courseId,
        int pageNumber,
        bool isMain,
        int recordsPerPage = 5,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<CourseTopicModel?> GetTopicAsync(Guid topicId, bool onlyActive = true);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<CourseTopic?> FindTopicAsync(Guid topicId, bool onlyActive = true);

    public Task UpdateNumberOfViewsAsync(Guid topicId, bool fromFeed, bool onlyActive = true);

    public Task<List<CourseTopic>> GetAllPublicTopicsOfDateAsync(DateTime date);

    public Task<List<CourseTopic>> GetPagedAllActiveCoursesTopicsAsync();

    public Task<List<CourseTopic>> GetPagedAllActiveCoursesTopicsAsync(int count, bool onlyActive = true);

    public Task<List<CourseTopic>> GetAllCourseTopicsAsync(int courseId, bool onlyActive = true);

    public Task<int> GetAllCourseTopicsCountAsync(bool onlyActive = true);

    public Task MarkAsDeletedAsync(CourseTopic? courseTopic);

    public Task UpdateCourseTopicItemAsync(CourseTopic? courseTopic, CourseTopicItemModel writeCourseItemModel);

    public Task<CourseTopic?> AddCourseTopicItemAsync(CourseTopicItemModel writeCourseItemModel,
        User? user,
        int courseId);

    public Task NotifyAddOrUpdateChangesAsync(CourseTopic? courseTopic);

    public Task IndexCourseTopicsAsync();
}
