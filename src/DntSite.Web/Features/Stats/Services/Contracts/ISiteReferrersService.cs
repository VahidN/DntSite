using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISiteReferrersService : IScopedService
{
    public Task DeleteAllAsync();

    public Task<bool> TryAddOrUpdateReferrerAsync(string referrerUrl,
        string destinationUrl,
        string baseUrl,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat,
        bool isProtectedPage);

    public Task<SiteReferrer?> FindSiteReferrerAsync(string referrerHash);

    public ValueTask<SiteReferrer?> FindSiteReferrerAsync(int id);

    public Task<SiteReferrer?> FindLocalReferrerAsync(string? destinationUrl);

    public Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer);

    public Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(string? destinationUrl,
        int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer,
        string[] ignoredUrlsPatterns);

    public Task RemoveSiteReferrerAsync(int id);
}
