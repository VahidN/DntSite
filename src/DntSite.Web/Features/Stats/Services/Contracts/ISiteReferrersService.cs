using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISiteReferrersService : IScopedService
{
    Task DeleteAllAsync();

    Task<bool> TryAddOrUpdateReferrerAsync(string referrerUrl,
        string destinationUrl,
        bool isDestinationUrlProtected,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat);

    Task<SiteReferrer?> FindSiteReferrerAsync(string referrerHash);

    ValueTask<SiteReferrer?> FindSiteReferrerAsync(int id);

    Task<SiteReferrer?> FindLocalReferrerAsync(string? destinationUrl);

    Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer);

    Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(string? destinationUrl,
        int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer);

    Task RemoveSiteReferrerAsync(int id);
}
