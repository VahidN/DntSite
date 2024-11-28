using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementsService : IScopedService
{
    public ValueTask<Advertisement?> FindAdvertisementAsync(int id);

    public Task<List<Advertisement>> GetDeletedAnnouncementsListAsync(int pageNumber, int recordsPerPage = 8);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task UpdateNumberOfAdvertisementViewsAsync(Advertisement? advertisement, bool fromFeed);

    public Task UpdateNumberOfAdvertisementViewsAsync(int id, bool fromFeed);

    public Task<PagedResultModel<Advertisement>> GetAllUserAdvertisementsAsync(int pageNumber,
        int userId,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<Advertisement?> AddAdvertisementAsync(WriteAdvertisementModel writeAdvertisementModel, User? user);

    public Task MarkAsDeletedAsync(Advertisement? advertisement);

    /// <summary>
    ///     Note: You need to call _tagsService.SaveNewTags first to add the missing tags to the db.
    /// </summary>
    public Task<Advertisement> SaveAdvertisementAsync(Advertisement advertisement,
        IList<AdvertisementTag> listOfActualTags,
        bool isEditForm = false);

    public Task<Advertisement?> GetAdvertisementAsync(int id, bool showDeletedItems = false);

    public Task<AdvertisementModel> GetAdvertisementLastAndNextPostAsync(int id, bool showDeletedItems = false);

    public Task<PagedResultModel<Advertisement>> GetLastAdvertisementsByTagAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Advertisement>> GetLastAdvertisementsByUserAsync(string userName,
        int pageNumber,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Advertisement>> GetAnnouncementsListAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Advertisement>> GetLastPagedAdvertisementsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    public Task UpdateAdvertisementAsync(Advertisement? advertisement, WriteAdvertisementModel writeAdvertisementModel);

    public Task NotifyAddOrUpdateChangesAsync(Advertisement? advertisement);

    public Task NotifyDeleteChangesAsync(Advertisement? advertisement);

    public Task IndexAdvertisementsAsync();
}
