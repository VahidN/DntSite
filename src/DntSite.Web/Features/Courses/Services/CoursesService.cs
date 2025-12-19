using AutoMapper;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.ModelsMappings;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Courses.Services;

public class CoursesService(
    IUnitOfWork uow,
    IUsersInfoService usersService,
    IUserRatingsService userRatingsService,
    ICoursesEmailsService emailsService,
    IMapper mapper,
    ITagsService tagsService,
    IStatService statService,
    IFullTextSearchService fullTextSearchService,
    ILogger<CoursesService> logger) : ICoursesService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<Course, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<Course> _courses = uow.DbSet<Course>();

    public ValueTask<Course?> FindCourseAsync(int id) => _courses.FindAsync(id);

    public Course AddCourse(Course item) => _courses.Add(item).Entity;

    public Task<List<Course>> GetAllUserCoursesAsync(int userId, bool onlyActive = true)
        => _courses.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActive && x.UserId == userId)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

    public Task<List<Course>> GetAllCoursesAsync(bool onlyActive = true)
        => _courses.Where(x => x.IsDeleted != onlyActive)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

    public Task<Course?> FindCourseIncludeTagsAndUserAsync(int id)
        => _courses.Where(x => x.Id == id)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<CourseReaction, Course>(fkId, reactionType, fromUserId);

    public Task<PagedResultModel<Course>> GetLastCoursesByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _courses.AsNoTracking() from t in b.Tags where t.Name == tag select b;

        query = query.Include(x => x.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Include(blogPost => blogPost.CourseTopics)
            .Where(x => x.IsDeleted != onlyActive);

        if (showOnlyFinished)
        {
            query = query.Where(x => x.IsReadyToPublish == showOnlyFinished);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task AddUserToCourseAsync(Course data, string username)
    {
        ArgumentNullException.ThrowIfNull(data);

        var user = await usersService.FindUserByFriendlyNameAsync(username);

        if (user?.IsActive != true)
        {
            return;
        }

        if (data.CourseAllowedUsers is null || data.CourseAllowedUsers.Count == 0)
        {
            data.CourseAllowedUsers = [];
        }

        if (data.CourseAllowedUsers.Any(x => x.Id == user.Id))
        {
            return;
        }

        data.CourseAllowedUsers.Add(user);
    }

    public async Task RemoveUserFromCourseAsync(Course data, string username)
    {
        ArgumentNullException.ThrowIfNull(data);

        var user = await usersService.FindUserByFriendlyNameAsync(username);

        if (user?.IsActive != true)
        {
            return;
        }

        if (data.CourseAllowedUsers is null || data.CourseAllowedUsers.Count == 0)
        {
            return;
        }

        if (!data.CourseAllowedUsers.Any(x => x.Id == user.Id))
        {
            return;
        }

        data.CourseAllowedUsers.Remove(user);
    }

    public Task<List<User>> GetCourseAllowedUsersAsync(int courseId, bool onlyActive = true)
        => _courses.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActive && x.Id == courseId)
            .SelectMany(x => x.CourseAllowedUsers)
            .ToListAsync();

    public async Task<OperationResult> HasUserAccessToThisCourseForReadingAsync(int courseId,
        CurrentUserModel? currentUser)
    {
        if (currentUser is null)
        {
            return ("این مطلب فقط برای کاربران سایت قابل دسترسی است.", OperationStat.Failed);
        }

        if (currentUser.IsAdmin)
        {
            return OperationStat.Succeeded;
        }

        var course = await FindCourseAsync(courseId);

        if (course is null)
        {
            return ("این دوره یافت نشد.", OperationStat.Failed);
        }

        if (course is { IsDeleted: true })
        {
            return ("این دوره هنوز فعال نشده است.", OperationStat.Failed);
        }

        if (!course.IsReadyToPublish)
        {
            return ("این دوره هنوز آماده انتشار نیست", OperationStat.Failed);
        }

        if (course.Perm == CourseType.FreeForAll)
        {
            return OperationStat.Succeeded;
        }

        if (!currentUser.IsAuthenticated)
        {
            return ("این مطلب فقط برای کاربران سایت قابل دسترسی است.", OperationStat.Failed);
        }

        var userId = currentUser.UserId ?? 0;
        var user = await usersService.FindUserAsync(userId);

        if (user is not { IsActive: true })
        {
            return ("چنین کاربری یافت نشد یا فعال نیست.", OperationStat.Failed);
        }

        if (course.UserId is not null && course.UserId.Value == userId)
        {
            return OperationStat.Succeeded;
        }

        switch (course.Perm)
        {
            case CourseType.FreeForAll:
                return OperationStat.Succeeded;

            case CourseType.FreeForWriters:
                if (course is { IsFree: true, NumberOfPostsRequired: > 0 })
                {
                    if (course.NumberOfMonthsRequired > 0)
                    {
                        return await CheckNumberOfMonthsRequiredAsync(userId, course);
                    }

                    if (user.UserStat.NumberOfPosts >= course.NumberOfPostsRequired)
                    {
                        return OperationStat.Succeeded;
                    }

                    return (
                        string.Create(CultureInfo.InvariantCulture,
                            $"برای دسترسی به این دوره نیاز به {course.NumberOfPostsRequired} مطلب ارسالی است. تعداد مطالب ارسالی شما: {user.UserStat.NumberOfPosts}"),
                        OperationStat.Failed);
                }

                break;

            case CourseType.FreeForActiveUsers:
                if (course is { IsFree: true, NumberOfTotalRatingsRequired: > 0 })
                {
                    if (course.NumberOfMonthsTotalRatingsRequired > 0)
                    {
                        var numberOfTotalRatings = await GetTotalNumberOfRatingsValueAsync(userId, course);

                        if (numberOfTotalRatings >= course.NumberOfTotalRatingsRequired)
                        {
                            return OperationStat.Succeeded;
                        }

                        return (
                            string.Create(CultureInfo.InvariantCulture,
                                $"برای دسترسی به این دوره نیاز به دریافت {course.NumberOfTotalRatingsRequired} امتیاز در طی {course.NumberOfMonthsTotalRatingsRequired} ماه قبل است. تعداد امتیازهای دریافتی شما در طی {course.NumberOfMonthsTotalRatingsRequired} ماه قبل: {numberOfTotalRatings}"),
                            OperationStat.Failed);
                    }

                    if (user.Rating.TotalRating >= course.NumberOfTotalRatingsRequired)
                    {
                        return OperationStat.Succeeded;
                    }

                    return (
                        string.Create(CultureInfo.InvariantCulture,
                            $"برای دسترسی به این دوره نیاز به {course.NumberOfTotalRatingsRequired} امتیاز دریافتی است. تعداد امتیازهای دریافتی شما: {user.Rating.TotalRating}"),
                        OperationStat.Failed);
                }

                break;

            case CourseType.IsNotFree:
                if (course.CourseAllowedUsers.Any(x => x.Id == user.Id))
                {
                    return OperationStat.Succeeded;
                }

                break;
        }

        return OperationStat.Failed;
    }

    public async Task<User?> FindCourseAuthorAsync(int courseId)
    {
        var course = await FindCourseAsync(courseId);

        return course is { IsDeleted: true } ? null : course?.User;
    }

    public Task<List<CourseTag>> GetAllCourseTagsListAsNoTrackingAsync(int courseId, int count, bool isActive = true)
        => _courses.AsNoTracking()
            .Where(x => x.IsDeleted != isActive && x.Id == courseId)
            .SelectMany(x => x.Tags)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .ToListAsync();

    public Task<PagedResultModel<Course>> GetPagedCourseItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _courses.Where(x => x.IsDeleted != onlyActive)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        if (showOnlyFinished)
        {
            query = query.Where(x => x.IsReadyToPublish == showOnlyFinished);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Course>> GetUserPagedCourseItemsIncludeUserAndTagsAsync(int userId,
        int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _courses.Where(x => x.IsDeleted != onlyActive && x.UserId == userId)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Course>> GetAuthorPagedCourseItemsIncludeUserAndTagsAsync(string name,
        int pageNumber,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _courses.Where(x => x.IsDeleted != onlyActive && x.User!.FriendlyName == name)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Include(blogPost => blogPost.CourseTopics)
            .AsNoTracking();

        if (showOnlyFinished)
        {
            query = query.Where(x => x.IsReadyToPublish == showOnlyFinished);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Course>> GetLastPagedCoursesAsync(DntQueryBuilderModel state,
        bool onlyActive = true,
        bool showOnlyFinished = true)
    {
        var query = _courses.Where(x => x.IsDeleted != onlyActive)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Include(x => x.CourseTopics)
            .AsNoTracking();

        if (showOnlyFinished)
        {
            query = query.Where(x => x.IsReadyToPublish == showOnlyFinished);
        }

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(Course.Id), [
            .. GridifyMapings.GetDefaultMappings<Course>(), new GridifyMap<Course>
            {
                From = CoursesProfiles.CourseTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public async Task<CourseItemModel> GetCurrentCourseLastAndNextAsync(int id,
        bool onlyActive = true,
        bool showOnlyFinished = true) // این شماره‌ها پشت سر هم نیستند
    {
        var query = _courses.AsNoTracking();

        if (showOnlyFinished)
        {
            query = query.Where(x => x.IsReadyToPublish == showOnlyFinished);
        }

        return new CourseItemModel
        {
            CurrentCourse =
                await query.Where(x => x.IsDeleted != onlyActive && x.Id == id)
                    .Include(x => x.User)
                    .Include(blogPost => blogPost.Reactions)
                    .Include(x => x.Tags)
                    .Include(x => x.CourseTopics)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextCourse =
                await query.Where(x => x.IsDeleted != onlyActive && x.Id > id)
                    .OrderBy(x => x.Id)
                    .Include(x => x.User)
                    .Include(blogPost => blogPost.Reactions)
                    .Include(x => x.Tags)
                    .FirstOrDefaultAsync(),
            PreviousCourse = await query.Where(x => x.IsDeleted != onlyActive && x.Id < id)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync()
        };
    }

    public Task<PagedResultModel<Course>> GetAllPagedCourseItemsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _courses.Where(x => x.IsDeleted != onlyActive)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task MarkAsDeletedAsync(Course? course)
    {
        if (course is null)
        {
            return;
        }

        course.IsDeleted = true;
        await uow.SaveChangesAsync();

        logger.LogWarning(message: "Deleted a Course record with Id={Id} and Title={Text}", course.Id, course.Title);

        fullTextSearchService.DeleteLuceneDocument(course
            .MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)
            .DocumentTypeIdHash);
    }

    public async Task NotifyDeleteChangesAsync(Course? course)
    {
        if (course is null)
        {
            return;
        }

        await emailsService.NewCourseEmailToAdminsAsync(course.Id, new CourseModel
        {
            Description = "حذف شد",
            Title = course.Title,
            HowToPay = "حذف شد",
            Requirements = "حذف شد",
            TopicsList = "حذف شد"
        });
    }

    public async Task UpdateCourseItemAsync(Course? course, CourseModel? writeCourseModel)
    {
        ArgumentNullException.ThrowIfNull(writeCourseModel);

        if (course is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveCourseItemTagsAsync(writeCourseModel.Tags);

        mapper.Map(writeCourseModel, course);
        course.Tags = listOfActualTags;

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(course.MapToWhatsNewItemModel(siteRootUri: "",
            showBriefDescription: false));
    }

    public async Task<Course?> AddCourseItemAsync(CourseModel? writeCourseModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeCourseModel);

        var listOfActualTags = await tagsService.SaveCourseItemTagsAsync(writeCourseModel.Tags);

        var item = mapper.Map<CourseModel, Course>(writeCourseModel);
        item.Tags = listOfActualTags;
        item.UserId = user?.Id;

        var course = AddCourse(item);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(course.MapToWhatsNewItemModel(siteRootUri: "",
            showBriefDescription: false));

        return course;
    }

    public async Task NotifyAddOrUpdateChangesAsync(Course? course, CourseModel? writeCourseModel, User? user)
    {
        if (course is null || writeCourseModel is null || user is null)
        {
            return;
        }

        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(user.Id);
        await statService.RecalculateTagsInUseCountsAsync<CourseTag, Course>();

        await emailsService.NewCourseEmailToAdminsAsync(course.Id, writeCourseModel);
        await emailsService.NewCourseEmailToUserAsync(course.Id, writeCourseModel, course.UserId ?? 0);
    }

    public async Task IndexCoursesAsync()
    {
        var items = await _courses.AsNoTracking()
            .Where(x => !x.IsDeleted && x.IsReadyToPublish)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(blogPost => blogPost.Reactions)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        await fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)));
    }

    private async Task<OperationResult> CheckNumberOfMonthsRequiredAsync(int userId, Course course)
    {
        var numberOfPosts = await GetWriterNumberOfPostsAsync(userId, course);

        if (numberOfPosts >= course.NumberOfPostsRequired)
        {
            return OperationStat.Succeeded;
        }

        return (
            string.Create(CultureInfo.InvariantCulture,
                $"برای دسترسی به این دوره نیاز به ارسال {course.NumberOfPostsRequired} مطلب در طی {course.NumberOfMonthsRequired} ماه قبل است. تعداد مطالب ارسالی شما در طی {course.NumberOfMonthsRequired} ماه قبل: {numberOfPosts}"),
            OperationStat.Failed);
    }

    private Task<int> GetWriterNumberOfPostsAsync(int userId, Course course)
        => usersService.WriterNumberOfPostsAsync(userId, DateTime.UtcNow.AddMonths(-course.NumberOfMonthsRequired),
            DateTime.UtcNow);

    private Task<long?> GetTotalNumberOfRatingsValueAsync(int userId, Course course)
        => userRatingsService.TotalNumberOfRatingsValueAsync(
            DateTime.UtcNow.AddMonths(-course.NumberOfMonthsTotalRatingsRequired), DateTime.UtcNow, userId);
}
