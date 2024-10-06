using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Common.Utils.WebToolkit;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class SiteReferrersService(
    IUnitOfWork uow,
    BaseHttpClient baseHttpClient,
    IAppSettingsService appSettingsService,
    ILogger<SiteReferrersService> logger,
    IPasswordHasherService hasherService,
    ISitePageTitlesCacheService sitePageTitlesCacheService) : ISiteReferrersService
{
    private readonly DbSet<SiteReferrer> _referrers = uow.DbSet<SiteReferrer>();

    public Task DeleteAllAsync() => uow.ExecuteTransactionAsync(() => _referrers.ExecuteDeleteAsync());

    public async Task<bool> TryAddOrUpdateReferrerAsync(string referrerUrl, string destinationUrl, bool isLocalReferrer)
    {
        if (string.IsNullOrWhiteSpace(destinationUrl))
        {
            return false;
        }

        try
        {
            var normalizedDestinationUrl = GetNormalizedDestinationUrl(destinationUrl);

            if (string.IsNullOrWhiteSpace(normalizedDestinationUrl))
            {
                return false;
            }

            var referrerUrlHtmlContent = await GetUrlHtmlContentAsync(referrerUrl);

            if (!await IsValidReferrerAsync(referrerUrl, referrerUrlHtmlContent))
            {
                return false;
            }

            var destinationTitle =
                await sitePageTitlesCacheService.GetOrAddSitePageTitleAsync(normalizedDestinationUrl, fetchUrl: true);

            var referrerTitle =
                await sitePageTitlesCacheService.GetOrAddSitePageTitleAsync(referrerUrl, fetchUrl: true);

            if (string.IsNullOrWhiteSpace(destinationTitle) || string.IsNullOrWhiteSpace(referrerTitle))
            {
                return false;
            }

            var referrerHash = hasherService.GetSha1Hash(string
                .Create(CultureInfo.InvariantCulture, $"{referrerUrl}_{normalizedDestinationUrl}")
                .ToUpperInvariant());

            var siteReferrer = await FindSiteReferrerAsync(referrerHash);

            if (siteReferrer is null)
            {
                _referrers.Add(new SiteReferrer
                {
                    ReferrerTitle = referrerTitle,
                    ReferrerUrl = referrerUrl,
                    DestinationUrl = normalizedDestinationUrl,
                    DestinationTitle = destinationTitle,
                    VisitHash = referrerHash,
                    VisitsCount = 1,
                    LastVisitTime = DateTime.UtcNow,
                    IsLocalReferrer = isLocalReferrer
                });
            }
            else
            {
                if (!referrerTitle.IsValidUrl())
                {
                    siteReferrer.ReferrerTitle = referrerTitle;
                }

                if (!destinationTitle.IsValidUrl())
                {
                    siteReferrer.DestinationTitle = destinationTitle;
                }

                siteReferrer.LastVisitTime = DateTime.UtcNow;
                siteReferrer.VisitsCount++;
            }

            await uow.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            if (ex is not HttpRequestException)
            {
                logger.LogError(ex.Demystify(),
                    message: "TryAddOrUpdateReferrerAsync({ReferrerUrl}, {DestinationUrl}): ", referrerUrl,
                    destinationUrl);
            }

            return false;
        }
    }

    public Task<SiteReferrer?> FindLocalReferrerAsync(string? destinationUrl)
        => string.IsNullOrWhiteSpace(destinationUrl)
            ? Task.FromResult<SiteReferrer?>(result: null)
            : _referrers.AsNoTracking()
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.DestinationUrl == destinationUrl && !x.IsDeleted);

    public ValueTask<SiteReferrer?> FindSiteReferrerAsync(int id) => _referrers.FindAsync(id);

    public Task<SiteReferrer?> FindSiteReferrerAsync(string referrerHash)
        => _referrers.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.VisitHash == referrerHash);

    public Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer)
    {
        var query = _referrers.AsNoTracking()
            .Where(x => !x.IsDeleted && x.IsLocalReferrer == isLocalReferrer)
            .OrderByDescending(x => x.LastVisitTime)
            .ThenByDescending(x => x.VisitsCount);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(string destinationUrl,
        int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer)
    {
        var url = GetNormalizedDestinationUrl(destinationUrl);

        if (string.IsNullOrWhiteSpace(url))
        {
            return Task.FromResult(new PagedResultModel<SiteReferrer>());
        }

        var query = _referrers.AsNoTracking()
            .Where(x => !x.IsDeleted && x.IsLocalReferrer == isLocalReferrer && x.DestinationUrl == url)
            .OrderByDescending(x => x.VisitsCount)
            .ThenByDescending(x => x.LastVisitTime);

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

    private string? GetNormalizedDestinationUrl(string? destinationUrl)
    {
        if (!destinationUrl.IsValidUrl())
        {
            return null;
        }

        if (destinationUrl.Contains(value: "/post/", StringComparison.OrdinalIgnoreCase))
        {
            return destinationUrl.GetNormalizedPostUrl();
        }

        destinationUrl = destinationUrl.GetUrlWithoutRssQueryStrings();

        return destinationUrl;
    }

    private async Task<string?> GetUrlHtmlContentAsync(string url)
    {
        try
        {
            return await baseHttpClient.HttpClient.GetStringAsync(url);
        }
        catch (Exception ex)
        {
            if (ex is not HttpRequestException)
            {
                throw;
            }
        }

        return null;
    }

    private async Task<bool> IsValidReferrerAsync(string referrerUrl, string? referrerUrlHtmlContent)
        => !IsSpam(referrerUrlHtmlContent) && !await appSettingsService.IsBannedReferrerAsync(referrerUrl);

    private static bool IsSpam(string? referrerUrlHtmlContent)
        => referrerUrlHtmlContent?.Contains(value: "<iframe", StringComparison.OrdinalIgnoreCase) == true;
}
