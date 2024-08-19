using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Common.Services;

public class SitePageTitlesCacheService(
    ICacheService cacheService,
    BaseHttpClient baseHttpClient,
    ILogger<SitePageTitlesCacheService> logger) : ISitePageTitlesCacheService
{
    private readonly DateTimeOffset _expiration = DateTimeOffset.UtcNow.AddDays(days: 1);

    public async Task<string> GetOrAddSitePageTitleAsync(string? url, bool fetchUrl)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return string.Empty;
        }

        var cacheKey = GetCacheKey(url);

        try
        {
            if (cacheService.TryGetValue(cacheKey, out string? title) && !string.IsNullOrWhiteSpace(title))
            {
                return title;
            }

            title = fetchUrl ? await GetDestinationTitleAsync(url) : url;
            cacheService.Add(cacheKey, title, _expiration);

            return title;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "GetOrAddSitePageTitleAsync({URL})", url);

            cacheService.Add(cacheKey, url, _expiration);

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
        cacheService.Add(cacheKey, title, _expiration);
    }

    public string GetPageTitle(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return string.Empty;
        }

        var cacheKey = GetCacheKey(url);

        if (cacheService.TryGetValue(cacheKey, out string? title) && !string.IsNullOrWhiteSpace(title))
        {
            return title;
        }

        return url;
    }

    private static string GetCacheKey(string url) => $"DNT_URL_{url.ToXxHash64():X}";

    private async Task<string> GetDestinationTitleAsync(string destinationUrl)
    {
        var destinationUrlHtmlContent = await baseHttpClient.HttpClient.GetStringAsync(destinationUrl);
        var title = destinationUrlHtmlContent.GetHtmlPageTitle();

        return string.IsNullOrWhiteSpace(title) ? destinationUrl : title;
    }
}
