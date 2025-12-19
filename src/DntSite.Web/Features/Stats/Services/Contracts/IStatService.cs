using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IStatService : IScopedService
{
    Task RecalculateTagsInUseCountsAsync<TTagEntity, TAssociatedEntity>(bool showActives = true,
        bool showDeletedItems = false)
        where TTagEntity : BaseTagEntity<TAssociatedEntity>
        where TAssociatedEntity : BaseAuditedEntity;

    Task UpdateAllUsersRatingsAsync(CancellationToken cancellationToken = default);

    Task<AgeStatModel?> GetAverageAgeAsync();

    Task<List<User>> GetTodayBirthdayListAsync(DateTime limit);

    Task<int> GetTodayBirthdayCountAsync(DateTime limit);

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

    Task<RecalculatePostsModel> GetStatAsync();

    Task<ProjectsStatModel> GetProjectStatAsync(int projectId, bool showDeletedItems = false);

    Task<ProjectsStatModel> GetProjectsStatAsync(bool showDeletedItems = false);

    Task RecalculateAllUsersNumberOfPostsAndCommentsAsync(bool showDeletedItems = false);

    Task RecalculateThisStackExchangeQuestionCommentsCountsAsync(int postId, bool showDeletedItems = false);

    Task RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(int userId, bool showDeletedItems = false);

    Task RecalculateThisBlogPostCommentsCountsAsync(int postId, bool showDeletedItems = false);

    Task RecalculateAllBlogPostsCommentsCountsAsync(bool showDeletedItems = false);

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

    Task RecalculateAllAdvertisementTagsInUseCountsAsync(bool onlyInUseItems,
        bool showActives = true,
        bool showDeletedItems = false);

    Task RecalculateThisAdvertisementCommentsCountsAsync(int id, bool showDeletedItems = false);

    Task UpdateNumberOfAdvertisementsOfActiveUsersAsync(bool showDeletedItems = false);

    Task UpdateNumberOfCourseTopicsStatAsync(int courseId);

    Task RecalculateThisCourseTopicCommentsCountsAsync(int postId);

    Task UpdateCourseNumberOfTopicsCommentsAsync(int courseId);

    Task RecalculateThisCourseQuestionsCountsAsync(int courseId);

    Task RecalculateThisCourseQuestionCommentsCountsAsync(int questionId);

    Task UpdateNumberOfCourseQuestionsCommentsAsync(int courseId);
}
