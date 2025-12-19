using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICoursesService : IScopedService
{
    ValueTask<Course?> FindCourseAsync(int id);

    Task<List<Course>> GetAllCoursesAsync(bool onlyActive = true);

    Task<PagedResultModel<Course>> GetLastPagedCoursesAsync(DntQueryBuilderModel state,
        bool onlyActive = true,
        bool showOnlyFinished = true);

    Task<CourseItemModel> GetCurrentCourseLastAndNextAsync(int id,
        bool onlyActive = true,
        bool showOnlyFinished = true);

    Task<User?> FindCourseAuthorAsync(int courseId);

    Course AddCourse(Course item);

    Task<List<CourseTag>> GetAllCourseTagsListAsNoTrackingAsync(int courseId, int count, bool isActive = true);

    Task<PagedResultModel<Course>> GetPagedCourseItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Course>> GetUserPagedCourseItemsIncludeUserAndTagsAsync(int userId,
        int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Course>> GetAuthorPagedCourseItemsIncludeUserAndTagsAsync(string name,
        int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Course>> GetLastCoursesByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<Course?> FindCourseIncludeTagsAndUserAsync(int id);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<List<Course>> GetAllUserCoursesAsync(int userId, bool onlyActive = true);

    Task AddUserToCourseAsync(Course data, string username);

    Task RemoveUserFromCourseAsync(Course data, string username);

    Task<List<User>> GetCourseAllowedUsersAsync(int courseId, bool onlyActive = true);

    Task<OperationResult> HasUserAccessToThisCourseForReadingAsync(int courseId, CurrentUserModel? currentUser);

    Task<PagedResultModel<Course>> GetAllPagedCourseItemsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task MarkAsDeletedAsync(Course? course);

    Task NotifyDeleteChangesAsync(Course? course);

    Task UpdateCourseItemAsync(Course? course, CourseModel? writeCourseModel);

    Task<Course?> AddCourseItemAsync(CourseModel? writeCourseModel, User? user);

    Task NotifyAddOrUpdateChangesAsync(Course? course, CourseModel? writeCourseModel, User? user);

    Task IndexCoursesAsync();
}
