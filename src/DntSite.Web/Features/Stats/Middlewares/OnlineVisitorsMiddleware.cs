using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Middlewares;

public class OnlineVisitorsMiddleware(RequestDelegate next, IOnlineVisitorsService onlineVisitorsService)
{
#pragma warning disable MA0137
    public Task Invoke(HttpContext context)
#pragma warning restore MA0137
    {
        ArgumentNullException.ThrowIfNull(context);

        onlineVisitorsService.UpdateStat(context);

        return next(context);
    }
}
