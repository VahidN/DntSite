using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementCommentsService : IScopedService
{
    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task MarkAllOfAdvertisementCommentsAsDeletedAsync(int id);

    Task<List<AdvertisementComment>> GetRootCommentsOfAdvertisementAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    ValueTask<AdvertisementComment?> FindAdvertisementCommentAsync(int id);

    Task<AdvertisementComment?> FindAdvertisementCommentIncludeParentAsync(int id);

    Task<List<AdvertisementComment>> GetLastPagedAdvertisementCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false);

    AdvertisementComment AddAdvertisementComment(AdvertisementComment comment);

    Task<PagedResultModel<AdvertisementComment>> GetPagedLastAdvertisementCommentsIncludeBlogPostAndUserAsync(
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<AdvertisementComment>> GetLastPagedAdvertisementsCommentsAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<List<AdvertisementComment>> GetLastPagedDeletedAdvertisementCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8);

    Task DeleteCommentAsync(int? modelFormCommentId);

    Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    Task AddReplyAsync(int? modelFormCommentId, int modelFormPostId, string modelComment, int currentUserUserId);

    Task IndexAdvertisementCommentsAsync();
}
