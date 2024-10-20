using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.Stats.Utils;

namespace DntSite.Web.Features.Stats.Services;

public class OnlineVisitorsService : IOnlineVisitorsService
{
    public const int PurgeInterval = 10; // 10 minutes
    private readonly HashSet<LastSiteUrlVisitorStat> _visitors = new(new LastSiteUrlVisitorStatEqualityComparer());

    public OnlineVisitorsInfoModel GetOnlineVisitorsInfo()
    {
        var localVisitors = _visitors.ToList();

        var totalOnlineVisitorsCount = localVisitors.Count;
        var totalOnlineAuthenticatedUsersCount = localVisitors.Count(x => !x.DisplayName.IsEmpty() && !x.IsSpider);
        var onlineSpidersCount = localVisitors.Count(x => x.IsSpider);

        return new OnlineVisitorsInfoModel
        {
            TotalOnlineAuthenticatedUsersCount = totalOnlineAuthenticatedUsersCount,
            TotalOnlineGuestUsersCount =
                totalOnlineVisitorsCount - (totalOnlineAuthenticatedUsersCount + onlineSpidersCount),
            OnlineSpidersCount = onlineSpidersCount,
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
        _visitors.RemoveWhere(visitor => visitor.VisitTime < purgeDateTime);
    }
}
