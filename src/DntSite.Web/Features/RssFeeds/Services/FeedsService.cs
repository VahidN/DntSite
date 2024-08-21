using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.RssFeeds.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;
using DntSite.Web.Features.Surveys.RoutingConstants;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.RssFeeds.Services;

public class FeedsService(
    IAppSettingsService appSettingsService,
    IProjectsService projectsService,
    IProjectIssuesService projectIssuesService,
    IProjectReleasesService projectReleases,
    IProjectIssueCommentsService projectIssueCommentsService,
    IProjectFaqsService projectFaqsService,
    IBlogPostsService blogPostsService,
    IBlogCommentsService blogCommentsService,
    IDailyNewsItemsService dailyNewsItemsService,
    IDailyNewsItemCommentsService dailyNewsItemCommentsService,
    IBlogPostDraftsService blogPostDraftsService,
    IVotesService votesService,
    IVoteCommentsService voteCommentsService,
    IAdvertisementsService advertisementsService,
    IAdvertisementCommentsService advertisementCommentsService,
    ICoursesService coursesService,
    ICourseTopicsService courseTopicsService,
    ICourseTopicCommentsService courseTopicCommentsService,
    ILearningPathService learningPathService,
    IBacklogsService backlogsService,
    IQuestionsService stackExchangeService,
    IQuestionsCommentsService questionsCommentsService) : IFeedsService
{
    public async Task<WhatsNewFeedChannel> GetLatestChangesAsync(int take = 30)
    {
        var result = new List<WhatsNewItemModel>();
        result.AddRange((await GetProjectsNewsAsync()).RssItems ?? []);
        result.AddRange((await GetProjectsFilesAsync()).RssItems ?? []);
        result.AddRange((await GetProjectsIssuesAsync()).RssItems ?? []);
        result.AddRange((await GetProjectsIssuesRepliesAsync()).RssItems ?? []);
        result.AddRange((await GetProjectsFaqsAsync()).RssItems ?? []);
        result.AddRange((await GetPostsAsync()).RssItems ?? []);
        result.AddRange((await GetCommentsAsync()).RssItems ?? []);
        result.AddRange((await GetNewsAsync()).RssItems ?? []);
        result.AddRange((await GetNewsCommentsAsync()).RssItems ?? []);
        result.AddRange((await GetAllDraftsAsync()).RssItems ?? []);
        result.AddRange((await GetAllVotesAsync()).RssItems ?? []);
        result.AddRange((await GetVotesRepliesAsync()).RssItems ?? []);
        result.AddRange((await GetAllAdvertisementsAsync()).RssItems ?? []);
        result.AddRange((await GetAdvertisementCommentsAsync()).RssItems ?? []);
        result.AddRange((await GetAllCoursesAsync()).RssItems ?? []);
        result.AddRange((await GetAllCoursesTopicsAsync()).RssItems ?? []);
        result.AddRange((await GetCourseTopicsRepliesAsync()).RssItems ?? []);
        result.AddRange((await GetLearningPathsAsync()).RssItems ?? []);
        result.AddRange((await GetBacklogsAsync()).RssItems ?? []);
        result.AddRange((await GetQuestionsFeedItemsAsync()).RssItems ?? []);
        result.AddRange((await GetQuestionsCommentsFeedItemsAsync()).RssItems ?? []);

        var rssItems = result.OrderByDescending(x => x.PublishDate).Take(take).ToList();
        var appSetting = await GetAppSettingsAsync();

        var title = "فید کلی آخرین نظرات، مطالب، اشتراک‌ها و پروژه‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetQuestionsCommentsFeedItemsAsync(int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await questionsCommentsService.GetLastPagedStackExchangeQuestionCommentsAsNoTrackingAsync(pageNumber,
            recordsPerPage, showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پاسخ به پرسش: {item.Parent.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{QuestionsRoutingConstants.QuestionsDetailsBase}/{item.ParentId}#comment-{item.Id}")),
                Categories = ["پاسخ به پرسش‌ها"]
            })
            .ToList();

        var title = "فید پاسخ‌های پرسش‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetBacklogsAsync(int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false,
        bool isInProgress = false)
    {
        var list = await backlogsService.GetBacklogsAsync(pageNumber, userId, recordsPerPage, showDeletedItems,
            pagerSortBy, isAscending, isNewItems, isDone, isInProgress);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پیشنهاد: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    BacklogsRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["پیشنهادها"]
            })
            .ToList();

        var title = "فید پیشنهادهای";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetQuestionsFeedItemsAsync(int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false)
    {
        var list = await stackExchangeService.GetStackExchangeQuestionsAsync(pageNumber, userId, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending, isNewItems, isDone);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پرسش: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    QuestionsRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["پرسش‌ها"]
            })
            .ToList();

        var title = "فید پرسش‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetLearningPathsAsync(int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showAll = false,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await learningPathService.GetLearningPathsAsync(pageNumber, userId, recordsPerPage, showAll,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"مسیر راه: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    RoadMapsRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["مسیرراه‌ها"]
            })
            .ToList();

        var title = "فید مسیرهای راه";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllCoursesTopicsAsync()
    {
        var list = await courseTopicsService.GetPagedAllActiveCoursesTopicsAsync();

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body.GetBriefDescription(charLength: 200),
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{CoursesRoutingConstants.CoursesTopicBase}/{item.CourseId}/{item.DisplayId:D}")),
                Categories = ["مطالب دوره‌ها"]
            })
            .ToList();

        var title = "فید مطالب دوره‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllCoursesAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await coursesService.GetPagedCourseItemsIncludeUserAndTagsAsync(pageNumber, recordsPerPage,
            onlyActive, showOnlyFinished, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"دوره جدید: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    CoursesRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["دوره‌ها"]
            })
            .ToList();

        var title = "فید دوره‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllVotesAsync(int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await votesService.GetVotesListAsync(pageNumber, recordsPerPage, showDeletedItems, pagerSortBy,
            isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content =
                    item.SurveyItems.Where(x => !x.IsDeleted)
                        .Select(x => x.Title)
                        .Aggregate((s1, s2) => s1 + "<br/>" + s2),
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"نظر سنجی: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    SurveysRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["نظرسنجی‌ها"]
            })
            .ToList();

        var title = "فید نظرسنجی‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllAdvertisementsAsync(int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await advertisementsService.GetAnnouncementsListAsync(pageNumber, recordsPerPage, showDeletedItems,
            pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"آگهی: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    AdvertisementsRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["آگهی‌ها"]
            })
            .ToList();

        var title = "فید آگهی‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllDraftsAsync()
    {
        var list = await blogPostDraftsService.ComingSoonItemsAsync();
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = "به زودی ...",
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"به زودی: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(PostsRoutingConstants.ComingSoon2),
                Categories = ["مقالات آتی"]
            })
            .ToList();

        var title = "فید پیش‌نویس‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsNewsAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await projectsService.GetPagedProjectItemsIncludeUserAndTagsAsync(pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پروژه جدید: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    ProjectsRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["پروژه‌ها"]
            })
            .ToList();

        var title = "فید پروژه‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsFilesAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await projectReleases.GetAllProjectsReleasesIncludeProjectsAsync(pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.FileDescription,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"فایل جدید: {item.FileName}",
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}")),
                Categories = ["فایل‌های پروژه‌ها"]
            })
            .ToList();

        var title = "فید فایل‌های پروژه‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsIssuesAsync(int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await projectIssuesService.GetLastPagedAllProjectsIssuesAsNoTrackingAsync(pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}")),
                Categories = ["بازخوردهای پروژه‌ها"]
            })
            .ToList();

        var title = "فید بازخورد‌های پروژه‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsIssuesRepliesAsync(int count = 15, bool showDeletedItems = false)
    {
        var list = await projectIssueCommentsService.GetLastIssueCommentsIncludeBlogPostAndUserAsync(count,
            showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.Parent.ProjectId}/{item.ParentId}#comment-{item.Id}")),
                Categories = ["پاسخ به بازخورد‌های پروژه‌ها"]
            })
            .ToList();

        var title = "فید پاسخ به بازخورد‌های پروژه‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetVotesRepliesAsync(int count = 15, bool showDeletedItems = false)
    {
        var list = await voteCommentsService.GetLastVoteCommentsIncludeBlogPostAndUserAsync(count, showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{SurveysRoutingConstants.SurveysArchiveDetailsBase}/{item.ParentId}#comment-{item.Id}")),
                Categories = ["نظرات نظرسنجی‌ها"]
            })
            .ToList();

        var title = "فید نظرات نظرسنجی‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAdvertisementCommentsAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false)
    {
        var list = await advertisementCommentsService.GetLastPagedAdvertisementCommentsAsNoTrackingAsync(pageNumber,
            recordsPerPage, showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{AdvertisementsRoutingConstants.AdvertisementsDetailsBase}/{item.ParentId}#comment-{item.Id}")),
                Categories = ["نظرات آگهی‌ها"]
            })
            .ToList();

        var title = "فید نظرات آگهی‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsFaqsAsync(int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false)
    {
        var list = await projectFaqsService.GetAllLastPagedProjectFaqsAsync(pageNumber, recordsPerPage,
            showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}")),
                Categories = ["راهنماهای پروژه‌ها"]
            })
            .ToList();

        var title = "فید راهنماهای پروژه‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFaqsAsync(int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        if (projectId is null)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(projectId.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list = await projectFaqsService.GetLastPagedProjectFaqsAsync(projectId.Value, pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}")),
                Categories = ["راهنماهای پروژه‌ها"]
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید راهنمای پروژه {0}", project.Title);

        return (new WhatsNewFeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFilesAsync(int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        if (projectId is null)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(projectId.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list = await projectReleases.GetAllProjectReleasesAsync(projectId.Value, pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.FileDescription,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.FileName,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}")),
                Categories = ["فایل‌های پروژه‌ها"]
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید فایل‌های پروژه‌ {0}", project.Title);

        return (new WhatsNewFeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesAsync(int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        if (projectId is null)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(projectId.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list = await projectIssuesService.GetLastPagedProjectIssuesAsNoTrackingAsync(projectId.Value, pageNumber,
            recordsPerPage, showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}")),
                Categories = ["بازخورد‌های پروژه‌ها"]
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید بازخورد‌های پروژه {0}", project.Title);

        return (new WhatsNewFeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesRepliesAsync(int? projectId,
        int count = 15,
        bool showDeletedItems = false)
    {
        if (!projectId.HasValue)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(projectId.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list =
            await projectIssueCommentsService.GetLastProjectIssueCommentsIncludeBlogPostAndUserAsync(projectId.Value,
                count, showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{projectId.Value}/{item.ParentId}#comment-{item.Id}")),
                Categories = ["پاسخ ‌به بازخورد‌های پروژه‌ها"]
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید پاسخ ‌به بازخورد‌های پروژه‌ {0}",
            project.Title);

        return (new WhatsNewFeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<WhatsNewFeedChannel> GetPostsAsync(int count = 15, bool showDeletedItems = false)
    {
        var items = await blogPostsService.GetLastBlogPostsIncludeAuthorTagsAsync(count, showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Where(item => !IsPrivate(item))
            .Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    PostsRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["مطالب"]
            })
            .ToList();

        var title = "فید مطالب";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetCommentsAsync(int count = 15, bool showDeletedItems = false)
    {
        var items = await blogCommentsService.GetLastBlogCommentsIncludeBlogPostAndUserAsync(count, showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Where(item => !IsPrivateComment(item))
            .Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{PostsRoutingConstants.PostBase}/{item.ParentId}#comment-{item.Id}")),
                Categories = ["نظرات مطالب"]
            })
            .ToList();

        var title = "فید نظرات مطالب";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetNewsAsync(int count = 15, bool showDeletedItems = false)
    {
        var list = await dailyNewsItemsService.GetLastDailyNewsItemsIncludeUserAsync(count, showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.BriefDescription ?? "",
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    NewsRoutingConstants.PostUrlTemplate, item.Id)),
                Categories = ["اشتراک‌ها"]
            })
            .ToList();

        var title = "فید خلاصه اشتراک‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetTagAsync(string tag,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var items = await blogPostsService.GetLastBlogPostsByTagIncludeAuthorAsync(tag, pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Data.Where(item => !IsPrivate(item))
            .Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(Invariant($"{PostsRoutingConstants.PostBase}/{item.Id}")),
                Categories = ["گروه‌ها"]
            })
            .ToList();

        var title = string.Format(CultureInfo.InvariantCulture, format: "فید گروه {0}", tag);

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAuthorAsync(string authorName,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var items = await blogPostsService.GetLastBlogPostsByAuthorIncludeAuthorTagsAsync(authorName, pageNumber,
            recordsPerPage, showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Data.Where(item => !IsPrivate(item))
            .Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(Invariant($"{PostsRoutingConstants.PostBase}/{item.Id}")),
                Categories = ["نویسنده‌ها"]
            })
            .ToList();

        var title = string.Format(CultureInfo.InvariantCulture, format: "فید مطالب {0}", authorName);

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetNewsCommentsAsync(int count = 15, bool showDeletedItems = false)
    {
        var items =
            await dailyNewsItemCommentsService.GetLastBlogNewsCommentsIncludeBlogPostAndUserAsync(count,
                showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{NewsRoutingConstants.NewsDetailsBase}/{item.ParentId}#comment-{item.Id}")),
                Categories = ["نظرات اشتراک‌ها"]
            })
            .ToList();

        var title = "فید نظرات اشتراک‌ها";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetNewsAuthorAsync(string name,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var items = await dailyNewsItemsService.GetLastPagedDailyNewsItemsIncludeUserAndTagAsync(name, pageNumber,
            recordsPerPage, showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Data.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.BriefDescription ?? "",
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(Invariant($"{NewsRoutingConstants.NewsDetailsBase}/{item.Id}")),
                Categories = ["اشتراک‌های اشخاص"]
            })
            .ToList();

        var title = string.Format(CultureInfo.InvariantCulture, format: "فید اشتراک‌های {0}", name);

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetCourseTopicsRepliesAsync(int count = 15, bool onlyActives = true)
    {
        var list = await courseTopicCommentsService.GetLastTopicCommentsIncludePostAndUserAsync(count, onlyActives);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new WhatsNewItemModel
            {
                User = item.User,
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body.GetBriefDescription(charLength: 200),
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پاسخ به: {item.Parent.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{CoursesRoutingConstants.CoursesTopicBase}/{item.Parent.CourseId}/{item.Parent.DisplayId:D}#comment-{item.Id}")),
                Categories = ["بازخوردهای دوره‌ها"]
            })
            .ToList();

        var title = "فید بازخوردهای دوره‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    private static WhatsNewFeedChannel GetFeedChannel(string title,
        AppSetting appSetting,
        List<WhatsNewItemModel> rssItems)
        => new()
        {
            FeedTitle = $"{title} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR",
            FeedCopyright = $"© {appSetting.BlogName}"
        };

    private async Task<AppSetting> GetAppSettingsAsync()
        => await appSettingsService.GetAppSettingsAsync() ?? new AppSetting
        {
            BlogName = "DNT",
            SiteRootUri = "/"
        };

    private static bool IsPrivate(BlogPost item) => item.NumberOfRequiredPoints is > 0;

    private static bool IsPrivateComment(BlogPostComment item) => item.Parent.NumberOfRequiredPoints is > 0;
}
