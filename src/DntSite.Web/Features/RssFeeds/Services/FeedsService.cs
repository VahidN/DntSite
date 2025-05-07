using DntSite.Web.Features.Advertisements.ModelsMappings;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Backlogs.ModelsMappings;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.ModelsMappings;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.News.ModelsMappings;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.ModelsMappings;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.ModelsMappings;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.RoadMaps.ModelsMappings;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.RssFeeds.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;
using DntSite.Web.Features.Surveys.ModelsMappings;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.RssFeeds.Services;

public class FeedsService(
    ICachedAppSettingsProvider appSettingsService,
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
    IQuestionsCommentsService questionsCommentsService,
    IDailyNewsScreenshotsService dailyNewsScreenshotsService) : IFeedsService
{
    public async Task<WhatsNewFeedChannel> GetLatestChangesAsync(bool showBriefDescription, int take = 30)
    {
        var result = new List<WhatsNewItemModel>();
        result.AddRange((await GetProjectsNewsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetProjectsFilesAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetProjectsIssuesAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetProjectsIssuesRepliesAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetProjectsFaqsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetPostsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetCommentsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetNewsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetNewsCommentsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetAllDraftsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetAllVotesAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetVotesRepliesAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetAllAdvertisementsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetAdvertisementCommentsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetAllCoursesAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetAllCoursesTopicsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetCourseTopicsRepliesAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetLearningPathsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetBacklogsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetQuestionsFeedItemsAsync(showBriefDescription)).RssItems ?? []);
        result.AddRange((await GetQuestionsCommentsFeedItemsAsync(showBriefDescription)).RssItems ?? []);

        var rssItems = result.OrderByDescending(x => x.PublishDate).Take(take).ToList();
        var appSetting = await GetAppSettingsAsync();

        var title = "فید کلی آخرین نظرات، مطالب، اشتراک‌ها و پروژه‌های";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetQuestionsCommentsFeedItemsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await questionsCommentsService.GetLastPagedStackExchangeQuestionCommentsAsNoTrackingAsync(pageNumber,
            recordsPerPage, showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.QuestionsComments.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetBacklogsAsync(bool showBriefDescription,
        int pageNumber = 0,
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

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.Backlogs.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetQuestionsFeedItemsAsync(bool showBriefDescription,
        int pageNumber = 0,
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

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.Questions.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetLearningPathsAsync(bool showBriefDescription,
        int pageNumber = 0,
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

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.LearningPaths.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllCoursesTopicsAsync(bool showBriefDescription)
    {
        var list = await courseTopicsService.GetPagedAllActiveCoursesTopicsAsync();
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.AllCoursesTopics.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllCoursesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await coursesService.GetPagedCourseItemsIncludeUserAndTagsAsync(pageNumber, recordsPerPage,
            onlyActive, showOnlyFinished, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.AllCourses.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllVotesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await votesService.GetVotesListAsync(pageNumber, recordsPerPage, showDeletedItems, pagerSortBy,
            isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.AllVotes.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllAdvertisementsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await advertisementsService.GetAnnouncementsListAsync(pageNumber, recordsPerPage, showDeletedItems,
            pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.AllAdvertisements.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAllDraftsAsync(bool showBriefDescription)
    {
        var list = await blogPostDraftsService.ComingSoonItemsAsync();
        var appSetting = await GetAppSettingsAsync();
        var rssItems = list.Select(item => item.MapToPostWhatsNewItemModel(appSetting.SiteRootUri)).ToList();
        var title = $"فید {WhatsNewItemType.AllDrafts.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsNewsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await projectsService.GetPagedProjectItemsIncludeUserAndTagsAsync(pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.ProjectsNews.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsFilesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await projectReleases.GetAllProjectsReleasesIncludeProjectsAsync(pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item
                => item.MapToProjectsReleasesWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.ProjectsFiles.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsIssuesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var list = await projectIssuesService.GetLastPagedAllProjectsIssuesAsNoTrackingAsync(pageNumber, recordsPerPage,
            showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Data.Select(item
                => item.MapToProjectsIssuesWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.ProjectsIssues.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsIssuesRepliesAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false)
    {
        var list = await projectIssueCommentsService.GetLastIssueCommentsIncludeBlogPostAndUserAsync(count,
            showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item
                => item.MapToProjectsIssuesWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.ProjectsIssuesReplies.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetVotesRepliesAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false)
    {
        var list = await voteCommentsService.GetLastVoteCommentsIncludeBlogPostAndUserAsync(count, showDeletedItems);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.VotesReplies.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAdvertisementCommentsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false)
    {
        var list = await advertisementCommentsService.GetLastPagedAdvertisementCommentsAsNoTrackingAsync(pageNumber,
            recordsPerPage, showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.AdvertisementComments.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetProjectsFaqsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false)
    {
        var list = await projectFaqsService.GetAllLastPagedProjectFaqsAsync(pageNumber, recordsPerPage,
            showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item
                => item.MapToProjectsFaqsWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.ProjectsFaqs.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFaqsAsync(bool showBriefDescription,
        int? projectId,
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

        var rssItems = list.Data
            .Select(item => item.MapToProjectFaqWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید راهنمای پروژه {0}", project.Title);

        return (new WhatsNewFeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFilesAsync(bool showBriefDescription,
        int? projectId,
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

        var rssItems = list.Data.Select(item
                => item.MapToProjectReleaseWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید فایل‌های پروژه‌ {0}", project.Title);

        return (new WhatsNewFeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesAsync(bool showBriefDescription,
        int? projectId,
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

        var rssItems = list.Data.Select(item
                => item.MapToProjectIssueWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var feedTitle = string.Format(CultureInfo.InvariantCulture, format: "فید بازخورد‌های پروژه {0}", project.Title);

        return (new WhatsNewFeedChannel
        {
            FeedTitle = $"{feedTitle} {appSetting.BlogName}",
            RssItems = rssItems,
            CultureName = "fa-IR"
        }, project);
    }

    public async Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesRepliesAsync(
        bool showBriefDescription,
        int? projectId,
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

        var rssItems = list.Select(item
                => item.MapToProjectIssuesWhatsNewItemModel(appSetting.SiteRootUri, projectId.Value,
                    showBriefDescription))
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

    public async Task<WhatsNewFeedChannel> GetPostsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false)
    {
        var items = await blogPostsService.GetLastBlogPostsIncludeAuthorTagsAsync(count, showDeletedItems);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Where(item => !IsPrivate(item))
            .Select(item => item.MapToPostWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.Posts.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetCommentsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false)
    {
        var items = await blogCommentsService.GetLastBlogCommentsIncludeBlogPostAndUserAsync(count, showDeletedItems);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Where(item => !IsPrivateComment(item))
            .Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.Comments.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetNewsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false)
    {
        var list = await dailyNewsItemsService.GetLastDailyNewsItemsIncludeUserAsync(count, showDeletedItems);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => item.MapToNewsWhatsNewItemModel(showBriefDescription, appSetting.SiteRootUri,
                dailyNewsScreenshotsService.GetNewsThumbImage(item, appSetting.SiteRootUri)))
            .ToList();

        var title = $"فید {WhatsNewItemType.News.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetTagAsync(bool showBriefDescription,
        string tag,
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
            .Select(item => item.MapToTagWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = string.Format(CultureInfo.InvariantCulture, format: "فید گروه {0}", tag);

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetAuthorAsync(bool showBriefDescription,
        string authorName,
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
            .Select(item => item.MapToAuthorWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = string.Format(CultureInfo.InvariantCulture, format: "فید مطالب {0}", authorName);

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetNewsCommentsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false)
    {
        var items =
            await dailyNewsItemCommentsService.GetLastBlogNewsCommentsIncludeBlogPostAndUserAsync(count,
                showDeletedItems);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.NewsComments.Value}";

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetNewsAuthorAsync(bool showBriefDescription,
        string name,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var items = await dailyNewsItemsService.GetLastPagedDailyNewsItemsIncludeUserAndTagAsync(name, pageNumber,
            recordsPerPage, showDeletedItems, pagerSortBy, isAscending);

        var appSetting = await GetAppSettingsAsync();

        var rssItems = items.Data.Select(item => item.MapToAuthorWhatsNewItemModel(showBriefDescription,
                appSetting.SiteRootUri, dailyNewsScreenshotsService.GetNewsThumbImage(item, appSetting.SiteRootUri)))
            .ToList();

        var title = string.Format(CultureInfo.InvariantCulture, format: "فید اشتراک‌های {0}", name);

        return GetFeedChannel(title, appSetting, rssItems);
    }

    public async Task<WhatsNewFeedChannel> GetCourseTopicsRepliesAsync(bool showBriefDescription,
        int count = 15,
        bool onlyActives = true)
    {
        var list = await courseTopicCommentsService.GetLastTopicCommentsIncludePostAndUserAsync(count, onlyActives);
        var appSetting = await GetAppSettingsAsync();

        var rssItems = list.Select(item => item.MapToWhatsNewItemModel(appSetting.SiteRootUri, showBriefDescription))
            .ToList();

        var title = $"فید {WhatsNewItemType.CourseTopicsReplies.Value}";

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
