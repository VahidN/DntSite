using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class SiteReferrersService(
    IUnitOfWork uow,
    BaseHttpClient baseHttpClient,
    IAppSettingsService appSettingsService,
    IHtmlHelperService htmlHelperService,
    ILogger<SiteReferrersService> logger,
    IPasswordHasherService hasherService,
    ISitePageTitlesCacheService sitePageTitlesCacheService) : ISiteReferrersService
{
    private readonly DbSet<SiteReferrer> _referrers = uow.DbSet<SiteReferrer>();

    public async Task<bool> TryAddOrUpdateReferrerAsync(string referrerUrl, string destinationUrl)
    {
        try
        {
            var referrerUrlHtmlContent = await baseHttpClient.HttpClient.GetStringAsync(referrerUrl);

            if (!await IsValidReferrerAsync(referrerUrl, destinationUrl, referrerUrlHtmlContent))
            {
                return false;
            }

            var referrerHash =
                hasherService.GetSha1Hash(Invariant($"{referrerUrl}_{destinationUrl}").ToUpperInvariant());

            var siteReferrer = await FindSiteReferrerAsync(referrerHash);

            var destinationTitle =
                await sitePageTitlesCacheService.GetOrAddSitePageTitleAsync(destinationUrl, fetchUrl: true);

            var referrerTitle = GetReferrerTitle(referrerUrl, referrerUrlHtmlContent);

            if (siteReferrer is null)
            {
                _referrers.Add(new SiteReferrer
                {
                    ReferrerTitle = referrerTitle,
                    ReferrerUrl = referrerUrl,
                    DestinationUrl = destinationUrl,
                    DestinationTitle = destinationTitle,
                    VisitHash = referrerHash,
                    VisitsCount = 1,
                    LastVisitTime = DateTime.UtcNow
                });
            }
            else
            {
                siteReferrer.ReferrerTitle = referrerTitle;
                siteReferrer.DestinationTitle = destinationTitle;
                siteReferrer.LastVisitTime = DateTime.UtcNow;
                siteReferrer.VisitsCount++;
            }

            await uow.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "TryAddOrUpdateReferrerAsync({ReferrerUrl}, {DestinationUrl}): ", referrerUrl,
                destinationUrl);

            return false;
        }
    }

    public Task<SiteReferrer?> FindSiteReferrerAsync(string referrerHash)
        => _referrers.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.VisitHash == referrerHash);

    public ValueTask<SiteReferrer?> FindSiteReferrerAsync(int id) => _referrers.FindAsync(id);

    public Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(int pageNumber, int recordsPerPage)
    {
        var query = _referrers.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.LastVisitTime)
            .ThenByDescending(x => x.VisitsCount);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public async Task RemoveSiteReferrerAsync(int id)
    {
        var item = await FindSiteReferrerAsync(id);

        if (item is null)
        {
            return;
        }

        item.IsDeleted = true;
        await uow.SaveChangesAsync();
    }

    private string GetReferrerTitle(string referrerUrl, string referrerUrlHtmlContent)
    {
        var title = htmlHelperService.GetHtmlPageTitle(referrerUrlHtmlContent);
        sitePageTitlesCacheService.AddSitePageTitle(referrerUrl, title);

        return string.IsNullOrWhiteSpace(title) ? referrerUrl : title;
    }

    private async Task<bool>
        IsValidReferrerAsync(string referrerUrl, string destinationUrl, string referrerUrlHtmlContent)
        => !IsSpam(referrerUrlHtmlContent, destinationUrl) &&
           !await appSettingsService.IsBannedReferrerAsync(referrerUrl);

    private static bool IsSpam(string? referrerUrlHtmlContent, string destinationUrl)
        => string.IsNullOrWhiteSpace(referrerUrlHtmlContent) ||
           referrerUrlHtmlContent.Contains(value: "<iframe", StringComparison.OrdinalIgnoreCase) ||
           !referrerUrlHtmlContent.Contains(destinationUrl.GetHostWithoutSubDomain(),
               StringComparison.OrdinalIgnoreCase);
}
