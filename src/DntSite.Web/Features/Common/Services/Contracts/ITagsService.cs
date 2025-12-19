using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Common.Services.Contracts;

public interface ITagsService : IScopedService
{
    Task<List<BacklogTag>> SaveNewBacklogTagsAsync(IList<string> tagsList);

    Task<List<TEntity>> SaveNewTagsAsync<TEntity, TAssociatedEntity>(IList<string>? inputTagsList)
        where TEntity : BaseTagEntity<TAssociatedEntity>, new()
        where TAssociatedEntity : BaseAuditedEntity;

    Task<PagedResultModel<SurveyTag>> GetPagedAllSurveyTagsListAsNoTrackingAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<BacklogTag>> GetPagedAllBacklogTagsListAsNoTrackingAsync(int pageNumber, int recordsPerPage);

    Task<List<BlogPostTag>> GetAllPostTagsListAsNoTrackingAsync(int count);

    Task<PagedResultModel<CourseTag>> GetPagedAllCoursesTagsListAsNoTrackingAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<BlogPostTag>> GetPagedAllPostTagsListAsNoTrackingAsync(int pageNumber, int recordsPerPage);

    Task<List<string>> GetTagNamesArrayAsync(int count);

    Task<List<BlogPostTag>> SaveNewArticleTagsAsync(IList<string> tagsList);

    Task<List<AdvertisementTag>> SaveNewAdvertisementTagsAsync(IList<string> tagsList);

    Task<List<StackExchangeQuestionTag>> SaveNewStackExchangeQuestionsTagsAsync(IList<string>? inputTagsList);

    Task<List<BlogPostTag>> GetThisPostTagsListAsync(int postId);

    ValueTask<BlogPostTag?> FindArticleTagAsync(int tagId);

    Task<string> AvailableTagsToJsonAsync(int count = 2000);

    Task<List<DailyNewsItemTag>> SaveNewLinkItemTagsAsync(IList<string> tagsList);

    Task<List<StackExchangeQuestionTag>> SaveNewQuestionTagsAsync(IList<string> tagsList);

    Task<List<DailyNewsItemTag>> GetAllLinkTagsListAsNoTrackingAsync(int count);

    Task<PagedResultModel<DailyNewsItemTag>> GetPagedAllLinkTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    Task<PagedResultModel<ProjectTag>> GetPagedAllProjectTagsListAsNoTrackingAsync(int pageNumber, int recordsPerPage);

    Task<PagedResultModel<StackExchangeQuestionTag>> GetPagedAllStackExchangeQuestionTagsListAsNoTrackingAsync(
        int pageNumber,
        int recordsPerPage);

    Task<PagedResultModel<AdvertisementTag>> GetPagedAllAdvertisementTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    Task<PagedResultModel<LearningPathTag>> GetPagedAllLearningPathTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    ValueTask<DailyNewsItemTag?> FindLinkTagAsync(int tagId);

    Task<List<ProjectTag>> SaveProjectItemTagsAsync(IList<string> tagsList);

    Task<string> GetTagPdfFileNameAsync(string tagName, string fileName = "dot-net-tips-tag-");

    Task<List<SurveyTag>> SaveVoteTagsAsync(IList<string> tagsList);

    Task<List<CourseTag>> SaveCourseItemTagsAsync(IList<string> tagsList);

    Task<List<LearningPathTag>> SaveNewLearningPathTagsAsync(IList<string> tags);
}
