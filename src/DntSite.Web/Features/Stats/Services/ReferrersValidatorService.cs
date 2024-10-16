using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class ReferrersValidatorService(
    IUAParserService uaParserService,
    ICachedAppSettingsProvider appSettingsProvider,
    IUrlNormalizationService urlNormalizationService) : IReferrersValidatorService
{
    private readonly HashSet<string> _protectedUrls = new(StringComparer.OrdinalIgnoreCase);

    public async Task<bool> ShouldSkipThisRequestAsync(HttpContext context)
    {
        if (context.IsProtectedRoute())
        {
            _protectedUrls.Add(context.GetRawUrl());

            return true;
        }

        var rootUrl = await GetRootUrlAsync(context);

        var referrerUrl = context.GetReferrerUrl();
        var destinationUrl = context.GetRawUrl();

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

        if (string.Equals(urlNormalizationService.NormalizeUrl(referrerUrl),
                urlNormalizationService.NormalizeUrl(destinationUrl), StringComparison.OrdinalIgnoreCase))
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

        return false;
    }

    public async Task<string?> GetNormalizedUrlAsync(string? url)
    {
        if (string.IsNullOrWhiteSpace(url) || !url.IsValidUrl())
        {
            return null;
        }

        var rootUrl = (await appSettingsProvider.GetAppSettingsAsync()).SiteRootUri;

        if (!url.IsReferrerToThisSite(rootUrl))
        {
            return urlNormalizationService.NormalizeUrl(url, defaultProtocol: "https",
                NormalizeUrlRules.LimitProtocols | NormalizeUrlRules.RemoveDefaultDirectoryIndexes |
                NormalizeUrlRules.RemoveTheFragment | NormalizeUrlRules.RemoveDuplicateSlashes |
                NormalizeUrlRules.RemoveTrailingSlashAndEmptyQuery);
        }

        if (url.Contains(value: "/post/", StringComparison.OrdinalIgnoreCase))
        {
            return GetNormalizedPostUrl(url);
        }

        url = url.GetUrlWithoutRssQueryStrings();

        return url.IsEmpty() ? null : urlNormalizationService.NormalizeUrl(url, new Uri(url).Scheme);
    }

    private static string? GetNormalizedPostUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        if (!url.IsValidUrl())
        {
            return null;
        }

        if (!url.Contains(value: "/post/", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        var uri = new Uri(url);

        if (uri.Segments.Length < 2)
        {
            return url;
        }

        var id = uri.Segments[2].Replace(oldValue: "/", string.Empty, StringComparison.OrdinalIgnoreCase).ToInt();

        var domain = uri.IsDefaultPort
            ? uri.Host
            : string.Create(CultureInfo.InvariantCulture, $"{uri.Host}:{uri.Port}");

        return string.Create(CultureInfo.InvariantCulture, $"{uri.Scheme}://{domain}/post/{id}");
    }

    private async Task<string> GetRootUrlAsync(HttpContext context)
    {
        var rootUrl = (await appSettingsProvider.GetAppSettingsAsync()).SiteRootUri;

        return string.IsNullOrWhiteSpace(rootUrl) ? context.GetBaseUrl() : rootUrl;
    }
}
