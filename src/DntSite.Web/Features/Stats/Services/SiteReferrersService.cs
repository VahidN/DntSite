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
        bool isDestinationUrlProtected,
        LastSiteUrlVisitorStat lastSiteUrlVisitorStat)
    {
        if (string.IsNullOrWhiteSpace(destinationUrl) || string.IsNullOrWhiteSpace(referrerUrl))
        {
            return false;
        }

        try
        {
            var normalizedDestinationUrl = await referrersValidatorService.GetNormalizedUrlAsync(destinationUrl);
            var normalizedReferrerUrl = await referrersValidatorService.GetNormalizedUrlAsync(referrerUrl);

            if (normalizedDestinationUrl.AreNullOrEmptyOrEqual(normalizedReferrerUrl,
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (await appSettingsService.IsBannedReferrerAsync(normalizedReferrerUrl))
            {
                return false;
            }

            var destinationSiteUrl = await siteUrlsService.GetOrAddOrUpdateSiteUrlAsync(normalizedDestinationUrl,
                title: "", isDestinationUrlProtected, updateVisitsCount: false, lastSiteUrlVisitorStat);

            var destinationTitle = destinationSiteUrl?.IsHidden == true ? null : destinationSiteUrl?.Title;

            var referrerSiteUrl = await siteUrlsService.GetOrAddOrUpdateSiteUrlAsync(normalizedReferrerUrl, title: "",
                isProtectedPage: null, updateVisitsCount: false, lastSiteUrlVisitorStat);

            var referrerTitle = referrerSiteUrl?.IsHidden == true ? null : referrerSiteUrl?.Title;

            if (destinationTitle.AreNullOrEmptyOrEqual(referrerTitle, StringComparison.OrdinalIgnoreCase))
            {
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
                    ReferrerUrl = normalizedReferrerUrl,
                    DestinationUrl = normalizedDestinationUrl,
                    DestinationTitle = destinationTitle,
                    VisitHash = referrerHash,
                    VisitsCount = 1,
                    LastVisitTime = DateTime.UtcNow,
                    IsLocalReferrer = referrerUrl.IsLocalReferrer(destinationUrl)
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

    public async Task<PagedResultModel<SiteReferrer>> GetPagedSiteReferrersAsync(string? destinationUrl,
        int pageNumber,
        int recordsPerPage,
        bool isLocalReferrer)
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

        var query = _referrers.AsNoTracking()
            .Where(x => !x.IsDeleted && x.IsLocalReferrer == isLocalReferrer && x.DestinationUrl == url)
            .OrderByDescending(x => x.VisitsCount)
            .ThenByDescending(x => x.LastVisitTime);

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
    }
}
