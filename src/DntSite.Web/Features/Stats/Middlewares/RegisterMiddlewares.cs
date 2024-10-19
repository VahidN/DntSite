namespace DntSite.Web.Features.Stats.Middlewares;

public static class RegisterMiddlewares
{
    public static IApplicationBuilder UseOnlineVisitorsMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<OnlineVisitorsMiddleware>();
}
