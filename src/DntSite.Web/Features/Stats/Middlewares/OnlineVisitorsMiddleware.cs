using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Middlewares;

public class OnlineVisitorsMiddleware(IOnlineVisitorsService onlineVisitorsService) : IMiddleware, ISingletonService
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        onlineVisitorsService.UpdateStat(context);

        return next(context);
    }
}
