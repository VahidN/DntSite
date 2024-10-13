using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services;

namespace DntSite.Web.Features.Stats.Middlewares;

public class OnlineVisitorsMiddleware(
    IBackgroundQueueService backgroundQueueService,
    IUAParserService uaParserService,
    ILogger<OnlineVisitorsMiddleware> logger) : IMiddleware, ISingletonService
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            var ua = context.GetUserAgent() ?? "Unknown";
            var referrerUrl = context.GetReferrerUrl();

            var newVisit = new OnlineVisitorInfoModel
            {
                Ip = context.GetIP() ?? "::1",
                VisitTime = DateTime.UtcNow,
                IsSpider = await uaParserService.IsSpiderClientAsync(ua),
                UserAgent = ua,
                ReferrerUrl = referrerUrl,
                ReferrerUrlTitle = referrerUrl,
                VisitedUrl = context.GetRawUrl(),
                IsProtectedPage = context.IsProtectedRoute(),
                RootUrl = context.GetBaseUrl(),
                ClientInfo = await uaParserService.GetClientInfoAsync(context),
                DisplayName = context.User.GetFirstUserClaimValue(UserRolesService.DisplayNameClaim)
            };

            backgroundQueueService.QueueBackgroundWorkItem(async (_, serviceProvider) =>
            {
                var onlineVisitorsService = serviceProvider.GetRequiredService<IOnlineVisitorsService>();
                await onlineVisitorsService.ProcessItemAsync(newVisit);
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "OnlineVisitorsMiddleware Error");
        }

        await next(context);
    }
}
