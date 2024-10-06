using System.Collections.Concurrent;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Common.Services;

public class SitePageTitlesCacheService(
    BaseHttpClient baseHttpClient,
    ILogger<SitePageTitlesCacheService> logger,
    ICachedAppSettingsProvider appSettingsProvider) : ISitePageTitlesCacheService
{
    private readonly ConcurrentDictionary<string, string> _urlTitles = new(StringComparer.OrdinalIgnoreCase);

    public async Task<string?> GetOrAddSitePageTitleAsync(string? url, bool fetchUrl)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        var cacheKey = GetCacheKey(url);

        try
        {
            if (_urlTitles.TryGetValue(cacheKey, out var title) && !string.IsNullOrWhiteSpace(title))
            {
                return title;
            }

            if (fetchUrl)
            {
                title = await GetDestinationTitleAsync(url);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                return null;
            }

            _urlTitles[cacheKey] = title;

            return title;
        }
        catch (HttpRequestException hre)
        {
            if (!hre.IgnoreIfUrlExists())
            {
                logger.LogError(hre.Demystify(), message: "GetOrAddSitePageTitleAsync({URL})", url);
            }

            return null;
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

    public string? GetPageTitle(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        var cacheKey = GetCacheKey(url);

        if (_urlTitles.TryGetValue(cacheKey, out var title) && !string.IsNullOrWhiteSpace(title))
        {
            return title;
        }

        return null;
    }

    private static string GetCacheKey(string url) => url.ToXxHash64(prefix: "DNT_URL");

    private async Task<string?> GetDestinationTitleAsync(string destinationUrl)
    {
        var destinationUrlHtmlContent = await baseHttpClient.HttpClient.GetStringAsync(destinationUrl);
        var title = destinationUrlHtmlContent.GetHtmlPageTitle();

        if (string.IsNullOrWhiteSpace(title))
        {
            return null;
        }

        var name = (await appSettingsProvider.GetAppSettingsAsync()).BlogName;

        return title.Replace(name, newValue: "", StringComparison.OrdinalIgnoreCase)
            .Trim()
            .TrimStart(trimChar: '|')
            .TrimEnd(trimChar: '|')
            .Trim();
    }
}
