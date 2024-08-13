using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Middlewares;

public class OnlineVisitorsMiddleware(IOnlineVisitorsService onlineVisitorsService) : IMiddleware, ISingletonService
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        await onlineVisitorsService.UpdateStatAsync(context);
        await next(context);
    }
}
