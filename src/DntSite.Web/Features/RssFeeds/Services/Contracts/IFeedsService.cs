using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.RssFeeds.Services.Contracts;

public interface IFeedsService : IScopedService
{
    public Task<WhatsNewFeedChannel> GetLatestChangesAsync(bool showBriefDescription, int take = 30);

    public Task<WhatsNewFeedChannel> GetBacklogsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false,
        bool isInProgress = false);

    public Task<WhatsNewFeedChannel> GetQuestionsFeedItemsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false);

    public Task<WhatsNewFeedChannel> GetQuestionsCommentsFeedItemsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetLearningPathsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showAll = false,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetAllCoursesTopicsAsync(bool showBriefDescription);

    public Task<WhatsNewFeedChannel> GetAllCoursesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetAllVotesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetAllAdvertisementsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetAllDraftsAsync(bool showBriefDescription);

    public Task<WhatsNewFeedChannel> GetProjectsNewsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetProjectsFilesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetProjectsIssuesAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetProjectsIssuesRepliesAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetVotesRepliesAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetAdvertisementCommentsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetProjectsFaqsAsync(bool showBriefDescription,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false);

    public Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFaqsAsync(bool showBriefDescription,
        int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFilesAsync(bool showBriefDescription,
        int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesAsync(bool showBriefDescription,
        int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesRepliesAsync(bool showBriefDescription,
        int? projectId,
        int count = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetPostsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetCommentsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetNewsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetTagAsync(bool showBriefDescription,
        string tag,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetAuthorAsync(bool showBriefDescription,
        string authorName,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetNewsCommentsAsync(bool showBriefDescription,
        int count = 15,
        bool showDeletedItems = false);

    public Task<WhatsNewFeedChannel> GetNewsAuthorAsync(bool showBriefDescription,
        string name,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<WhatsNewFeedChannel> GetCourseTopicsRepliesAsync(bool showBriefDescription,
        int count = 15,
        bool onlyActives = true);
}
