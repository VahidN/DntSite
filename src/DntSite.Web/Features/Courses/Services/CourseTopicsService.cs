using AutoMapper;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.Models;
using DntSite.Web.Features.Courses.ModelsMappings;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Courses.Services;

public class CourseTopicsService(
    IUnitOfWork uow,
    IMapper mapper,
    IStatService statService,
    ICoursesEmailsService emailsService,
    IUserRatingsService userRatingsService,
    ICoursesService coursesService,
    IFullTextSearchService fullTextSearchService,
    IPdfExportService pdfExportService,
    ILogger<CourseTopicsService> logger) : ICourseTopicsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<CourseTopic, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<CourseTopic> _courseTopics = uow.DbSet<CourseTopic>();

    public ValueTask<CourseTopic?> FindCourseTopicAsync(int id) => _courseTopics.FindAsync(id);

    public Task<CourseTopic?> FindCourseTopicAsync(Guid id)
        => _courseTopics.Include(x => x.Course)
            .ThenInclude(x => x.Tags)
            .OrderBy(x => x.DisplayId)
            .FirstOrDefaultAsync(x => x.DisplayId == id);

    public CourseTopic AddCourseTopic(CourseTopic topic) => _courseTopics.Add(topic).Entity;

    public Task<PagedResultModel<CourseTopic>> GetPagedCourseTopicsAsync(int courseId,
        int pageNumber,
        bool isMain,
        int recordsPerPage = 5,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _courseTopics
            .Where(x => x.IsDeleted != onlyActive && x.IsMainTopic == isMain && x.CourseId == courseId)
            .Include(x => x.User)
            .Include(x => x.Course)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<List<CourseTopic>> GetAllCourseTopicsAsync(int courseId, bool onlyActive = true)
        => _courseTopics.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActive && x.CourseId == courseId)
            .Include(x => x.User)
            .Include(x => x.Course)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public Task<List<CourseTopic>> GetPagedAllActiveCoursesTopicsAsync()
        => _courseTopics.AsNoTracking()
            .Where(x => !x.IsDeleted && x.Course.IsReadyToPublish)
            .Include(x => x.User)
            .Include(x => x.Course)
            .ThenInclude(x => x.Tags)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

    public Task<List<CourseTopic>> GetPagedAllActiveCoursesTopicsAsync(int count, bool onlyActive = true)
        => _courseTopics.AsNoTracking()
            .Where(x => x.IsDeleted != onlyActive && x.Course.IsReadyToPublish)
            .Include(x => x.User)
            .Include(x => x.Course)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<PagedResultModel<CourseTopic>> GetPagedAllCoursesTopicsAsync(int pageNumber,
        bool isMain,
        int recordsPerPage = 5,
        bool onlyActive = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _courseTopics
            .Where(x => x.IsDeleted != onlyActive && x.Course.IsReadyToPublish && x.IsMainTopic == isMain)
            .Include(x => x.User)
            .Include(x => x.Course)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<CourseTopic?> FindTopicAsync(Guid topicId, bool onlyActive = true)
        => _courseTopics.Where(x => x.IsDeleted != onlyActive)
            .Include(x => x.User)
            .Include(x => x.Course)
            .ThenInclude(x => x.Tags)
            .Include(x => x.Reactions)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.DisplayId == topicId);

    public async Task UpdateNumberOfViewsAsync(Guid topicId, bool fromFeed, bool onlyActive = true)
    {
        var thisTopic = await FindTopicAsync(topicId, onlyActive);

        if (thisTopic is null)
        {
            return;
        }

        thisTopic.EntityStat.NumberOfViews++;

        if (fromFeed)
        {
            thisTopic.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<CourseTopicReaction, CourseTopic>(fkId, reactionType, fromUserId);

    public Task<List<CourseTopic>> GetAllPublicTopicsOfDateAsync(DateTime date)
        => _courseTopics.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Course)
            .Where(x => !x.IsDeleted && !x.Course.IsDeleted && x.Course.IsReadyToPublish &&
                        x.Audit.CreatedAt.Year == date.Year && x.Audit.CreatedAt.Month == date.Month &&
                        x.Audit.CreatedAt.Day == date.Day)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public Task<int> GetAllCourseTopicsCountAsync(bool onlyActive = true)
        => _courseTopics.AsNoTracking().CountAsync(x => x.IsDeleted != onlyActive && x.Course.IsReadyToPublish);

    public async Task<CourseTopicModel?> GetTopicAsync(Guid topicId, bool onlyActive = true)
    {
        var thisTopic = await FindTopicAsync(topicId, onlyActive);

        if (thisTopic is null)
        {
            return null;
        }

        await UpdateReadingTimeOfOldPostsAsync(thisTopic);

        var id = thisTopic.Id;
        var courseId = thisTopic.CourseId;

        return new CourseTopicModel
        {
            ThisTopic = thisTopic,
            PreviousTopic =
                await _courseTopics.AsNoTracking()
                    .Where(x => x.IsDeleted != onlyActive && x.Id > id && x.CourseId == courseId)
                    .OrderBy(x => x.Id)
                    .Include(x => x.User)
                    .Include(x => x.Course)
                    .ThenInclude(x => x.Tags)
                    .Include(x => x.Reactions)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextTopic = await _courseTopics.AsNoTracking()
                .Where(x => x.IsDeleted != onlyActive && x.Id < id && x.CourseId == courseId)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(x => x.Course)
                .ThenInclude(x => x.Tags)
                .Include(x => x.Reactions)
                .FirstOrDefaultAsync()
        };
    }

    public async Task<bool> CanUserAddCourseTopicAsync(CurrentUserModel? user, int courseId)
    {
        if (user is null)
        {
            return false;
        }

        if (!user.IsAuthenticated)
        {
            return false;
        }

        if (user.IsAdmin)
        {
            return true;
        }

        var author = await coursesService.FindCourseAuthorAsync(courseId);

        return author is not null && author.Id == user.UserId;
    }

    public async Task MarkAsDeletedAsync(CourseTopic? courseTopic)
    {
        if (courseTopic is null)
        {
            return;
        }

        courseTopic.IsDeleted = true;
        await uow.SaveChangesAsync();

        logger.LogWarning(message: "Deleted a CourseTopic record with Id={Id} and Title={Text}", courseTopic.Id,
            courseTopic.Title);

        fullTextSearchService.DeleteLuceneDocument(courseTopic
            .MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)
            .DocumentTypeIdHash);

        await pdfExportService.InvalidateExportedFilesAsync(WhatsNewItemType.AllCoursesTopics, courseTopic.Id);
    }

    public async Task UpdateCourseTopicItemAsync(CourseTopic? courseTopic, CourseTopicItemModel writeCourseItemModel)
    {
        ArgumentNullException.ThrowIfNull(writeCourseItemModel);

        if (courseTopic is null)
        {
            return;
        }

        mapper.Map(writeCourseItemModel, courseTopic);

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(
            courseTopic.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false));

        await pdfExportService.InvalidateExportedFilesAsync(WhatsNewItemType.AllCoursesTopics, courseTopic.Id);
    }

    public async Task<CourseTopic?> AddCourseTopicItemAsync(CourseTopicItemModel writeCourseItemModel,
        User? user,
        int courseId)
    {
        ArgumentNullException.ThrowIfNull(writeCourseItemModel);

        var item = mapper.Map<CourseTopicItemModel, CourseTopic>(writeCourseItemModel);
        item.UserId = user?.Id;
        item.CourseId = courseId;
        var courseTopic = AddCourseTopic(item);
        await uow.SaveChangesAsync();

        await SetParentAsync(courseTopic, courseId);

        fullTextSearchService.AddOrUpdateLuceneDocument(
            courseTopic.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false));

        await pdfExportService.InvalidateExportedFilesAsync(WhatsNewItemType.AllCoursesTopics, courseTopic.Id);

        return courseTopic;
    }

    public async Task NotifyAddOrUpdateChangesAsync(CourseTopic? courseTopic)
    {
        if (courseTopic is null)
        {
            return;
        }

        await emailsService.WriteCourseTopicSendEmailAsync(courseTopic);
        await statService.UpdateNumberOfCourseTopicsStatAsync(courseTopic.CourseId);
    }

    public async Task IndexCourseTopicsAsync()
    {
        var items = await _courseTopics.AsNoTracking()
            .Where(x => !x.IsDeleted && x.Course.IsReadyToPublish)
            .Include(x => x.User)
            .Include(x => x.Course)
            .ThenInclude(x => x.Tags)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        await fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)));
    }

    private async Task SetParentAsync(CourseTopic result, int courseId)
        => result.Course =
            await uow.DbSet<Course>()
                .Include(x => x.Tags)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.Id == courseId) ?? new Course
            {
                Id = courseId,
                Title = "",
                Description = ""
            };

    private Task UpdateReadingTimeOfOldPostsAsync(CourseTopic? currentItem)
    {
        if (currentItem is null || currentItem.ReadingTimeMinutes != 0)
        {
            return Task.CompletedTask;
        }

        currentItem.ReadingTimeMinutes = currentItem.Body.MinReadTime();

        return uow.SaveChangesAsync();
    }
}
