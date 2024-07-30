using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.RssFeeds.Services.Contracts;

public interface IFeedsService : IScopedService
{
    Task<WhatsNewFeedChannel> GetLatestChangesAsync(int take = 30);

    Task<WhatsNewFeedChannel> GetBacklogsAsync();

    Task<WhatsNewFeedChannel> GetQuestionsFeedItemsAsync();

    Task<WhatsNewFeedChannel> GetQuestionsCommentsFeedItemsAsync();

    Task<WhatsNewFeedChannel> GetLearningPathsAsync();

    Task<WhatsNewFeedChannel> GetAllCoursesTopicsAsync();

    Task<WhatsNewFeedChannel> GetAllCoursesAsync();

    Task<WhatsNewFeedChannel> GetAllVotesAsync();

    Task<WhatsNewFeedChannel> GetAllAdvertisementsAsync();

    Task<WhatsNewFeedChannel> GetAllDraftsAsync();

    Task<WhatsNewFeedChannel> GetProjectsNewsAsync();

    Task<WhatsNewFeedChannel> GetProjectsFilesAsync();

    Task<WhatsNewFeedChannel> GetProjectsIssuesAsync();

    Task<WhatsNewFeedChannel> GetProjectsIssuesRepliesAsync();

    Task<WhatsNewFeedChannel> GetVotesRepliesAsync();

    Task<WhatsNewFeedChannel> GetAdvertisementCommentsAsync();

    Task<WhatsNewFeedChannel> GetProjectsFaqsAsync();

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFaqsAsync(int? id);

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectFilesAsync(int? id);

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesAsync(int? id);

    Task<(WhatsNewFeedChannel? Items, Project? Project)> GetProjectIssuesRepliesAsync(int? id);

    Task<WhatsNewFeedChannel> GetPostsAsync();

    Task<WhatsNewFeedChannel> GetCommentsAsync();

    Task<WhatsNewFeedChannel> GetNewsAsync();

    Task<WhatsNewFeedChannel> GetTagAsync(string id);

    Task<WhatsNewFeedChannel> GetAuthorAsync(string id);

    Task<WhatsNewFeedChannel> GetNewsCommentsAsync();

    Task<WhatsNewFeedChannel> GetNewsAuthorAsync(string id);

    Task<WhatsNewFeedChannel> GetCourseTopicsRepliesAsync();
}
