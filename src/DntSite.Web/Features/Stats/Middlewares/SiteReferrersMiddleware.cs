using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Middlewares;

public class SiteReferrersMiddleware(
    IBackgroundQueueService backgroundQueueService,
    IReferrersValidatorService referrersValidatorService,
    ILogger<SiteReferrersMiddleware> logger) : IMiddleware, ISingletonService
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var referrerUrl = context.GetReferrerUrl();
        var destinationUrl = context.GetRawUrl();

        try
        {
            if (!await referrersValidatorService.ShouldSkipThisRequestAsync(context))
            {
                backgroundQueueService.QueueBackgroundWorkItem(async (_, serviceProvider) =>
                {
                    var siteReferrersService = serviceProvider.GetRequiredService<ISiteReferrersService>();
                    await siteReferrersService.TryAddOrUpdateReferrerAsync(referrerUrl, destinationUrl);
                });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(),
                message:
                "SiteReferrers Error -> RootUrl: {RootUrl}, ReferrerUrl: {ReferrerUrl}, DestinationUrl: {DestinationUrl}, Log: {Log}",
                context.GetBaseUrl(), referrerUrl, destinationUrl, context.Request.LogRequest(responseCode: 500));
        }

        await next(context);
    }
}
