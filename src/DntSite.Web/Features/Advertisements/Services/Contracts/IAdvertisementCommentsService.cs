using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Advertisements.Services.Contracts;

public interface IAdvertisementCommentsService : IScopedService
{
    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task MarkAllOfAdvertisementCommentsAsDeletedAsync(int id);

    public Task<List<AdvertisementComment>> GetRootCommentsOfAdvertisementAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    public ValueTask<AdvertisementComment?> FindAdvertisementCommentAsync(int id);

    public Task<AdvertisementComment?> FindAdvertisementCommentIncludeParentAsync(int id);

    public Task<List<AdvertisementComment>> GetLastPagedAdvertisementCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false);

    public AdvertisementComment AddAdvertisementComment(AdvertisementComment comment);

    public Task<PagedResultModel<AdvertisementComment>> GetPagedLastAdvertisementCommentsIncludeBlogPostAndUserAsync(
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<AdvertisementComment>> GetLastPagedAdvertisementsCommentsAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<List<AdvertisementComment>> GetLastPagedDeletedAdvertisementCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8);

    public Task DeleteCommentAsync(int? modelFormCommentId);

    public Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    public Task AddReplyAsync(int? modelFormCommentId, int modelFormPostId, string modelComment, int currentUserUserId);

    public Task IndexAdvertisementCommentsAsync();
}
