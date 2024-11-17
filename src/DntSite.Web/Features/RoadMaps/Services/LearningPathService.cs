using AutoMapper;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.RoadMaps.ModelsMappings;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.RoadMaps.Services;

public class LearningPathService(
    IUnitOfWork uow,
    IMapper mapper,
    ITagsService tagsService,
    IStatService statService,
    IHtmlHelperService htmlHelperService,
    IEmailsFactoryService emailsFactoryService,
    ILearningPathEmailsService emailsService,
    IUserRatingsService userRatingsService,
    IFullTextSearchService fullTextSearchService,
    ILogger<LearningPathService> logger) : ILearningPathService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<LearningPath, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<LearningPath> _learningPaths = uow.DbSet<LearningPath>();

    public ValueTask<LearningPath?> FindLearningPathAsync(int id) => _learningPaths.FindAsync(id);

    public Task<LearningPath?> GetLearningPathAsync(int id, bool showDeletedItems = false)
        => _learningPaths.Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public LearningPath AddLearningPath(LearningPath data) => _learningPaths.Add(data).Entity;

    public IList<int> GetItemPostIds(string contains, LearningPath learningPathItem, string domain)
    {
        if (string.IsNullOrWhiteSpace(learningPathItem?.Description))
        {
            return [];
        }

        var links = htmlHelperService.ExtractLinks(learningPathItem.Description).ToList();

        if (links.Count == 0)
        {
            return [];
        }

        var postIds = new List<int>();

        foreach (var link in links)
        {
            if (!link.IsValidUrl())
            {
                continue;
            }

            var uri = new Uri(link);

            if (!uri.Host.Contains(domain, StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            if (uri.Segments.Length < 3)
            {
                continue;
            }

            if (!uri.Segments[1].Contains(contains, StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            var postId = int.Parse(uri.Segments[2].Replace(oldValue: "/", string.Empty, StringComparison.Ordinal),
                CultureInfo.InvariantCulture);

            postIds.Add(postId);
        }

        return [..postIds.OrderBy(pId => pId)];
    }

    public Task<PagedResultModel<LearningPath>> GetLearningPathsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showAll = false,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _learningPaths.AsNoTracking()
            .Include(x => x.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems);

        if (!showAll)
        {
            query = query.Where(x => !x.IsDeleted);
        }

        if (userId is not null)
        {
            query = query.Where(x => x.UserId == userId.Value);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<LearningPath>> GetLastPagedLearningPathsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false,
        bool showAll = false)
    {
        var query = _learningPaths.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        if (!showAll)
        {
            query = query.Where(x => !x.IsDeleted);
        }

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(LearningPath.Id), [
            .. GridifyMapings.GetDefaultMappings<LearningPath>(), new GridifyMap<LearningPath>
            {
                From = LearningPathMappingsProfiles.LearningPathTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<LearningPathReaction, LearningPath>(fkId, reactionType, fromUserId);

    public async Task<LearningPathDetailsModel> LearningPathDetailsAsync(int id, bool showDeletedItems = false)
        => new()
        {
            CurrentItem =
                await _learningPaths.AsNoTracking()
                    .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
                    .OrderBy(x => x.Id)
                    .Include(x => x.User)
                    .Include(blogPost => blogPost.Tags)
                    .Include(blogPost => blogPost.Reactions)
                    .FirstOrDefaultAsync(),
            NextItem = await _learningPaths.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id > id)
                .OrderBy(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Tags)
                .Include(blogPost => blogPost.Reactions)
                .FirstOrDefaultAsync(),
            PreviousItem = await _learningPaths.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id < id)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Tags)
                .Include(blogPost => blogPost.Reactions)
                .FirstOrDefaultAsync()
        };

    public Task<List<LearningPath>> GetAllPublicLearningPathsOfDateAsync(DateTime date)
        => _learningPaths.AsNoTracking()
            .Include(learningPath => learningPath.User)
            .Where(learningPath => !learningPath.IsDeleted && learningPath.Audit.CreatedAt.Year == date.Year &&
                                   learningPath.Audit.CreatedAt.Month == date.Month &&
                                   learningPath.Audit.CreatedAt.Day == date.Day)
            .OrderBy(learningPath => learningPath.Id)
            .ToListAsync();

    public string GetTagPdfFileName(LearningPath tag, string fileName = "dot-net-tips-learning-path-")
        => tag is null
            ? string.Empty
            : string.Create(CultureInfo.InvariantCulture,
                $"{fileName}{tag.Id}-{tag.Title.RemoveIllegalCharactersFromFileName()}.pdf");

    public async Task UpdateStatAsync(int learningPathId, bool isFromFeed)
    {
        var item = await FindLearningPathAsync(learningPathId);

        if (item is null)
        {
            return;
        }

        item.EntityStat.NumberOfViews++;

        if (isFromFeed)
        {
            item.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public Task<PagedResultModel<LearningPath>> GetLearningPathsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _learningPaths.AsNoTracking()
            from t in b.Tags
            where t.Name == tagName
            select b;

        query = query.Include(x => x.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<LearningPath>> GetLastPagedLearningPathsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        bool showAll = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _learningPaths.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(x => x.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name);

        if (!showAll)
        {
            query = query.Where(x => !x.IsDeleted);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task MarkAsDeletedAsync(LearningPath? learningPathItem)
    {
        if (learningPathItem is null)
        {
            return;
        }

        learningPathItem.IsDeleted = true;
        await uow.SaveChangesAsync();

        logger.LogWarning(message: "Deleted a LearningPath record with Id={Id} and Title={Text}", learningPathItem.Id,
            learningPathItem.Title);

        fullTextSearchService.DeleteLuceneDocument(learningPathItem.MapToWhatsNewItemModel(siteRootUri: "")
            .DocumentTypeIdHash);

        if (learningPathItem.UserId is not null)
        {
            await statService.UpdateNumberOfLearningPathsAsync(learningPathItem.UserId.Value);
        }
    }

    public Task NotifyDeleteChangesAsync(LearningPath? learningPathItem)
    {
        if (learningPathItem is null)
        {
            return Task.CompletedTask;
        }

        return emailsFactoryService.SendTextToAllAdminsAsync(string.Create(CultureInfo.InvariantCulture,
            $"مسیر راه شماره {learningPathItem.Id} حذف شد."));
    }

    public async Task UpdateLearningPathAsync(LearningPath? learningPathItem, LearningPathModel writeLearningPathModel)
    {
        ArgumentNullException.ThrowIfNull(writeLearningPathModel);

        if (learningPathItem is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveNewLearningPathTagsAsync(writeLearningPathModel.Tags);

        mapper.Map(writeLearningPathModel, learningPathItem);
        learningPathItem.Tags = listOfActualTags;

        await uow.SaveChangesAsync();

        await statService.RecalculateTagsInUseCountsAsync<LearningPathTag, LearningPath>();
        fullTextSearchService.AddOrUpdateLuceneDocument(learningPathItem.MapToWhatsNewItemModel(siteRootUri: ""));
    }

    public async Task<LearningPath?> AddLearningPathAsync(LearningPathModel writeLearningPathModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeLearningPathModel);

        var listOfActualTags = await tagsService.SaveNewLearningPathTagsAsync(writeLearningPathModel.Tags);

        var newsItem = mapper.Map<LearningPathModel, LearningPath>(writeLearningPathModel);
        newsItem.Tags = listOfActualTags;
        newsItem.UserId = user?.Id;
        var result = AddLearningPath(newsItem);
        await uow.SaveChangesAsync();

        await statService.RecalculateTagsInUseCountsAsync<LearningPathTag, LearningPath>();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        return result;
    }

    public async Task NotifyAddOrUpdateChangesAsync(LearningPath? learningPathItem)
    {
        if (learningPathItem is null)
        {
            return;
        }

        if (learningPathItem.UserId is not null)
        {
            await statService.UpdateNumberOfLearningPathsAsync(learningPathItem.UserId.Value);
        }

        await emailsService.NewLearningPathSendEmailToAdminsAsync(learningPathItem);
    }

    public async Task IndexLearningPathsAsync()
    {
        var items = await _learningPaths.AsNoTracking()
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        await fullTextSearchService.IndexTableAsync(items.Select(item => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }
}
