using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IStatService : IScopedService
{
    public Task RecalculateTagsInUseCountsAsync<TTagEntity, TAssociatedEntity>(bool showActives = true,
        bool showDeletedItems = false)
        where TTagEntity : BaseTagEntity<TAssociatedEntity>
        where TAssociatedEntity : BaseAuditedEntity;

    public Task UpdateAllUsersRatingsAsync();

    public Task<AgeStatModel?> GetAverageAgeAsync();

    public Task<List<User>> GetTodayBirthdayListAsync();

    public Task<int> GetTodayBirthdayCountAsync();

    public Task<CoursesStatModel> GetCoursesStatAsync(bool onlyActives = true);

    public Task<int> NumberOfLinksAsync(bool showDeletedItems = false);

    public Task<int> NumberOfPostsAsync(bool showDeletedItems = false);

    public Task<int> NumberOfCommentsAsync(bool showDeletedItems = false);

    public Task<int> NumberOfTagsAsync(bool showActives = true);

    public Task<int> NumberOfAdminsAsync(bool showActives = true);

    public Task<int> NumberOfUsersAsync(bool showActives = true);

    public Task UpdateNumberOfLearningPathsAsync(int userId);

    public Task UpdateUserRatingsAsync(User user);

    public Task<int> NumberOfWritersAsync(bool showActives = true);

    public Task<RecalculatePostsModel> GetStatAsync();

    public Task<ProjectsStatModel> GetProjectStatAsync(int projectId, bool showDeletedItems = false);

    public Task<ProjectsStatModel> GetProjectsStatAsync(bool showDeletedItems = false);

    public Task RecalculateAllUsersNumberOfPostsAndCommentsAsync(bool showDeletedItems = false);

    public Task RecalculateThisStackExchangeQuestionCommentsCountsAsync(int postId, bool showDeletedItems = false);

    public Task RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(int userId, bool showDeletedItems = false);

    public Task RecalculateThisBlogPostCommentsCountsAsync(int postId, bool showDeletedItems = false);

    public Task RecalculateAllBlogPostsCommentsCountsAsync(bool showDeletedItems = false);

    public Task RecalculateThisNewsPostCommentsCountsAsync(int postId, bool showDeletedItems = false);

    public Task RecalculateThisProjectIssueCommentsCountsAsync(int issueId, bool showDeletedItems = false);

    public Task RecalculateThisProjectIssuesCountsAsync(int projectId, bool showDeletedItems = false);

    public Task UpdateProjectNumberOfReleasesStatAsync(int projectId, bool showDeletedItems = false);

    public Task UpdateProjectNumberOfIssuesCommentsAsync(int projectId, bool showDeletedItems = false);

    public Task UpdateProjectNumberOfFaqsAsync(int projectId, bool showDeletedItems = false);

    public Task UpdateProjectNumberOfViewsOfFaqAsync(int faqId, bool fromFeed);

    public Task UpdateProjectFileNumberOfDownloadsAsync(int fileId);

    public Task UpdateNumberOfDraftsStatAsync(int userId);

    public Task UpdateTotalVotesCountAsync(int voteId);

    public Task RecalculateThisVoteCommentsCountsAsync(int voteId, bool showDeletedItems = false);

    public Task RecalculateAllAdvertisementTagsInUseCountsAsync(bool onlyInUseItems,
        bool showActives = true,
        bool showDeletedItems = false);

    public Task RecalculateThisAdvertisementCommentsCountsAsync(int id, bool showDeletedItems = false);

    public Task UpdateNumberOfAdvertisementsOfActiveUsersAsync(bool showDeletedItems = false);

    public Task UpdateNumberOfCourseTopicsStatAsync(int courseId);

    public Task RecalculateThisCourseTopicCommentsCountsAsync(int postId);

    public Task UpdateCourseNumberOfTopicsCommentsAsync(int courseId);

    public Task RecalculateThisCourseQuestionsCountsAsync(int courseId);

    public Task RecalculateThisCourseQuestionCommentsCountsAsync(int questionId);

    public Task UpdateNumberOfCourseQuestionsCommentsAsync(int courseId);
}
