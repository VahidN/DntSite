using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsScreenshotsService(
    IUnitOfWork uow,
    IProtectionProviderService protectionProviderService,
    IAppFoldersService appFoldersService,
    IDailyNewsItemsService dailyNewsItemsService,
    IHtmlToPngGenerator htmlToPngGenerator,
    ILogger<DailyNewsScreenshotsService> logger) : IDailyNewsScreenshotsService
{
    private readonly DbSet<DailyNewsItem> _dailyNewsItem = uow.DbSet<DailyNewsItem>();

    public Task<List<DownloadItem>> GetNeedScreenshotsItemsAsync(int count)
        => _dailyNewsItem.AsNoTracking()
            .Where(x => !x.IsDeleted && x.PageThumbnail == null)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .Select(x => new DownloadItem
            {
                Id = x.Id,
                Url = x.Url
            })
            .ToListAsync();

    public async Task<OperationResult<string>> DeleteImageAsync(string pid)
    {
        if (string.IsNullOrWhiteSpace(pid))
        {
            return OperationStat.Failed;
        }

        var id = protectionProviderService.Decrypt(pid)?.ToInt() ?? 0;
        var post = await dailyNewsItemsService.FindDailyNewsItemAsync(id);

        if (post is null)
        {
            return OperationStat.Failed;
        }

        var (name, path) = GetImageInfo(id);
        File.Delete(path);

        return ("", OperationStat.Succeeded, name);
    }

    public async Task DownloadScreenshotsAsync(int count)
    {
        try
        {
            var items = await GetNeedScreenshotsItemsAsync(count);

            foreach (var item in items)
            {
                var (_, path) = GetImageInfo(item.Id);

                await htmlToPngGenerator.GeneratePngFromHtmlAsync(new HtmlToPngGeneratorOptions
                {
                    SourceHtmlFileOrUri = item.Url,
                    OutputPngFile = path
                });

                if (path.IsBlankImage())
                {
                    File.Delete(path);
                }

                await Task.Delay(TimeSpan.FromSeconds(value: 7));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "DownloadScreenshotsAsync Error");
        }
    }

    public async Task UpdateAllNewsPageThumbnailsAsync()
    {
        var availableImageFileIds = GetAvailableImageFileIds();
        var itemsNeedUpdate = await GetItemsNeedUpdateAsync();
        await UpdateRecordIfThereIsImageFileAsync(itemsNeedUpdate, availableImageFileIds);
        await MakePageThumbnailNullForNonExistingFilesAsync(availableImageFileIds);
    }

    private Task<List<DailyNewsItem>> GetItemsNeedUpdateAsync()
        => _dailyNewsItem.Where(x => !x.IsDeleted && x.PageThumbnail == null)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

    private (string FileName, string Path) GetImageInfo(int id)
    {
        var name = Invariant($"news-{id}.jpg");
        var path = Path.Combine(appFoldersService.ThumbnailsServiceFolderPath, name);

        return (name, path);
    }

    private List<int> GetAvailableImageFileIds()
    {
        var imageFiles = Directory.GetFiles(appFoldersService.ThumbnailsServiceFolderPath, searchPattern: "*.jpg");

        var imageIds = new List<int>
        {
            0
        };

        if (imageFiles.Length == 0)
        {
            return imageIds;
        }

        foreach (var item in imageFiles)
        {
            var id = Path.GetFileNameWithoutExtension(item)
                .Replace(oldValue: "news-", string.Empty, StringComparison.OrdinalIgnoreCase)
                .ToInt();

            imageIds.Add(id);
        }

        return imageIds;
    }

    private async Task UpdateRecordIfThereIsImageFileAsync(List<DailyNewsItem> itemsNeedUpdate,
        List<int> availableImageFileIds)
    {
        foreach (var item in itemsNeedUpdate.Where(item => availableImageFileIds.Contains(item.Id)))
        {
            var (name, path) = GetImageInfo(item.Id);

            if (File.Exists(path))
            {
                item.PageThumbnail = name;
            }
        }

        await uow.SaveChangesAsync();
    }

    private async Task MakePageThumbnailNullForNonExistingFilesAsync(List<int> availableImageFileIds)
    {
        var allPublicItemsWithNoImage =
            await _dailyNewsItem.Where(x => !x.IsDeleted && !availableImageFileIds.Contains(x.Id)).ToListAsync();

        foreach (var item in allPublicItemsWithNoImage)
        {
            item.PageThumbnail = null;
        }

        await uow.SaveChangesAsync();
    }
}