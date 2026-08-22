using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Bookmarks.Models;
using DntSite.Web.Features.Bookmarks.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Entities;
using Gridify;

namespace DntSite.Web.Features.Bookmarks.Services;

public class BookmarksService(IUnitOfWork uow) : IBookmarksService
{
    private readonly DbSet<ParentBookmarkEntity> _bookmarks = uow.DbSet<ParentBookmarkEntity>();

    public Task<List<TBookmarkEntity>> GetPostBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId,
        int count = 100)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
        => _bookmarks.AsNoTracking()
            .OfType<TBookmarkEntity>()
            .Include(x => x.User)
            .Where(x => x.ParentId == fkId)
            .Take(count)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public Task<List<User?>> GetPostBookmarksUsersListAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
        => _bookmarks.AsNoTracking()
            .OfType<TBookmarkEntity>()
            .Include(x => x.User)
            .Where(x => x.ParentId == fkId)
            .Select(x => x.User)
            .ToListAsync();

    public async Task<PagedResultModel<TBookmarkEntity>> GetUserBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(
        int? userId,
        int pageNumber,
        int recordsPerPage = 8,
        bool isAscending = false)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        var query = GetUserBookmarksQuery<TBookmarkEntity, TForeignKeyEntity>(userId, isAscending);

        return new PagedResultModel<TBookmarkEntity>
        {
            TotalItems = await query.CountAsync(),
            Data = await query.ApplyPaging(pageNumber, recordsPerPage).ToListAsync()
        };
    }

    public async Task<bool> SavePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId,
        BookmarkActionType actionType,
        int? fromUserId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>, new()
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        if (fromUserId is null)
        {
            return false;
        }

        var parentEntity = await uow.DbSet<TForeignKeyEntity>().FindAsync(fkId);

        if (parentEntity is null)
        {
            return false;
        }

        var entityBookmarksQuery = _bookmarks.OfType<TBookmarkEntity>();

        var userBookmark = await entityBookmarksQuery.OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.ParentId == fkId && x.UserId == fromUserId);

        switch (actionType)
        {
            case BookmarkActionType.Add:
                if (userBookmark is null)
                {
                    _bookmarks.Add(new TBookmarkEntity
                    {
                        UserId = fromUserId.Value,
                        ParentId = fkId
                    });
                }
                else
                {
                    userBookmark.IsDeleted = false;
                    _bookmarks.Update(userBookmark);
                }

                break;
            case BookmarkActionType.Cancel:
                if (userBookmark is not null)
                {
                    _bookmarks.Remove(userBookmark);
                }

                break;
        }

        await uow.SaveChangesAsync();

        parentEntity.EntityStat.NumberOfBookmarks =
            await entityBookmarksQuery.AsNoTracking().CountAsync(x => x.ParentId == fkId);

        await uow.SaveChangesAsync();

        return true;
    }

#pragma warning disable CA1305,CA1863,MA0076
    public async Task<PagedResultModel<BookmarkDto>> GetAllUserBookmarksAsync(int? userId,
        int pageNumber,
        int recordsPerPage = 20,
        bool isAscending = false)
    {
        var allQueries = GetUserBookmarksQuery<AdvertisementBookmark, Advertisement>(userId)
            .Select(b => new BookmarkDto
            {
                BookmarkId = b.Id,
                Title = b.Parent.Title,
                Type = WhatsNewItemType.AllAdvertisementsName,
                Url = AdvertisementsRoutingConstants.AdvertisementsDetailsBase + "/" + b.Parent.Id
            })
            .Concat(GetUserBookmarksQuery<BacklogBookmark, Backlog>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.BacklogsName,
                    Url = BacklogsRoutingConstants.BacklogsDetailsBase + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<BlogPostBookmark, BlogPost>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.PostsName,
                    Url = PostsRoutingConstants.PostBase + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<CourseBookmark, Course>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.AllCoursesName,
                    Url = CoursesRoutingConstants.CoursesDetailsBase + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<CourseTopicBookmark, CourseTopic>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.AllCoursesTopicsName,
                    Url = CoursesRoutingConstants.CoursesTopicBase + "/" + b.Parent.CourseId + "/" + b.Parent.DisplayId
                }))
            .Concat(GetUserBookmarksQuery<LearningPathBookmark, LearningPath>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.LearningPathsName,
                    Url = RoadMapsRoutingConstants.LearningPathsDetailsBase + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<DailyNewsItemBookmark, DailyNewsItem>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.NewsName,
                    Url = NewsRoutingConstants.NewsDetailsBase + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<ProjectFaqBookmark, ProjectFaq>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.ProjectFaqsName,
                    Url = ProjectsRoutingConstants.ProjectFaqsBase + "/" + b.Parent.Project.Id + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<ProjectIssueBookmark, ProjectIssue>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.ProjectIssuesName,
                    Url = ProjectsRoutingConstants.ProjectFeedbacksBase + "/" + b.Parent.ProjectId + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<ProjectReleaseBookmark, ProjectRelease>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.FileName,
                    Type = WhatsNewItemType.ProjectFilesName,
                    Url = ProjectsRoutingConstants.ProjectReleasesBase + "/" + b.Parent.ProjectId + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<ProjectBookmark, Project>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.ProjectsNewsName,
                    Url = ProjectsRoutingConstants.ProjectsDetailsBase + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<StackExchangeQuestionBookmark, StackExchangeQuestion>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.QuestionsName,
                    Url = QuestionsRoutingConstants.QuestionsDetailsBase + "/" + b.Parent.Id
                }))
            .Concat(GetUserBookmarksQuery<SurveyBookmark, Survey>(userId)
                .Select(b => new BookmarkDto
                {
                    BookmarkId = b.Id,
                    Title = b.Parent.Title,
                    Type = WhatsNewItemType.AllVotesName,
                    Url = SurveysRoutingConstants.SurveysArchiveDetailsBase + "/" + b.Parent.Id
                }));

        allQueries = !isAscending
            ? allQueries.OrderByDescending(b => b.BookmarkId)
            : allQueries.OrderBy(b => b.BookmarkId);

        return new PagedResultModel<BookmarkDto>
        {
            TotalItems = await allQueries.CountAsync(),
            Data = await allQueries.ApplyPaging(pageNumber, recordsPerPage).ToListAsync()
        };
    }
#pragma warning restore CA1305,CA1863,MA0076

    private IQueryable<TBookmarkEntity> GetUserBookmarksQuery<TBookmarkEntity, TForeignKeyEntity>(int? userId,
        bool? isAscending = null)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        var query = _bookmarks.OfType<TBookmarkEntity>()
            .Include(bookmarkEntity => bookmarkEntity.Parent)
            .ThenInclude(keyEntity => keyEntity.User)
            .Include(bookmarkEntity => bookmarkEntity.User)
            .Where(bookmarkEntity => bookmarkEntity.UserId == userId && !bookmarkEntity.Parent.IsDeleted &&
                                     !bookmarkEntity.IsDeleted)
            .AsNoTracking();

        if (isAscending.HasValue)
        {
            query = !isAscending.Value
                ? query.OrderByDescending(bookmarkEntity => bookmarkEntity.Id)
                : query.OrderBy(bookmarkEntity => bookmarkEntity.Id);
        }

        return query;
    }
}
