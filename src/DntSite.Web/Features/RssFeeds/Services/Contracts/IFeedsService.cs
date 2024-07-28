using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.RssFeeds.Services.Contracts;

public interface IFeedsService : IScopedService
{
    Task<FeedChannel> GetLatestChangesAsync(int take = 30);

    Task<FeedChannel> GetBacklogsAsync();

    Task<FeedChannel> GetQuestionsFeedItemsAsync();

    Task<FeedChannel> GetQuestionsCommentsFeedItemsAsync();

    Task<FeedChannel> GetLearningPathsAsync();

    Task<FeedChannel> GetAllCoursesTopicsAsync();

    Task<FeedChannel> GetAllCoursesAsync();

    Task<FeedChannel> GetAllVotesAsync();

    Task<FeedChannel> GetAllAdvertisementsAsync();

    Task<FeedChannel> GetAllDraftsAsync();

    Task<FeedChannel> GetProjectsNewsAsync();

    Task<FeedChannel> GetProjectsFilesAsync();

    Task<FeedChannel> GetProjectsIssuesAsync();

    Task<FeedChannel> GetProjectsIssuesRepliesAsync();

    Task<FeedChannel> GetVotesRepliesAsync();

    Task<FeedChannel> GetAdvertisementCommentsAsync();

    Task<FeedChannel> GetProjectsFaqsAsync();

    Task<(FeedChannel? Items, Project? Project)> GetProjectFaqsAsync(int? id);

    Task<(FeedChannel? Items, Project? Project)> GetProjectFilesAsync(int? id);

    Task<(FeedChannel? Items, Project? Project)> GetProjectIssuesAsync(int? id);

    Task<(FeedChannel? Items, Project? Project)> GetProjectIssuesRepliesAsync(int? id);

    Task<FeedChannel> GetPostsAsync();

    Task<FeedChannel> GetCommentsAsync();

    Task<FeedChannel> GetNewsAsync();

    Task<FeedChannel> GetTagAsync(string id);

    Task<FeedChannel> GetAuthorAsync(string id);

    Task<FeedChannel> GetNewsCommentsAsync();

    Task<FeedChannel> GetNewsAuthorAsync(string id);

    Task<FeedChannel> GetCourseTopicsRepliesAsync();
}
