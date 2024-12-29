using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.Stats.Utils;

namespace DntSite.Web.Features.Stats.Services;

public class OnlineVisitorsService(ILockerService lockerService) : IOnlineVisitorsService
{
    public const int PurgeInterval = 10; // 10 minutes
    private readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(value: 5);
    private readonly HashSet<LastSiteUrlVisitorStat> _visitors = new(new LastSiteUrlVisitorStatEqualityComparer());

    public OnlineVisitorsInfoModel GetOnlineVisitorsInfo()
    {
        using var @lock = lockerService.Lock<OnlineVisitorsService>(_lockTimeout);

        var totalOnlineVisitorsCount = _visitors.Count;
        var totalOnlineAuthenticatedUsersCount = _visitors.Count(x => !x.DisplayName.IsEmpty() && !x.IsSpider);
        var onlineSpidersCount = _visitors.Count(x => x.IsSpider);

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

        using var @lock = lockerService.Lock<OnlineVisitorsService>(_lockTimeout);
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
