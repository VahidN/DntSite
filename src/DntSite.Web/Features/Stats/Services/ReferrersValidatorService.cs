using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Stats.Middlewares.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using Microsoft.AspNetCore.Http.Extensions;

namespace DntSite.Web.Features.Stats.Services;

public class ReferrersValidatorService(IUAParserService uaParserService, ICachedAppSettingsProvider appSettingsProvider)
    : IReferrersValidatorService
{
    private readonly HashSet<string> _protectedUrls = new(StringComparer.OrdinalIgnoreCase);

    public async Task<bool> ShouldSkipThisRequestAsync(HttpContext context, string referrerUrl, string destinationUrl)
    {
        if (context.IsProtectedRoute())
        {
            _protectedUrls.Add(context.GetRawUrl());

            return true;
        }

        var rootUrl = await GetRootUrlAsync(context);

        if (string.IsNullOrEmpty(referrerUrl) || string.IsNullOrEmpty(destinationUrl))
        {
            return true;
        }

        if (string.Equals(referrerUrl, destinationUrl, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (!destinationUrl.IsValidUrl())
        {
            return true;
        }

        if (!referrerUrl.IsValidUrl())
        {
            return true;
        }

        if (string.Equals(UriHelper.Encode(new Uri(referrerUrl)), UriHelper.Encode(new Uri(destinationUrl)),
                StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (!string.Equals(new Uri(destinationUrl).Scheme, new Uri(rootUrl).Scheme, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (_protectedUrls.Contains(destinationUrl) || _protectedUrls.Contains(referrerUrl))
        {
            return true;
        }

        if (await uaParserService.IsSpiderClientAsync(context))
        {
            return true;
        }

        if (!destinationUrl.IsReferrerToThisSite(rootUrl))
        {
            return true;
        }

        if (destinationUrl.IsStaticFileUrl())
        {
            return true;
        }

        return DoNotLog(context);
    }

    private async Task<string> GetRootUrlAsync(HttpContext context)
    {
        var rootUrl = (await appSettingsProvider.GetAppSettingsAsync()).SiteRootUri;

        return string.IsNullOrWhiteSpace(rootUrl) ? context.GetBaseUrl() : rootUrl;
    }

    private static bool DoNotLog(HttpContext context)
        => context.GetEndpoint()?.Metadata?.GetMetadata<DoNotLogReferrerAttribute>() is not null;
}
