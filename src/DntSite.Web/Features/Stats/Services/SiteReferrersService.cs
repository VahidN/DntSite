using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class SiteReferrersService(
    IUnitOfWork uow,
    IAppSettingsService appSettingsService,
    IReferrersValidatorService referrersValidatorService,
    ILogger<SiteReferrersService> logger,
    IPasswordHasherService hasherService,
    ISiteUrlsService siteUrlsService) : ISiteReferrersService
{
    private readonly DbSet<SiteReferrer> _referrers = uow.DbSet<SiteReferrer>();

    public Task DeleteAllAsync() => uow.ExecuteTransactionAsync(() => _referrers.ExecuteDeleteAsync());

    public async Task<bool> TryAddOrUpdateReferrerAsync(string referrerUrl,
        string destinationUrl,
        string baseUrl,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat,
        bool isProtectedPage)
    {
        ArgumentNullException.ThrowIfNull(lastSiteUrlVisitorStat);

        if (await referrersValidatorService.ShouldSkipThisRequestAsync(referrerUrl, destinationUrl, baseUrl,
                isProtectedPage))
        {
            return false;
        }

        try
        {
            var normalizedDestinationUrl = await referrersValidatorService.GetNormalizedUrlAsync(destinationUrl);
            var normalizedReferrerUrl = await referrersValidatorService.GetNormalizedUrlAsync(referrerUrl);

            if (await appSettingsService.IsBannedReferrerAsync(normalizedReferrerUrl))
            {
                LogIgnoredReferrer(normalizedReferrerUrl, normalizedDestinationUrl, reason: "IsBannedReferrerAsync");

                return false;
            }

            var destinationSiteUrl = await siteUrlsService.GetUrlTitleAsync(normalizedDestinationUrl,
                lastSiteUrlVisitorStat);

            var referrerSiteUrl = await siteUrlsService.GetUrlTitleAsync(normalizedReferrerUrl, lastSiteUrlVisitorStat);
            var referrerTitle = referrerSiteUrl.Title;

            if (!destinationSiteUrl.SiteUrlId.HasValue || referrerTitle.IsEmpty())
            {
                LogIgnoredReferrer(normalizedReferrerUrl, normalizedDestinationUrl,
                    $"Titles (`{referrerTitle}`,`{destinationSiteUrl.Title}`) are null.");

                return false;
            }

            var referrerHash = hasherService.GetSha1Hash(string
                .Create(CultureInfo.InvariantCulture, $"{normalizedReferrerUrl}_{normalizedDestinationUrl}")
                .ToUpperInvariant());

            var siteReferrer = await FindSiteReferrerAsync(referrerHash);

            if (siteReferrer is null)
            {
                _referrers.Add(new SiteReferrer
                {
                    ReferrerTitle = referrerTitle,
                    ReferrerUrl = normalizedReferrerUrl ?? referrerUrl,
                    VisitHash = referrerHash,
                    VisitsCount = 1,
                    LastVisitTime = DateTime.UtcNow,
                    IsLocalReferrer = referrerUrl.IsLocalReferrer(destinationUrl),
                    DestinationSiteUrlId = destinationSiteUrl.SiteUrlId
                });
            }
            else
            {
                siteReferrer.ReferrerTitle = referrerTitle;
                siteReferrer.DestinationSiteUrlId = destinationSiteUrl.SiteUrlId;
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
                .Include(x => x.DestinationSiteUrl)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.DestinationSiteUrl != null &&
                                          x.DestinationSiteUrl.Url == destinationUrl && !x.IsDeleted);

    public ValueTask<SiteReferrer?> FindSiteReferrerAsync(int id) => _referrers.FindAsync(id);

    public Task<SiteReferrer?> FindSiteReferrerAsync(string referrerHash)
        => _referrers.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.VisitHash == referrerHash);

    public Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer)
    {
        var query = _referrers.AsNoTracking()
            .Include(x => x.DestinationSiteUrl)
            .Where(x => !x.IsDeleted && x.IsLocalReferrer == isLocalReferrer && x.DestinationSiteUrl != null &&
                        !x.DestinationSiteUrl.IsProtectedPage && x.DestinationSiteUrl.Title != "")
            .OrderByDescending(x => x.LastVisitTime)
            .ThenByDescending(x => x.VisitsCount);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public async Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(string? destinationUrl,
        int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer,
        string[] ignoredUrlsPatterns)
    {
        if (string.IsNullOrWhiteSpace(destinationUrl))
        {
            return new PagedResultModel<SiteReferrer>();
        }

        var url = await referrersValidatorService.GetNormalizedUrlAsync(destinationUrl);

        if (string.IsNullOrWhiteSpace(url))
        {
            return new PagedResultModel<SiteReferrer>();
        }

#pragma warning disable CA1307
        var query = _referrers.AsNoTracking()
            .Include(x => x.DestinationSiteUrl)
            .Where(x => !x.IsDeleted && x.IsLocalReferrer == isLocalReferrer && x.DestinationSiteUrl != null &&
                        !ignoredUrlsPatterns.Any(pattern => x.ReferrerUrl.Contains(pattern)) &&
                        x.DestinationSiteUrl.Url == url && !x.DestinationSiteUrl.IsProtectedPage &&
                        x.DestinationSiteUrl.Title != "")
            .OrderByDescending(x => x.VisitsCount)
            .ThenByDescending(x => x.LastVisitTime);
#pragma warning restore CA1307

        return await query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
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

        logger.LogWarning(message: "Deleted a SiteReferrer record with Id={Id} and Title={Text}", item.Id,
            item.ReferrerTitle);
    }

    private void LogIgnoredReferrer(string? normalizedReferrerUrl, string? normalizedDestinationUrl, string reason)
        => logger.LogInformation(message: "Ignored referrer: `{ReferrerUrl}` ❱ `{DestinationUrl}` ❱ {Reason}",
            normalizedReferrerUrl, normalizedDestinationUrl, reason);
}
