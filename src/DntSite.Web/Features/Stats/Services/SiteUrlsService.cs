using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services;

namespace DntSite.Web.Features.Stats.Services;

public class SiteUrlsService(
    IUnitOfWork uow,
    IUAParserService uaParserService,
    IReferrersValidatorService referrersValidatorService,
    ICachedAppSettingsProvider appSettingsProvider) : ISiteUrlsService
{
    private readonly DbSet<SiteUrl> _siteUrls = uow.DbSet<SiteUrl>();

    public async Task<SiteUrl?> GetOrAddOrUpdateSiteUrlAsync(string? url,
        string? title,
        bool? isProtectedPage,
        bool updateVisitsCount,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat)
    {
        url = await referrersValidatorService.GetNormalizedUrlAsync(url);

        if (!url.IsValidUrl())
        {
            return null;
        }

        var appSettings = await appSettingsProvider.GetAppSettingsAsync();

        if (!url.IsReferrerToThisSite(appSettings.SiteRootUri))
        {
            return new SiteUrl
            {
                Title = url.GetUrlDomain().Domain,
                IsProtectedPage = false,
                IsStaticFileUrl = false,
                Url = url
            };
        }

        var cacheKey = GetUrlHash(url);
        var siteUrl = await _siteUrls.OrderBy(x => x.UrlHash).FirstOrDefaultAsync(x => x.UrlHash == cacheKey);

        if (siteUrl is null)
        {
            siteUrl = _siteUrls.Add(new SiteUrl
                {
                    LastSiteUrlVisitorStat = lastSiteUrlVisitorStat,
                    Title = title ?? "",
                    Url = url,
                    UrlHash = cacheKey,
                    VisitsCount = 1,
                    IsProtectedPage = isProtectedPage ?? false,
                    IsStaticFileUrl = url.IsStaticFileUrl()
                })
                .Entity;
        }
        else
        {
            siteUrl.Title = title ?? "";
            siteUrl.LastSiteUrlVisitorStat = lastSiteUrlVisitorStat;

            if (updateVisitsCount)
            {
                siteUrl.VisitsCount++;
            }

            if (isProtectedPage.HasValue)
            {
                siteUrl.IsProtectedPage = isProtectedPage.Value;
            }
        }

        await uow.SaveChangesAsync();

        return siteUrl;
    }

    public async Task<PagedResultModel<SiteUrl>> GetPagedSiteUrlsAsync(int pageNumber,
        int recordsPerPage,
        bool isSpider)
    {
        var query = _siteUrls.AsNoTracking()
            .Where(x => !x.IsDeleted && x.LastSiteUrlVisitorStat.IsSpider == isSpider && !x.IsStaticFileUrl &&
                        !x.IsProtectedPage && x.Title != "")
            .OrderByDescending(x => x.LastSiteUrlVisitorStat.VisitTime)
            .ThenByDescending(x => x.VisitsCount);

        var result = await query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);

        foreach (var item in result.Data)
        {
            item.LastSiteUrlVisitorStat.ClientInfo =
                await uaParserService.GetClientInfoAsync(item.LastSiteUrlVisitorStat.UserAgent);
        }

        return result;
    }

    public async Task<LastSiteUrlVisitorStat> GetLastSiteUrlVisitorStatAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var ua = context.GetUserAgent();

        return new LastSiteUrlVisitorStat
        {
            VisitTime = DateTime.UtcNow,
            Ip = context.GetIP() ?? "::1",
            UserAgent = ua,
            DisplayName = context.User.GetFirstUserClaimValue(UserRolesService.DisplayNameClaim),
            IsSpider = await uaParserService.IsSpiderClientAsync(ua),
            ClientInfo = await uaParserService.GetClientInfoAsync(context)
        };
    }

    public string GetUrlHash(string? url) => url.IsEmpty() ? "".GetSha1Hash() : url.NormalizeUrl().GetSha1Hash();
}
