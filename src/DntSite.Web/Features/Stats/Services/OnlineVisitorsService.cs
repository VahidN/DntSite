using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class OnlineVisitorsService(
    ISitePageTitlesCacheService sitePageTitlesCacheService,
    ILogger<OnlineVisitorsService> logger) : IOnlineVisitorsService
{
    public const int PurgeInterval = 10; // 10 minutes
    private static readonly List<OnlineVisitorInfoModel> Visitors = [];

    public PagedResultModel<OnlineVisitorInfoModel> GetPagedOnlineVisitorsList(int pageNumber,
        int recordsPerPage,
        bool isSpider)
    {
        var skipRecords = pageNumber * recordsPerPage;

        var query = Visitors.Where(x
                => x.IsSpider == isSpider && x is
                {
                    IsStaticFileUrl: false, IsProtectedPage: false, HasMissingVisitedUrlTitle: false
                })
            .ToList();

        var items = query.OrderByDescending(x => x.VisitTime).Skip(skipRecords).Take(recordsPerPage).ToList();

        return new PagedResultModel<OnlineVisitorInfoModel>
        {
            TotalItems = query.Count,
            Data = items
        };
    }

    public OnlineVisitorsInfoModel GetOnlineVisitorsInfo()
    {
        var localVisitors = Visitors.ToList();

        return new OnlineVisitorsInfoModel
        {
            TotalOnlineAuthenticatedUsersCount =
                localVisitors.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName) && !x.IsSpider)
                    .DistinctBy(x => x.Ip)
                    .Count(),
            TotalOnlineGuestUsersCount =
                localVisitors.Where(x => string.IsNullOrWhiteSpace(x.DisplayName) && !x.IsSpider)
                    .DistinctBy(x => x.Ip)
                    .Count(),
            OnlineSpidersCount = localVisitors.Where(x => x.IsSpider).DistinctBy(x => x.Ip).Count(),
            TotalOnlineVisitorsCount = localVisitors.DistinctBy(x => x.Ip).Count()
        };
    }

    public async Task ProcessItemAsync(OnlineVisitorInfoModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        try
        {
            await AddNewItemAsync(item);
            RemoveOldItems();
            TryFixMissingLocalUrlsTitles();
            TryFixMissingReferrerUrlsTitles();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "ProcessItemsAsync");
        }
    }

    private void TryFixMissingReferrerUrlsTitles()
    {
        if (Visitors.Count == 0)
        {
            return;
        }

        foreach (var visitor in Visitors.Where(visitor => visitor.HasMissingReferrerUrlTitle))
        {
            visitor.ReferrerUrlTitle = sitePageTitlesCacheService.GetPageTitle(visitor.ReferrerUrl);
        }
    }

    private void TryFixMissingLocalUrlsTitles()
    {
        if (Visitors.Count == 0)
        {
            return;
        }

        foreach (var visitor in Visitors.Where(visitor => visitor.HasMissingVisitedUrlTitle))
        {
            visitor.VisitedUrlTitle = sitePageTitlesCacheService.GetPageTitle(visitor.VisitedUrl);
        }
    }

    private async Task AddNewItemAsync(OnlineVisitorInfoModel item)
    {
        item.VisitedUrlTitle =
            await sitePageTitlesCacheService.GetOrAddSitePageTitleAsync(item.VisitedUrl, fetchUrl: false);

        Visitors.Add(item);
    }

    private void RemoveOldItems()
    {
        if (Visitors.Count == 0)
        {
            return;
        }

        var purgeDateTime = DateTime.UtcNow.AddMinutes(-PurgeInterval);
        Visitors.RemoveAll(visitor => visitor.VisitTime < purgeDateTime);
    }
}
