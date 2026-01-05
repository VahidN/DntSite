namespace DntSite.Web.Features.News.Services.Contracts;

public interface IYoutubeScreenshotsService : ISingletonService
{
    Task<byte[]?> TryGetYoutubeVideoThumbnailDataAsync(string? videoId);

    (bool Success, string? VideoId) IsYoutubeVideo(string? url);

    Task<string?> GetYoutubeVideoDescriptionAsync(string? url);
}
