using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementsService : IScopedService
{
    ValueTask<Advertisement?> FindAdvertisementAsync(int id);

    Task<List<Advertisement>> GetDeletedAnnouncementsListAsync(int pageNumber, int recordsPerPage = 8);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task UpdateNumberOfAdvertisementViewsAsync(Advertisement? advertisement, bool fromFeed);

    Task UpdateNumberOfAdvertisementViewsAsync(int id, bool fromFeed);

    Task<PagedResultModel<Advertisement>> GetAllUserAdvertisementsAsync(int pageNumber,
        int userId,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<Advertisement?> AddAdvertisementAsync(WriteAdvertisementModel writeAdvertisementModel, User? user);

    Task MarkAsDeletedAsync(Advertisement? advertisement);

    /// <summary>
    ///     Note: You need to call _tagsService.SaveNewTags first to add the missing tags to the db.
    /// </summary>
    Task<Advertisement> SaveAdvertisementAsync(Advertisement advertisement,
        IList<AdvertisementTag> listOfActualTags,
        bool isEditForm = false);

    Task<Advertisement?> GetAdvertisementAsync(int id, bool showDeletedItems = false);

    Task<AdvertisementModel> GetAdvertisementLastAndNextPostAsync(int id, bool showDeletedItems = false);

    Task<PagedResultModel<Advertisement>> GetLastAdvertisementsByTagAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Advertisement>> GetLastAdvertisementsByUserAsync(string userName,
        int pageNumber,
        int recordsPerPage = 8,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Advertisement>> GetAnnouncementsListAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Advertisement>> GetLastPagedAdvertisementsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    Task UpdateAdvertisementAsync(Advertisement? advertisement, WriteAdvertisementModel writeAdvertisementModel);

    Task NotifyAddOrUpdateChangesAsync(Advertisement? advertisement);

    Task NotifyDeleteChangesAsync(Advertisement? advertisement);
}
