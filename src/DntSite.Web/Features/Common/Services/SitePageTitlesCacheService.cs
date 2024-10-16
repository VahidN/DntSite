using System.Collections.Concurrent;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Common.Services;

public class SitePageTitlesCacheService(
    BaseHttpClient baseHttpClient,
    ILogger<SitePageTitlesCacheService> logger,
    ICachedAppSettingsProvider appSettingsProvider) : ISitePageTitlesCacheService
{
    private readonly HashSet<string> _dontLogUrls = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, string> _urlTitles = new(StringComparer.OrdinalIgnoreCase);

    public async Task<string?> GetOrAddSitePageTitleAsync(string? url, bool fetchUrl)
    {
        if (string.IsNullOrWhiteSpace(url) || DoNotLog(url))
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
        catch (Exception ex)
        {
            if (ex is not HttpRequestException)
            {
                logger.LogError(ex.Demystify(), message: "GetOrAddSitePageTitleAsync({URL})", url);
            }

            return null;
        }
    }

    public void AddSitePageTitle(string? url, string? title, bool dontLog)
    {
        if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(title))
        {
            return;
        }

        var cacheKey = GetCacheKey(url);
        _urlTitles[cacheKey] = dontLog ? string.Empty : title;

        if (dontLog)
        {
            _dontLogUrls.Add(url);
        }
    }

    public string? GetPageTitle(string? url)
    {
        if (string.IsNullOrWhiteSpace(url) || DoNotLog(url))
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

    private bool DoNotLog(string? url)
        => url.IsEmpty() || _dontLogUrls.Contains(url) ||
           _dontLogUrls.Any(item => url.Contains(item, StringComparison.OrdinalIgnoreCase));

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
