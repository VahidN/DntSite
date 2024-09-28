using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsScreenshotsService : IScopedService
{
    Task<List<DailyNewsItem>> GetNeedScreenshotsItemsAsync(int count);

    Task DeleteImageAsync(DailyNewsItem? post);

    Task<int> DownloadScreenshotsAsync(int count);

    Task UpdateAllNewsPageThumbnailsAsync();

    string GetNewsThumbImage(DailyNewsItem? item, string siteRootUri);
}
