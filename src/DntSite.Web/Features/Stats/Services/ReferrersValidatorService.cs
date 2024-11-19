using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class ReferrersValidatorService(
    ICachedAppSettingsProvider appSettingsProvider,
    IUrlNormalizationService urlNormalizationService) : IReferrersValidatorService
{
    private static readonly string[] IgnoresList =
    [
        "/api/", "/file/", "/feed/", "/feeds/", "/error/", "/search-results/", "/whats-new/", "/more-like-this/"
    ];

    private readonly HashSet<string> _protectedUrls = new(StringComparer.OrdinalIgnoreCase);

    public async Task<string?> GetNormalizedUrlAsync([NotNullIfNotNull(nameof(url))] string? url)
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

    public async Task<bool> ShouldSkipThisRequestAsync([NotNullWhen(returnValue: false)] string? referrerUrl,
        [NotNullWhen(returnValue: false)] string? destinationUrl,
        string baseUrl,
        bool isProtectedRoute)
    {
        if (isProtectedRoute)
        {
            if (!destinationUrl.IsEmpty())
            {
                _protectedUrls.Add(destinationUrl);
            }

            return true;
        }

        var rootUrl = await GetRootUrlAsync(baseUrl);

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

        if (HasIgnorePattern(destinationUrl, rootUrl) || HasIgnorePattern(referrerUrl, rootUrl))
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

        if (!destinationUrl.IsReferrerToThisSite(rootUrl))
        {
            return true;
        }

        if (destinationUrl.IsStaticFileUrl())
        {
            return true;
        }

        if (IsFromLocalHost(referrerUrl))
        {
            return true;
        }

        return false;
    }

    private static bool IsFromLocalHost(string referrerUrl)
        => referrerUrl.Contains(value: "localhost", StringComparison.OrdinalIgnoreCase);

    private static bool HasIgnorePattern(string url, string rootUrl)
        => url.IsReferrerToThisSite(rootUrl) &&
           IgnoresList.Any(item => url.Contains(item, StringComparison.OrdinalIgnoreCase));

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

        if (uri.Segments.Length <= 2)
        {
            return url;
        }

        var id = uri.Segments[2].Replace(oldValue: "/", string.Empty, StringComparison.OrdinalIgnoreCase).ToInt();

        if (id == 0)
        {
            return url;
        }

        var domain = uri.IsDefaultPort
            ? uri.Host
            : string.Create(CultureInfo.InvariantCulture, $"{uri.Host}:{uri.Port}");

        return string.Create(CultureInfo.InvariantCulture, $"{uri.Scheme}://{domain}/post/{id}");
    }

    private async Task<string> GetRootUrlAsync(string baseUrl)
    {
        var rootUrl = (await appSettingsProvider.GetAppSettingsAsync()).SiteRootUri;

        return string.IsNullOrWhiteSpace(rootUrl) ? baseUrl : rootUrl;
    }
}
