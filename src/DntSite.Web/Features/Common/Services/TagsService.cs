using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Common.Services;

public class TagsService(IUnitOfWork uow) : ITagsService
{
    private readonly DbSet<AdvertisementTag> _advertisementTags = uow.DbSet<AdvertisementTag>();
    private readonly DbSet<BacklogTag> _backlogsTag = uow.DbSet<BacklogTag>();
    private readonly DbSet<BlogPost> _blogPosts = uow.DbSet<BlogPost>();
    private readonly DbSet<CourseTag> _courseTags = uow.DbSet<CourseTag>();
    private readonly DbSet<DailyNewsItemTag> _dailyNewsItemTag = uow.DbSet<DailyNewsItemTag>();
    private readonly DbSet<LearningPathTag> _learningPathTag = uow.DbSet<LearningPathTag>();
    private readonly DbSet<ProjectTag> _projectTags = uow.DbSet<ProjectTag>();
    private readonly DbSet<StackExchangeQuestionTag> _questionTags = uow.DbSet<StackExchangeQuestionTag>();
    private readonly DbSet<BlogPostTag> _tags = uow.DbSet<BlogPostTag>();
    private readonly DbSet<SurveyTag> _voteTags = uow.DbSet<SurveyTag>();

    public Task<List<DailyNewsItemTag>> GetAllLinkTagsListAsNoTrackingAsync(int count)
        => _dailyNewsItemTag.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .ToListAsync();

    public async Task<List<string>> GetTagNamesArrayAsync(int count)
    {
        var array1 = await _tags.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .Select(x => x.Name)
            .Distinct()
            .ToArrayAsync();

        var array2 = await _dailyNewsItemTag.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .Select(x => x.Name)
            .Distinct()
            .ToArrayAsync();

        var array3 = await _projectTags.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .Select(x => x.Name)
            .Distinct()
            .ToArrayAsync();

        var array5 = await _voteTags.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .Select(x => x.Name)
            .Distinct()
            .ToArrayAsync();

        var array6 = await _advertisementTags.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .Select(x => x.Name)
            .Distinct()
            .ToArrayAsync();

        var array7 = await _courseTags.AsNoTracking()
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name)
            .Take(count)
            .Select(x => x.Name)
            .Distinct()
            .ToArrayAsync();

        return [.. array1.Union(array2)
            .Union(array3)
            .Union(array5)
            .Union(array6)
            .Union(array7)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)];
    }

    public async Task<string> AvailableTagsToJsonAsync(int count = 2000)
    {
        var tags = await GetTagNamesArrayAsync(count);

        return JsonSerializer.Serialize(tags);
    }

    public Task<List<StackExchangeQuestionTag>> SaveNewStackExchangeQuestionsTagsAsync(IList<string>? inputTagsList)
        => SaveNewTagsAsync<StackExchangeQuestionTag, StackExchangeQuestion>(inputTagsList);

    public ValueTask<DailyNewsItemTag?> FindLinkTagAsync(int tagId) => _dailyNewsItemTag.FindAsync(tagId);

    public async Task<string> GetTagPdfFileNameAsync(string tagName, string fileName = "dot-net-tips-tag-")
    {
        var tag = await _tags.AsNoTracking().OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Name == tagName);

        return tag is null
            ? string.Empty
            : string.Create(CultureInfo.InvariantCulture,
                $"{fileName}{tag.Id}-{tag.Name.RemoveIllegalCharactersFromFileName()}.pdf");
    }

    public Task<List<BlogPostTag>> GetAllPostTagsListAsNoTrackingAsync(int count)
        => _tags.AsNoTracking().OrderByDescending(x => x.InUseCount).ThenBy(x => x.Name).Take(count).ToListAsync();

    public Task<PagedResultModel<BlogPostTag>> GetPagedAllPostTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _tags.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<DailyNewsItemTag>> GetPagedAllLinkTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _dailyNewsItemTag.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<ProjectTag>> GetPagedAllProjectTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _projectTags.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<StackExchangeQuestionTag>> GetPagedAllStackExchangeQuestionTagsListAsNoTrackingAsync(
        int pageNumber,
        int recordsPerPage)
    {
        var query = _questionTags.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<AdvertisementTag>> GetPagedAllAdvertisementTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _advertisementTags.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<BacklogTag>> GetPagedAllBacklogTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _backlogsTag.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<CourseTag>> GetPagedAllCoursesTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _courseTags.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<SurveyTag>> GetPagedAllSurveyTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _voteTags.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<LearningPathTag>> GetPagedAllLearningPathTagsListAsNoTrackingAsync(int pageNumber,
        int recordsPerPage)
    {
        var query = _learningPathTag.AsNoTracking()
            .Where(x => !x.IsDeleted && x.InUseCount > 0)
            .OrderByDescending(x => x.InUseCount)
            .ThenBy(x => x.Name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<List<BlogPostTag>> GetThisPostTagsListAsync(int postId)
        => _blogPosts.AsNoTracking()
            .Include(x => x.Tags)
            .Where(x => x.Id == postId)
            .SelectMany(x => x.Tags)
            .ToListAsync();

    public ValueTask<BlogPostTag?> FindArticleTagAsync(int tagId) => _tags.FindAsync(tagId);

    public async Task<List<TEntity>> SaveNewTagsAsync<TEntity, TAssociatedEntity>(IList<string>? inputTagsList)
        where TEntity : BaseTagEntity<TAssociatedEntity>, new()
        where TAssociatedEntity : BaseAuditedEntity
    {
        if (inputTagsList is null || inputTagsList.Count == 0)
        {
            inputTagsList = ["C#"];
        }

        var normalizedTags = inputTagsList.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.GetCleanedTag()!)
            .ToArray();

        var baseTagEntities = uow.DbSet<TEntity>();
        var availableDbTags = await baseTagEntities.Where(x => normalizedTags.Contains(x.Name)).ToListAsync();

        foreach (var normalizedTag in normalizedTags)
        {
            if (!availableDbTags.Exists(x => string.Equals(x.Name, normalizedTag, StringComparison.OrdinalIgnoreCase)))
            {
                var newTag = baseTagEntities.Add(new TEntity
                    {
                        Name = normalizedTag,
                        InUseCount = 0
                    })
                    .Entity;

                availableDbTags.Add(newTag);
            }
        }

        await uow.SaveChangesAsync();

        return availableDbTags;
    }

    public Task<List<BlogPostTag>> SaveNewArticleTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<BlogPostTag, BlogPost>(tagsList);

    public Task<List<DailyNewsItemTag>> SaveNewLinkItemTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<DailyNewsItemTag, DailyNewsItem>(tagsList);

    public Task<List<StackExchangeQuestionTag>> SaveNewQuestionTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<StackExchangeQuestionTag, StackExchangeQuestion>(tagsList);

    public Task<List<AdvertisementTag>> SaveNewAdvertisementTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<AdvertisementTag, Advertisement>(tagsList);

    public Task<List<BacklogTag>> SaveNewBacklogTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<BacklogTag, Backlog>(tagsList);

    public Task<List<ProjectTag>> SaveProjectItemTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<ProjectTag, Project>(tagsList);

    public Task<List<SurveyTag>> SaveVoteTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<SurveyTag, Survey>(tagsList);

    public Task<List<CourseTag>> SaveCourseItemTagsAsync(IList<string> tagsList)
        => SaveNewTagsAsync<CourseTag, Course>(tagsList);

    public Task<List<LearningPathTag>> SaveNewLearningPathTagsAsync(IList<string> tags)
        => SaveNewTagsAsync<LearningPathTag, LearningPath>(tags);
}
