using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICoursesService : IScopedService
{
    public ValueTask<Course?> FindCourseAsync(int id);

    public Task<List<Course>> GetAllCoursesAsync(bool onlyActive = true);

    public Task<PagedResultModel<Course>> GetLastPagedCoursesAsync(DntQueryBuilderModel state,
        bool onlyActive = true,
        bool showOnlyFinished = true);

    public Task<CourseItemModel> GetCurrentCourseLastAndNextAsync(int id,
        bool onlyActive = true,
        bool showOnlyFinished = true);

    public Task<User?> FindCourseAuthorAsync(int courseId);

    public Course AddCourse(Course item);

    public Task<List<CourseTag>> GetAllCourseTagsListAsNoTrackingAsync(int courseId, int count, bool isActive = true);

    public Task<PagedResultModel<Course>> GetPagedCourseItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Course>> GetUserPagedCourseItemsIncludeUserAndTagsAsync(int userId,
        int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Course>> GetAuthorPagedCourseItemsIncludeUserAndTagsAsync(string name,
        int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Course>> GetLastCoursesByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<Course?> FindCourseIncludeTagsAndUserAsync(int id);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<List<Course>> GetAllUserCoursesAsync(int userId, bool onlyActive = true);

    public Task AddUserToCourseAsync(Course data, string username);

    public Task RemoveUserFromCourseAsync(Course data, string username);

    public Task<List<User>> GetCourseAllowedUsersAsync(int courseId, bool onlyActive = true);

    public Task<OperationResult> HasUserAccessToThisCourseForReadingAsync(int courseId, CurrentUserModel? currentUser);

    public Task<PagedResultModel<Course>> GetAllPagedCourseItemsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task MarkAsDeletedAsync(Course? course);

    public Task NotifyDeleteChangesAsync(Course? course);

    public Task UpdateCourseItemAsync(Course? course, CourseModel writeCourseModel);

    public Task<Course?> AddCourseItemAsync(CourseModel writeCourseModel, User? user);

    public Task NotifyAddOrUpdateChangesAsync(Course? course, CourseModel writeCourseModel, User? user);

    public Task IndexCoursesAsync();
}
