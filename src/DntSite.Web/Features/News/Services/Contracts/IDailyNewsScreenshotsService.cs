using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsScreenshotsService : IScopedService
{
    public Task TryReDownloadFailedScreenshotsAsync();

    public Task<List<DailyNewsItem>> GetNeedScreenshotsItemsAsync(int count);

    public Task DeleteImageAsync(DailyNewsItem? post);

    public Task<int> DownloadScreenshotsAsync(int count, CancellationToken cancellationToken);

    public Task UpdateAllNewsPageThumbnailsAsync();

    public string GetNewsThumbImage(DailyNewsItem? item, string siteRootUri);

    public Task InvalidateAllYoutubeScreenshotsAsync();
}
