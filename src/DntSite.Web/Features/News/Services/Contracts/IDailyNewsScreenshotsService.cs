using DntSite.Web.Features.News.Models;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsScreenshotsService : IScopedService
{
    Task<List<DownloadItem>> GetNeedScreenshotsItemsAsync(int count);

    Task<OperationResult<string>> DeleteImageAsync(string pid);

    Task DownloadScreenshotsAsync(int count);

    Task UpdateAllNewsPageThumbnailsAsync();
}
