using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsScreenshotsService(
    IUnitOfWork uow,
    IAppFoldersService appFoldersService,
    IHtmlToPngGenerator htmlToPngGenerator,
    IYoutubeScreenshotsService youtubeScreenshotsService,
    ILogger<DailyNewsScreenshotsService> logger) : IDailyNewsScreenshotsService
{
    private const int MaxFetchRetries = 3;
    private readonly DbSet<DailyNewsItem> _dailyNewsItem = uow.DbSet<DailyNewsItem>();

    public Task<List<DailyNewsItem>> GetNeedScreenshotsItemsAsync(int count)
        => _dailyNewsItem
            .Where(x => !x.IsDeleted && x.PageThumbnail == null &&
                        (!x.FetchRetries.HasValue || x.FetchRetries.Value <= MaxFetchRetries))
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public async Task DeleteImageAsync(DailyNewsItem? post)
    {
        if (post is null)
        {
            return;
        }

        var (_, path) = GetImageInfo(post.Id);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        post.PageThumbnail = null;
        await uow.SaveChangesAsync();
    }

    public async Task<int> DownloadScreenshotsAsync(int count)
    {
        var numberOfDownloadedFiles = 0;
        var currentUrl = "";

        try
        {
            var items = await GetNeedScreenshotsItemsAsync(count);

            foreach (var item in items)
            {
                var (name, path) = GetImageInfo(item.Id);
                currentUrl = item.Url;

                item.FetchRetries = item.FetchRetries.HasValue ? item.FetchRetries.Value + 1 : 1;

                await TryDownloadScreenshotAsync(currentUrl, path);

                if (!File.Exists(path))
                {
                    await Task.Delay(TimeSpan.FromSeconds(value: 7));

                    continue;
                }

                if (path.IsBlankImage())
                {
                    File.Delete(path);

                    logger.LogWarning(message: "DownloadScreenshotsAsync({URL}) failed. The received image is blank!",
                        currentUrl);
                }
                else
                {
                    item.PageThumbnail = name;
                    numberOfDownloadedFiles++;
                }

                await Task.Delay(TimeSpan.FromSeconds(value: 7));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "DownloadScreenshotsAsync({URL}) Error", currentUrl);
        }

        await uow.SaveChangesAsync();

        return numberOfDownloadedFiles;
    }

    public async Task UpdateAllNewsPageThumbnailsAsync()
    {
        var availableImageFileIds = GetAvailableImageFileIds();
        var itemsNeedUpdate = await GetItemsNeedUpdateAsync();
        await UpdateRecordIfThereIsImageFileAsync(itemsNeedUpdate, availableImageFileIds);
        await MakePageThumbnailNullForNonExistingFilesAsync(availableImageFileIds);
    }

    public string GetNewsThumbImage(DailyNewsItem? item, string siteRootUri)
    {
        if (item is null || string.IsNullOrWhiteSpace(siteRootUri))
        {
            return string.Empty;
        }

        var (fileName, path) = GetImageInfo(item.Id);

        if (!File.Exists(path))
        {
            return string.Empty;
        }

        var imageUrl = siteRootUri.CombineUrl(
            $"{ApiUrlsRoutingConstants.File.HttpAny.NewsThumb}?name={Uri.EscapeDataString(fileName)}",
            escapeRelativeUrl: false);

        var redirectUrl =
            siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture, $"{NewsRoutingConstants.NewsRedirectBase}/{item.Id}"),
                escapeRelativeUrl: false);

        return $"""
                <br/>
                <a href="{redirectUrl}" rel="nofollow" target="_blank">
                   <img src='{imageUrl}'
                        alt='{item.Title}'
                        title='{item.Title.ApplyRle()}'
                        style='border: 0 none; max-width: 100%; display: block; margin-left: auto; margin-right: auto;' />
                </a>
                """;
    }

    private async Task TryDownloadScreenshotAsync(string currentUrl, string path)
    {
        var (success, videoId) = youtubeScreenshotsService.IsYoutubeVideo(currentUrl);

        if (success)
        {
            var imageData = await youtubeScreenshotsService.TryGetYoutubeVideoThumbnailDataAsync(videoId);

            if (imageData is not null)
            {
                await File.WriteAllBytesAsync(path, imageData);
            }

            return;
        }

        await htmlToPngGenerator.GeneratePngFromHtmlAsync(new HtmlToPngGeneratorOptions
        {
            SourceHtmlFileOrUri = currentUrl,
            OutputPngFile = path
        });
    }

    private Task<List<DailyNewsItem>> GetItemsNeedUpdateAsync()
        => _dailyNewsItem.Where(x => !x.IsDeleted && x.PageThumbnail == null)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

    private (string FileName, string Path) GetImageInfo(int id)
    {
        var name = string.Create(CultureInfo.InvariantCulture, $"news-{id}.jpg");
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
