using AutoMapper;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.Advertisements.ModelsMappings;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Advertisements.Services;

public class AdvertisementsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IAdvertisementsEmailsService emailsService,
    IMapper mapper,
    ITagsService tagsService,
    IStatService statService,
    IAdvertisementCommentsService advertisementCommentsService,
    IFullTextSearchService fullTextSearchService) : IAdvertisementsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<Advertisement, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<Advertisement> _advertisements = uow.DbSet<Advertisement>();

    public ValueTask<Advertisement?> FindAdvertisementAsync(int id) => _advertisements.FindAsync(id);

    public Task<List<Advertisement>> GetDeletedAnnouncementsListAsync(int pageNumber, int recordsPerPage = 8)
    {
        var skipRecords = pageNumber * recordsPerPage;
        var now = DateTime.UtcNow;

        return _advertisements.AsNoTracking()
            .Where(x => x.IsDeleted || x.DueDate!.Value <= now)
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<AdvertisementReaction, Advertisement>(fkId, reactionType, fromUserId);

    public async Task UpdateNumberOfAdvertisementViewsAsync(Advertisement? advertisement, bool fromFeed)
    {
        if (advertisement is null)
        {
            return;
        }

        advertisement.EntityStat.NumberOfViews++;

        if (fromFeed)
        {
            advertisement.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public async Task UpdateNumberOfAdvertisementViewsAsync(int id, bool fromFeed)
    {
        var advertisement = await FindAdvertisementAsync(id);
        await UpdateNumberOfAdvertisementViewsAsync(advertisement, fromFeed);
    }

    public Task<PagedResultModel<Advertisement>> GetAllUserAdvertisementsAsync(int pageNumber,
        int userId,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _advertisements.AsNoTracking()
            .Where(x => !x.IsDeleted && x.UserId == userId)
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task<Advertisement?> AddAdvertisementAsync(WriteAdvertisementModel writeAdvertisementModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeAdvertisementModel);

        var listOfActualTags = await tagsService.SaveNewAdvertisementTagsAsync(writeAdvertisementModel.Tags);

        var newsItem = mapper.Map<WriteAdvertisementModel, Advertisement>(writeAdvertisementModel);
        newsItem.Tags = listOfActualTags;
        newsItem.UserId = user?.Id;
        var result = _advertisements.Add(newsItem).Entity;
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        return result;
    }

    public async Task MarkAsDeletedAsync(Advertisement? advertisement)
    {
        if (advertisement is null)
        {
            return;
        }

        advertisement.IsDeleted = true;

        await advertisementCommentsService.MarkAllOfAdvertisementCommentsAsDeletedAsync(advertisement.Id);

        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(advertisement.MapToWhatsNewItemModel(siteRootUri: "")
            .DocumentTypeIdHash);
    }

    /// <summary>
    ///     Note: You need to call _tagsService.SaveNewTags first to add the missing tags to the db.
    /// </summary>
    public async Task<Advertisement> SaveAdvertisementAsync(Advertisement advertisement,
        IList<AdvertisementTag> listOfActualTags,
        bool isEditForm = false)
    {
        ArgumentNullException.ThrowIfNull(advertisement);
        ArgumentNullException.ThrowIfNull(listOfActualTags);

        if (advertisement.Tags is { Count: not 0 })
        {
            advertisement.Tags.Clear();
        }

        advertisement.Tags = new List<AdvertisementTag>();

        foreach (var item in listOfActualTags)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                continue;
            }

            item.Name = item.Name.GetCleanedTag()!;
            advertisement.Tags.Add(item);
        }

        if (!isEditForm)
        {
            _advertisements.Add(advertisement);
        }

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(advertisement.MapToWhatsNewItemModel(siteRootUri: ""));

        return advertisement;
    }

    public Task<Advertisement?> GetAdvertisementAsync(int id, bool showDeletedItems = false)
        => _advertisements.Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task<AdvertisementModel> GetAdvertisementLastAndNextPostAsync(int id, bool showDeletedItems = false)
    {
        var now = DateTime.UtcNow;

        // این شماره‌ها پشت سر هم نیستند
        return new AdvertisementModel
        {
            CurrentItem =
                await _advertisements
                    .Where(x => x.IsDeleted == showDeletedItems && x.Id == id &&
                                (!x.DueDate.HasValue || x.DueDate.Value >= now))
                    .Include(x => x.Tags)
                    .Include(x => x.User)
                    .Include(blogPost => blogPost.Reactions)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem = await _advertisements.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id > id &&
                            (!x.DueDate.HasValue || x.DueDate.Value >= now))
                .OrderBy(x => x.Id)
                .Include(x => x.Tags)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(),
            PreviousItem = await _advertisements.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id < id &&
                            (!x.DueDate.HasValue || x.DueDate.Value >= now))
                .OrderByDescending(x => x.Id)
                .Include(x => x.Tags)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .FirstOrDefaultAsync()
        };
    }

    public Task<PagedResultModel<Advertisement>> GetLastAdvertisementsByTagAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var now = DateTime.UtcNow;

        var query = from b in _advertisements.AsNoTracking()
            from t in b.Tags
            where t.Name == tag
            select b;

        query = query.Where(x => x.IsDeleted == showDeletedItems && (!x.DueDate.HasValue || x.DueDate.Value >= now))
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .Include(blogPost => blogPost.Tags);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Advertisement>> GetLastAdvertisementsByUserAsync(string userName,
        int pageNumber,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _advertisements.AsNoTracking()
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .Include(blogPost => blogPost.Tags)
            .Where(x => x.User!.FriendlyName == userName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Advertisement>> GetAnnouncementsListAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var now = DateTime.UtcNow;

        var query = _advertisements.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems && (!x.DueDate.HasValue || x.DueDate.Value >= now))
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Advertisement>> GetLastPagedAdvertisementsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false)
    {
        var now = DateTime.UtcNow;

        var query = _advertisements
            .Where(blogPost => blogPost.IsDeleted == showDeletedItems &&
                               (!blogPost.DueDate.HasValue || blogPost.DueDate.Value >= now))
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(Advertisement.Id), [
            .. GridifyMapings.GetDefaultMappings<Advertisement>(), new GridifyMap<Advertisement>
            {
                From = AdvertisementsMappingsProfiles.AdvertisementTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public async Task UpdateAdvertisementAsync(Advertisement? advertisement,
        WriteAdvertisementModel writeAdvertisementModel)
    {
        ArgumentNullException.ThrowIfNull(writeAdvertisementModel);

        if (advertisement is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveNewAdvertisementTagsAsync(writeAdvertisementModel.Tags);

        mapper.Map(writeAdvertisementModel, advertisement);
        advertisement.Tags = listOfActualTags;

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(advertisement.MapToWhatsNewItemModel(siteRootUri: ""));
    }

    public Task NotifyAddOrUpdateChangesAsync(Advertisement? advertisement) => NotifyDeleteChangesAsync(advertisement);

    public async Task NotifyDeleteChangesAsync(Advertisement? advertisement)
    {
        if (advertisement is null)
        {
            return;
        }

        await statService.RecalculateAllAdvertisementTagsInUseCountsAsync(advertisement.Tags);
        await statService.RecalculateAllAdvertisementTagsInUseCountsAsync(onlyInUseItems: true);

        if (advertisement.UserId.HasValue)
        {
            await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(advertisement.UserId.Value);
            await statService.UpdateNumberOfAdvertisementsOfActiveUsersAsync();
        }

        await emailsService.AddAdvertisementSendEmailAsync(new Advertisement
        {
            Title = advertisement.Title,
            Body = $"حذف تبلیغ  {advertisement.Title} توسط مدیر از سایت ",
            Id = advertisement.Id
        });
    }

    public Task IndexAdvertisementsAsync()
    {
        var now = DateTime.UtcNow;

        var items = _advertisements.AsNoTracking()
            .Where(x => !x.IsDeleted && (!x.DueDate.HasValue || x.DueDate.Value >= now))
            .Include(x => x.User)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }
}
