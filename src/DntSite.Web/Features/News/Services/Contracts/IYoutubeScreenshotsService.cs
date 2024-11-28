namespace DntSite.Web.Features.News.Services.Contracts;

public interface IYoutubeScreenshotsService : ISingletonService
{
    public Task<byte[]?> TryGetYoutubeVideoThumbnailDataAsync(string? videoId);

    public (bool Success, string? VideoId) IsYoutubeVideo(string? url);
}
