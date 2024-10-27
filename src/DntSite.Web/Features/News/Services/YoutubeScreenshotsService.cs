using DntSite.Web.Features.News.Services.Contracts;
using Microsoft.AspNetCore.WebUtilities;

namespace DntSite.Web.Features.News.Services;

public class YoutubeScreenshotsService(BaseHttpClient baseHttpClient, ILogger<YoutubeScreenshotsService> logger)
    : IYoutubeScreenshotsService
{
    public async Task<byte[]?> TryGetYoutubeVideoThumbnailDataAsync(string? videoId)
    {
        try
        {
            if (videoId.IsEmpty())
            {
                return null;
            }

            var thumbnailUrl = $"https://i.ytimg.com/vi/{videoId}/hqdefault.jpg";

            return await baseHttpClient.HttpClient.DownloadDataAsync(thumbnailUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "GetYoutubeVideThumbnailAsync({ID}) Error", videoId);

            return null;
        }
    }

    public (bool Success, string? VideoId) IsYoutubeVideo(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return (false, null);
        }

        var uri = new Uri(url);

        if (uri.Host.Contains(value: ".youtube.", StringComparison.OrdinalIgnoreCase) && uri.Segments.Length >= 3 &&
            (uri.Segments[1].Equals(value: "embed/", StringComparison.OrdinalIgnoreCase) ||
             uri.Segments[1].Equals(value: "shorts/", StringComparison.OrdinalIgnoreCase)))
        {
            return (true, uri.Segments[2]);
        }

        if (uri.Host.Contains(value: ".youtube.", StringComparison.OrdinalIgnoreCase))
        {
            var queryDictionary = QueryHelpers.ParseQuery(uri.Query);

            return !queryDictionary.TryGetValue(key: "v", out var v) ? (false, null) : (true, v.ToString());
        }

        if (uri.Host.EndsWith(value: "youtu.be", StringComparison.OrdinalIgnoreCase) && uri.Segments.Length >= 2)
        {
            return (true, uri.Segments[1]);
        }

        return (false, null);
    }
}
