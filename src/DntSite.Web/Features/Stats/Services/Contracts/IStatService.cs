using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IStatService : IScopedService
{
    Task UpdateAllUsersRatingsAsync();

    Task<AgeStatModel?> GetAverageAgeAsync();

    Task<List<User>> GetTodayBirthdayListAsync();

    Task<int> GetTodayBirthdayCountAsync();

    Task<CoursesStatModel> GetCoursesStatAsync(bool onlyActives = true);

    Task<int> NumberOfLinksAsync(bool showDeletedItems = false);

    Task<int> NumberOfPostsAsync(bool showDeletedItems = false);

    Task<int> NumberOfCommentsAsync(bool showDeletedItems = false);

    Task<int> NumberOfTagsAsync(bool showActives = true);

    Task<int> NumberOfAdminsAsync(bool showActives = true);

    Task<int> NumberOfUsersAsync(bool showActives = true);

    Task UpdateNumberOfLearningPathsAsync(int userId);

    Task UpdateUserRatingsAsync(User user);

    Task<int> NumberOfWritersAsync(bool showActives = true);

    Task RecalculateAllVoteTagsInUseCountsAsync(string[] tags, bool showActives = true, bool showDeletedItems = false);

    Task<RecalculatePostsModel> GetStatAsync();

    Task<ProjectsStatModel> GetProjectStatAsync(int projectId, bool showDeletedItems = false);

    Task<ProjectsStatModel> GetProjectsStatAsync(bool showDeletedItems = false);

    Task RecalculateAllUsersNumberOfPostsAndCommentsAsync(bool showDeletedItems = false);

    Task RecalculateAllBlogPostTagsInUseCountsAsync(bool showActives = true, bool showDeletedItems = false);

    Task RecalculateThisStackExchangeQuestionCommentsCountsAsync(int postId, bool showDeletedItems = false);

    Task RecalculateBlogPostTagsInUseCountsAsync(IList<BlogPostTag> tags,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(int userId, bool showDeletedItems = false);

    Task RecalculateThisTagInUseCountsAsync(string tagName, bool showActives = true, bool showDeletedItems = false);

    Task RecalculateThisTagInUseCountsAsync(int tagId, bool showActives = true, bool showDeletedItems = false);

    Task RecalculateThisBlogPostCommentsCountsAsync(int postId, bool showDeletedItems = false);

    Task RecalculateAllBlogPostsCommentsCountsAsync(bool showDeletedItems = false);

    Task RecalculateAllLinkTagsInUseCountsAsync(IList<string> tags,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateAllQuestionTagsInUseCountsAsync(IList<string> tags,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateAllProjectTagsInUseCountsAsync(IList<string> tags,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateThisNewsPostCommentsCountsAsync(int postId, bool showDeletedItems = false);

    Task RecalculateThisProjectIssueCommentsCountsAsync(int issueId, bool showDeletedItems = false);

    Task RecalculateThisProjectIssuesCountsAsync(int projectId, bool showDeletedItems = false);

    Task UpdateProjectNumberOfReleasesStatAsync(int projectId, bool showDeletedItems = false);

    Task UpdateProjectNumberOfIssuesCommentsAsync(int projectId, bool showDeletedItems = false);

    Task UpdateProjectNumberOfFaqsAsync(int projectId, bool showDeletedItems = false);

    Task UpdateProjectNumberOfViewsOfFaqAsync(int faqId, bool fromFeed);

    Task UpdateProjectFileNumberOfDownloadsAsync(int fileId);

    Task UpdateNumberOfDraftsStatAsync(int userId);

    Task UpdateTotalVotesCountAsync(int voteId);

    Task RecalculateThisVoteCommentsCountsAsync(int voteId, bool showDeletedItems = false);

    Task RecalculateAllAdvertisementTagsInUseCountsAsync(ICollection<AdvertisementTag> tags,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateAllAdvertisementTagsInUseCountsAsync(bool onlyInUseItems,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateThisAdvertisementCommentsCountsAsync(int id, bool showDeletedItems = false);

    Task UpdateNumberOfAdvertisementsOfActiveUsersAsync(bool showDeletedItems = false);

    Task RecalculateAllCourseTagsInUseCountsAsync(IList<string> tags, bool onlyIsActive = true);

    Task UpdateNumberOfCourseTopicsStatAsync(int courseId);

    Task RecalculateThisCourseTopicCommentsCountsAsync(int postId);

    Task UpdateCourseNumberOfTopicsCommentsAsync(int courseId);

    Task RecalculateThisCourseQuestionsCountsAsync(int courseId);

    Task RecalculateThisCourseQuestionCommentsCountsAsync(int questionId);

    Task UpdateNumberOfCourseQuestionsCommentsAsync(int courseId);

    Task RecalculateAllLearningPathTagsInUseCountsAsync(IList<LearningPathTag> tags,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateAllBacklogTagsInUseCountsAsync(IList<string> backlogTags,
        bool showActives = true,
        bool showDeletedItems = false);
}
