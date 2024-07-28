using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;
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
    public async Task<FeedChannel> GetLatestChangesAsync(int take = 30)
    {
        var result = new List<FeedItem>();
        result.AddRange((await GetProjectsNewsAsync()).RssItems);
        result.AddRange((await GetProjectsFilesAsync()).RssItems);
        result.AddRange((await GetProjectsIssuesAsync()).RssItems);
        result.AddRange((await GetProjectsIssuesRepliesAsync()).RssItems);
        result.AddRange((await GetProjectsFaqsAsync()).RssItems);
        result.AddRange((await GetPostsAsync()).RssItems);
        result.AddRange((await GetCommentsAsync()).RssItems);
        result.AddRange((await GetNewsAsync()).RssItems);
        result.AddRange((await GetNewsCommentsAsync()).RssItems);
        result.AddRange((await GetAllDraftsAsync()).RssItems);
        result.AddRange((await GetAllVotesAsync()).RssItems);
        result.AddRange((await GetVotesRepliesAsync()).RssItems);
        result.AddRange((await GetAllAdvertisementsAsync()).RssItems);
        result.AddRange((await GetAdvertisementCommentsAsync()).RssItems);
        result.AddRange((await GetAllCoursesAsync()).RssItems);
        result.AddRange((await GetAllCoursesTopicsAsync()).RssItems);
        result.AddRange((await GetCourseTopicsRepliesAsync()).RssItems);
        result.AddRange((await GetLearningPathsAsync()).RssItems);
        result.AddRange((await GetBacklogsAsync()).RssItems);
        result.AddRange((await GetQuestionsFeedItemsAsync()).RssItems);
        result.AddRange((await GetQuestionsCommentsFeedItemsAsync()).RssItems);

        var rssItems = result.OrderByDescending(x => x.PublishDate).Take(take).ToList();
        var appSetting = await GetAppSettingsAsync();

        return new FeedChannel
        {
            FeedTitle = $"فید کلی آخرین نظرات، مطالب، اشتراک‌ها و پروژه‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetQuestionsCommentsFeedItemsAsync()
    {
        var list =
            await questionsCommentsService.GetLastPagedStackExchangeQuestionCommentsAsNoTrackingAsync(pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پاسخ به پرسش: {item.Parent.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{QuestionsRoutingConstants.QuestionsDetailsBase}/{item.ParentId}#comment-{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید پاسخ‌های پرسش‌های  {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetBacklogsAsync()
    {
        var list = await backlogsService.GetBacklogsAsync(pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پیشنهاد: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    BacklogsRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید پیشنهادهای {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetQuestionsFeedItemsAsync()
    {
        var list = await stackExchangeService.GetStackExchangeQuestionsAsync(pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پرسش: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    QuestionsRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید پرسش‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetLearningPathsAsync()
    {
        var list = await learningPathService.GetLearningPathsAsync(pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"مسیر راه: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    RoadMapsRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید مسیرهای راه {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetAllCoursesTopicsAsync()
    {
        var list = await courseTopicsService.GetPagedAllActiveCoursesTopicsAsync();

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body.GetBriefDescription(charLength: 200),
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{CoursesRoutingConstants.CoursesTopicBase}/{item.CourseId}/{item.DisplayId:D}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید مطالب دوره‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetAllCoursesAsync()
    {
        var list = await coursesService.GetPagedCourseItemsIncludeUserAndTagsAsync(pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"دوره جدید: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    CoursesRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید دوره‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetAllVotesAsync()
    {
        var list = await votesService.GetVotesListAsync(pageNumber: 0, recordsPerPage: 20);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
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
                    SurveysRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید نظرسنجی‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetAllAdvertisementsAsync()
    {
        var list = await advertisementsService.GetAnnouncementsListAsync(pageNumber: 0, recordsPerPage: 20);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"آگهی: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    AdvertisementsRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید آگهی‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetAllDraftsAsync()
    {
        var list = await blogPostDraftsService.ComingSoonItemsAsync();
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = "به زودی ...",
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"به زودی: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(PostsRoutingConstants.ComingSoon2)
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید پیش‌نویس‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetProjectsNewsAsync()
    {
        var list = await projectsService.GetPagedProjectItemsIncludeUserAndTagsAsync(pageNumber: 0);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پروژه جدید: {item.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    ProjectsRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید پروژه‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetProjectsFilesAsync()
    {
        var list = await projectReleases.GetAllProjectsReleasesIncludeProjectsAsync(pageNumber: 0);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.FileDescription,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"فایل جدید: {item.FileName}",
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید فایل‌های پروژه‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetProjectsIssuesAsync()
    {
        var list = await projectIssuesService.GetLastPagedAllProjectsIssuesAsNoTrackingAsync(pageNumber: 0);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید بازخورد‌های پروژه‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetProjectsIssuesRepliesAsync()
    {
        var list = await projectIssueCommentsService.GetLastIssueCommentsIncludeBlogPostAndUserAsync(count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.Parent.ProjectId}/{item.ParentId}#comment-{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید پاسخ به بازخورد‌های پروژه‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetVotesRepliesAsync()
    {
        var list = await voteCommentsService.GetLastVoteCommentsIncludeBlogPostAndUserAsync(count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{SurveysRoutingConstants.SurveysArchiveDetailsBase}/{item.ParentId}#comment-{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید نظرات نظرسنجی‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetAdvertisementCommentsAsync()
    {
        var list = await advertisementCommentsService.GetLastPagedAdvertisementCommentsAsNoTrackingAsync(pageNumber: 0,
            recordsPerPage: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{AdvertisementsRoutingConstants.AdvertisementsDetailsBase}/{item.ParentId}#comment-{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید نظرات آگهی‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetProjectsFaqsAsync()
    {
        var list = await projectFaqsService.GetAllLastPagedProjectFaqsAsync();

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید راهنماهای پروژه‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<(FeedChannel? Items, Project? Project)> GetProjectFaqsAsync(int? id)
    {
        if (id is null)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(id.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list = await projectFaqsService.GetLastPagedProjectFaqsAsync(id.Value);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{item.Project.Id}/{item.Id}"))
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید راهنمای پروژه {0}", project.Title);

        return (new FeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(FeedChannel? Items, Project? Project)> GetProjectFilesAsync(int? id)
    {
        if (id is null)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(id.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list = await projectReleases.GetAllProjectReleasesAsync(id.Value, pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.FileDescription,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.FileName,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{item.ProjectId}/{item.Id}"))
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید فایل‌های پروژه‌ {0}", project.Title);

        return (new FeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(FeedChannel? Items, Project? Project)> GetProjectIssuesAsync(int? id)
    {
        if (id is null)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(id.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list = await projectIssuesService.GetLastPagedProjectIssuesAsNoTrackingAsync(id.Value, pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Description,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{item.ProjectId}/{item.Id}"))
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید بازخورد‌های پروژه {0}", project.Title);

        return (new FeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(FeedChannel? Items, Project? Project)> GetProjectIssuesRepliesAsync(int? id)
    {
        if (id is null)
        {
            return (null, null);
        }

        var project = await projectsService.FindProjectAsync(id.Value);

        if (project is null)
        {
            return (null, null);
        }

        var list = await projectIssueCommentsService.GetLastProjectIssueCommentsIncludeBlogPostAndUserAsync(id.Value,
            count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{id.Value}/{item.ParentId}#comment-{item.Id}"))
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید پاسخ ‌به بازخورد‌های پروژه‌ {0}",
            project.Title);

        return (new FeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<FeedChannel> GetPostsAsync()
    {
        var items = await blogPostsService.GetLastBlogPostsIncludeAuthorTagsAsync(count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Where(item => !IsPrivate(item))
            .Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    PostsRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید مطالب {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetCommentsAsync()
    {
        var items = await blogCommentsService.GetLastBlogCommentsIncludeBlogPostAndUserAsync(count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Where(item => !IsPrivateComment(item))
            .Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{PostsRoutingConstants.PostBase}/{item.ParentId}#comment-{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید نظرات مطالب {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetNewsAsync()
    {
        var list = await dailyNewsItemsService.GetLastDailyNewsItemsIncludeUserAsync(count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.BriefDescription ?? "",
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture,
                    NewsRoutingConstants.PostUrlTemplate, item.Id))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید خلاصه اشتراک‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetTagAsync(string id)
    {
        var items = await blogPostsService.GetLastBlogPostsByTagIncludeAuthorAsync(id, pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Data.Where(item => !IsPrivate(item))
            .Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(Invariant($"{PostsRoutingConstants.PostBase}/{item.Id}"))
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید گروه {0}", id);

        return new FeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetAuthorAsync(string id)
    {
        var items = await blogPostsService.GetLastBlogPostsByAuthorIncludeAuthorTagsAsync(id, pageNumber: 0);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Data.Where(item => !IsPrivate(item))
            .Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(Invariant($"{PostsRoutingConstants.PostBase}/{item.Id}"))
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید مطالب {0}", id);

        return new FeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetNewsCommentsAsync()
    {
        var items = await dailyNewsItemCommentsService.GetLastBlogNewsCommentsIncludeBlogPostAndUserAsync(count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body,
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = string.Format(CultureInfo.InvariantCulture, format: "پاسخ به: {0}", item.Parent.Title),
                Url = appSetting.SiteRootUri.CombineUrl(
                    Invariant($"{NewsRoutingConstants.NewsDetailsBase}/{item.ParentId}#comment-{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید نظرات اشتراک‌ها {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetNewsAuthorAsync(string id)
    {
        var items = await dailyNewsItemsService.GetLastPagedDailyNewsItemsIncludeUserAndTagAsync(id, pageNumber: 0,
            recordsPerPage: 20);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Data.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.BriefDescription ?? "",
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = item.Title,
                Url = appSetting.SiteRootUri.CombineUrl(Invariant($"{NewsRoutingConstants.NewsDetailsBase}/{item.Id}"))
            })
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید اشتراک‌های {0}", id);

        return new FeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    public async Task<FeedChannel> GetCourseTopicsRepliesAsync()
    {
        var list = await courseTopicCommentsService.GetLastTopicCommentsIncludePostAndUserAsync(count: 15);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => new FeedItem
            {
                AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
                Content = item.Body.GetBriefDescription(charLength: 200),
                PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
                LastUpdatedTime =
                    new DateTimeOffset(item.AuditActions.Count > 0
                        ? item.AuditActions[^1].CreatedAt
                        : item.Audit.CreatedAt),
                Title = $"پاسخ به: {item.Parent.Title}",
                Url = appSetting.SiteRootUri.CombineUrl(Invariant(
                    $"{CoursesRoutingConstants.CoursesTopicBase}/{item.Parent.CourseId}/{item.Parent.DisplayId:D}#comment-{item.Id}"))
            })
            .ToList();

        return new FeedChannel
        {
            FeedTitle = $"فید بازخوردهای دوره‌های {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        };
    }

    private async Task<AppSetting> GetAppSettingsAsync()
        => await appSettingsService.GetAppSettingsAsync() ?? new AppSetting
        {
            BlogName = "DNT",
            SiteRootUri = "/"
        };

    private static bool IsPrivate(BlogPost item) => item.NumberOfRequiredPoints is > 0;

    private static bool IsPrivateComment(BlogPostComment item) => item.Parent.NumberOfRequiredPoints is > 0;
}
