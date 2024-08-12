using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISiteReferrersService : IScopedService
{
    Task<bool> TryAddOrUpdateReferrerAsync(string referrerUrl, string destinationUrl);

    Task<SiteReferrer?> FindSiteReferrerAsync(string referrerHash);

    ValueTask<SiteReferrer?> FindSiteReferrerAsync(int id);

    Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(int pageNumber, int recordsPerPage);

    Task RemoveSiteReferrerAsync(int id);
}
