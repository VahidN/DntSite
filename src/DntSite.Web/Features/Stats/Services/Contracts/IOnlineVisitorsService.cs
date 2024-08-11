namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IOnlineVisitorsService : ISingletonService, IDisposable
{
    int TotalOnlineVisitorsCount { get; }

    int OnlineSpidersCount { get; }

    void UpdateStat(HttpContext context);
}
