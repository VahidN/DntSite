using System.Collections.Concurrent;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Common.Services;

public class SitePageTitlesCacheService(BaseHttpClient baseHttpClient, ILogger<SitePageTitlesCacheService> logger)
    : ISitePageTitlesCacheService
{
    private readonly ConcurrentDictionary<string, string> _urlTitles = new(StringComparer.OrdinalIgnoreCase);

    public async Task<string> GetOrAddSitePageTitleAsync(string? url, bool fetchUrl)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return string.Empty;
        }

        var cacheKey = GetCacheKey(url);

        try
        {
            if (_urlTitles.TryGetValue(cacheKey, out var title) && !string.IsNullOrWhiteSpace(title))
            {
                return title;
            }

            title = fetchUrl ? await GetDestinationTitleAsync(url) : url;
            _urlTitles[cacheKey] = title;

            return title;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "GetOrAddSitePageTitleAsync({URL})", url);

            _urlTitles[cacheKey] = url;

            return url;
        }
    }

    public void AddSitePageTitle(string? url, string? title)
    {
        if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(title))
        {
            return;
        }

        var cacheKey = GetCacheKey(url);
        _urlTitles[cacheKey] = title;
    }

    public string GetPageTitle(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return string.Empty;
        }

        var cacheKey = GetCacheKey(url);

        if (_urlTitles.TryGetValue(cacheKey, out var title) && !string.IsNullOrWhiteSpace(title))
        {
            return title;
        }

        return url;
    }

    private static string GetCacheKey(string url) => url.ToXxHash64(prefix: "DNT_URL");

    private async Task<string> GetDestinationTitleAsync(string destinationUrl)
    {
        var destinationUrlHtmlContent = await baseHttpClient.HttpClient.GetStringAsync(destinationUrl);
        var title = destinationUrlHtmlContent.GetHtmlPageTitle();

        return string.IsNullOrWhiteSpace(title) ? destinationUrl : title;
    }
}
