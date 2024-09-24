using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsScreenshotsService : IScopedService
{
    Task<List<DownloadItem>> GetNeedScreenshotsItemsAsync(int count);

    Task DeleteImageAsync(DailyNewsItem? post);

    Task<int> DownloadScreenshotsAsync(int count);

    Task UpdateAllNewsPageThumbnailsAsync();

    string GetNewsThumbImage(DailyNewsItem? item, string siteRootUri);
}
