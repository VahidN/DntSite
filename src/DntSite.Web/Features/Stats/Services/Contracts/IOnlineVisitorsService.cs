namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IOnlineVisitorsService : ISingletonService, IDisposable
{
    int OnlineVisitorsCount { get; }

    void UpdateStat(HttpContext context);
}
