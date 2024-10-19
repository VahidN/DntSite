using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class OnlineVisitorsService : IOnlineVisitorsService
{
    public const int PurgeInterval = 10; // 10 minutes
    private readonly List<LastSiteUrlVisitorStat> _visitors = [];

    public OnlineVisitorsInfoModel GetOnlineVisitorsInfo()
    {
        var localVisitors = _visitors.ToList();

        var totalOnlineVisitorsCount = localVisitors.DistinctBy(x => x.Ip).Count();

        var totalOnlineAuthenticatedUsersCount = localVisitors
            .Where(x => !string.IsNullOrWhiteSpace(x.DisplayName) && !x.IsSpider)
            .DistinctBy(x => x.Ip)
            .Count();

        return new OnlineVisitorsInfoModel
        {
            TotalOnlineAuthenticatedUsersCount = totalOnlineAuthenticatedUsersCount,
            TotalOnlineGuestUsersCount = totalOnlineVisitorsCount - totalOnlineAuthenticatedUsersCount,
            OnlineSpidersCount = localVisitors.Where(x => x.IsSpider).DistinctBy(x => x.Ip).Count(),
            TotalOnlineVisitorsCount = totalOnlineVisitorsCount
        };
    }

    public void ProcessNewVisitor(LastSiteUrlVisitorStat item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _visitors.Add(item);
        RemoveOldItems();
    }

    private void RemoveOldItems()
    {
        if (_visitors.Count == 0)
        {
            return;
        }

        var purgeDateTime = DateTime.UtcNow.AddMinutes(-PurgeInterval);
        _visitors.RemoveAll(visitor => visitor.VisitTime < purgeDateTime);
    }
}
