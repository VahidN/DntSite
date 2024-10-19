using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Middlewares;

public class OnlineVisitorsMiddleware(
    IBackgroundQueueService backgroundQueueService,
    IReferrersValidatorService referrersValidatorService,
    ILogger<OnlineVisitorsMiddleware> logger) : IMiddleware, ISingletonService
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            await AddToReferrersBackgroundQueueAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "OnlineVisitorsMiddleware Error");
        }

        await next(context);
    }

    private async Task AddToReferrersBackgroundQueueAsync(HttpContext context)
    {
        if (await referrersValidatorService.ShouldSkipThisRequestAsync(context))
        {
            return;
        }

        var referrerUrl = context.GetReferrerUrl();
        var destinationUrl = context.GetRawUrl();
        var isProtectedRoute = context.IsProtectedRoute();

        var lastSiteUrlVisitorStat = await context.RequestServices.GetRequiredService<ISiteUrlsService>()
            .GetLastSiteUrlVisitorStatAsync(context);

        backgroundQueueService.QueueBackgroundWorkItem(async (_, serviceProvider) =>
        {
            var siteReferrersService = serviceProvider.GetRequiredService<ISiteReferrersService>();

            await siteReferrersService.TryAddOrUpdateReferrerAsync(referrerUrl, destinationUrl, isProtectedRoute,
                lastSiteUrlVisitorStat);
        });
    }
}
