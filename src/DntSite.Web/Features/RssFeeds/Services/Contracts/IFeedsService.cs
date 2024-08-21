using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.RssFeeds.Services.Contracts;

public interface IFeedsService : IScopedService
{
    Task<WhatsNewFeedChannel> GetLatestChangesAsync(int take = 30);

    Task<WhatsNewFeedChannel> GetBacklogsAsync(int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false,
        bool isInProgress = false);

    Task<WhatsNewFeedChannel> GetQuestionsFeedItemsAsync(int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false);

    Task<WhatsNewFeedChannel> GetQuestionsCommentsFeedItemsAsync(int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetLearningPathsAsync(int pageNumber = 0,
        int? userId = null,
        int recordsPerPage = 15,
        bool showAll = false,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetAllCoursesTopicsAsync();

    Task<WhatsNewFeedChannel> GetAllCoursesAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool onlyActive = true,
        bool showOnlyFinished = true,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetAllVotesAsync(int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetAllAdvertisementsAsync(int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetAllDraftsAsync();

    Task<WhatsNewFeedChannel> GetProjectsNewsAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetProjectsFilesAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetProjectsIssuesAsync(int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetProjectsIssuesRepliesAsync(int count = 15, bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetVotesRepliesAsync(int count = 15, bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetAdvertisementCommentsAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetProjectsFaqsAsync(int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false);

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFaqsAsync(int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 10,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFilesAsync(int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesAsync(int? projectId,
        int pageNumber = 0,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesRepliesAsync(int? projectId,
        int count = 15,
        bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetPostsAsync(int count = 15, bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetCommentsAsync(int count = 15, bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetNewsAsync(int count = 15, bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetTagAsync(string tag,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetAuthorAsync(string authorName,
        int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetNewsCommentsAsync(int count = 15, bool showDeletedItems = false);

    Task<WhatsNewFeedChannel> GetNewsAuthorAsync(string name,
        int pageNumber = 0,
        int recordsPerPage = 20,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<WhatsNewFeedChannel> GetCourseTopicsRepliesAsync(int count = 15, bool onlyActives = true);
}
