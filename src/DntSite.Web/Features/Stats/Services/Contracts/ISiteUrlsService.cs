using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISiteUrlsService : IScopedService
{
    public Task DeleteAllAsync();

    public Task<LastSiteUrlVisitorStat> GetLastSiteUrlVisitorStatAsync(HttpContext context);

    public Task<PagedResultModel<SiteUrl>> GetPagedSiteUrlsAsync(int pageNumber, int recordsPerPage, bool isSpider);

    public Task<SiteUrl?> GetOrAddOrUpdateSiteUrlAsync(string? url,
        string? title,
        bool? isProtectedPage,
        bool updateVisitsCount,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat);

    public Task<(string? Title, int? SiteUrlId)> GetUrlTitleAsync(string? url,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat);

    public Task RemoveSiteUrlAsync(int id);
}
