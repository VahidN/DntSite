using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsScreenshotsService : IScopedService
{
    (string FileName, string Path) GetThumbnailImageInfo(int id);

    Task TryReDownloadFailedScreenshotsAsync();

    Task<List<DailyNewsItem>> GetNeedScreenshotsItemsAsync(int count);

    Task DeleteImageAsync(DailyNewsItem? post);

    Task<int> DownloadScreenshotsAsync(int count, CancellationToken cancellationToken);

    Task UpdateAllNewsPageThumbnailsAsync();

    string GetNewsThumbImage(DailyNewsItem? item, string siteRootUri);

    Task InvalidateAllYoutubeScreenshotsAsync();
}
