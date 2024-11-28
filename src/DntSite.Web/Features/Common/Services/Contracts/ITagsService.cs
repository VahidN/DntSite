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
    public Task<List<BacklogTag>> SaveNewBacklogTagsAsync(IList<string> tagsList);

    public Task<List<TEntity>> SaveNewTagsAsync<TEntity, TAssociatedEntity>(IList<string>? inputTagsList)
        where TEntity : BaseTagEntity<TAssociatedEntity>, new()
        where TAssociatedEntity : BaseAuditedEntity;

    public Task<PagedResultModel<SurveyTag>> GetPagedAllSurveyTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public Task<PagedResultModel<BacklogTag>> GetPagedAllBacklogTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public Task<List<BlogPostTag>> GetAllPostTagsListAsNoTrackingAsync(int count);

    public Task<PagedResultModel<CourseTag>> GetPagedAllCoursesTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public Task<PagedResultModel<BlogPostTag>> GetPagedAllPostTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public Task<List<string>> GetTagNamesArrayAsync(int count);

    public Task<List<BlogPostTag>> SaveNewArticleTagsAsync(IList<string> tagsList);

    public Task<List<AdvertisementTag>> SaveNewAdvertisementTagsAsync(IList<string> tagsList);

    public Task<List<StackExchangeQuestionTag>> SaveNewStackExchangeQuestionsTagsAsync(IList<string>? inputTagsList);

    public Task<List<BlogPostTag>> GetThisPostTagsListAsync(int postId);

    public ValueTask<BlogPostTag?> FindArticleTagAsync(int tagId);

    public Task<string> AvailableTagsToJsonAsync(int count = 2000);

    public Task<List<DailyNewsItemTag>> SaveNewLinkItemTagsAsync(IList<string> tagsList);

    public Task<List<StackExchangeQuestionTag>> SaveNewQuestionTagsAsync(IList<string> tagsList);

    public Task<List<DailyNewsItemTag>> GetAllLinkTagsListAsNoTrackingAsync(int count);

    public Task<PagedResultModel<DailyNewsItemTag>> GetPagedAllLinkTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public Task<PagedResultModel<ProjectTag>> GetPagedAllProjectTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public Task<PagedResultModel<StackExchangeQuestionTag>> GetPagedAllStackExchangeQuestionTagsListAsNoTrackingAsync(
        int pageNumber,
        int recordsPerPage);

    public Task<PagedResultModel<AdvertisementTag>> GetPagedAllAdvertisementTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public Task<PagedResultModel<LearningPathTag>> GetPagedAllLearningPathTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage);

    public ValueTask<DailyNewsItemTag?> FindLinkTagAsync(int tagId);

    public Task<List<ProjectTag>> SaveProjectItemTagsAsync(IList<string> tagsList);

    public Task<string> GetTagPdfFileNameAsync(string tagName, string fileName = "dot-net-tips-tag-");

    public Task<List<SurveyTag>> SaveVoteTagsAsync(IList<string> tagsList);

    public Task<List<CourseTag>> SaveCourseItemTagsAsync(IList<string> tagsList);

    public Task<List<LearningPathTag>> SaveNewLearningPathTagsAsync(IList<string> tags);
}
