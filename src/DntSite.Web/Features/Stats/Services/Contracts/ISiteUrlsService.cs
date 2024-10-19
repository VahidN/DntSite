using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISiteUrlsService : IScopedService
{
    Task<LastSiteUrlVisitorStat> GetLastSiteUrlVisitorStatAsync(HttpContext context);

    Task<PagedResultModel<SiteUrl>> GetPagedSiteUrlsAsync(int pageNumber, int recordsPerPage, bool isSpider);

    Task<SiteUrl?> GetOrAddOrUpdateSiteUrlAsync(string? url,
        string? title,
        bool? isProtectedPage,
        bool updateVisitsCount,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat);
}
