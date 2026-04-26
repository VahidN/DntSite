using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Services;

public class EPubExportDocsInfoService(IPdfExportService pdfExportService, IAppFoldersService appFoldersService)
    : IEPubExportDocsInfoService
{
    public async Task<(string? Title, string? FileName)> GetNextLastItemsAsync(WhatsNewItemType type,
        EPubContentItem item,
        bool isAscending,
        int numberOfTries,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(item);
        var currentId = isAscending ? 1 : -1;

        while (true)
        {
            var (content, title, fileName) = await GetDocInfoAsync(type, item.Id + currentId, cancellationToken);

            if (title is not null && content is not null && fileName is not null)
            {
                return (title, fileName);
            }

            if (isAscending)
            {
                currentId++;
            }
            else
            {
                currentId--;
            }

            if (Math.Abs(currentId) > numberOfTries)
            {
                break;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(value: 100), cancellationToken);
        }

        return (null, null);
    }

    public string GetEPubTocPath(string domain, int page)
        => string.Create(CultureInfo.InvariantCulture, $"{domain}-toc-page-{page}.html");

    public string GetArticlesTocPath(string domain, int page)
        => string.Create(CultureInfo.InvariantCulture, $"{domain}-articles-toc-page-{page}.html");

    public string GetAuthorsTocPath(string domain, int page)
        => string.Create(CultureInfo.InvariantCulture, $"{domain}-authors-toc-page-{page}.html");

    public string GetTagsTocPath(string domain, int page)
        => string.Create(CultureInfo.InvariantCulture, $"{domain}-articles-tags-toc-page-{page}.html");

    public string GetLearningPathsTocPath(string domain, int page)
        => string.Create(CultureInfo.InvariantCulture, $"{domain}-learning-paths-toc-page-{page}.html");

    public string GetCoursesTocPath(string domain, int page)
        => string.Create(CultureInfo.InvariantCulture, $"{domain}-courses-toc-page-{page}.html");

    public string GetNewsTocPath(string domain, int page)
        => string.Create(CultureInfo.InvariantCulture, $"{domain}-news-toc-page-{page}.html");

    public async Task<(string? Content, string? Title, string? FileName)> GetDocInfoAsync(WhatsNewItemType type,
        int itemId,
        CancellationToken cancellationToken)
    {
        var (htmlDocFilePath, fileName) = await GetDocPathAsync(type, itemId);

        if (htmlDocFilePath.IsEmpty() || fileName.IsEmpty())
        {
            return (null, null, null);
        }

        var content = await File.ReadAllTextAsync(htmlDocFilePath, cancellationToken);
        var title = content.GetHtmlPageTitle();

        return (content, title, fileName);
    }

    public async Task<(string? DocPath, string? FileName)> GetDocPathAsync(WhatsNewItemType type, int id)
    {
        var path = await pdfExportService.GetHtmlDocFilePathAsync(type, id);
        var name = path.GetFileName();

        return path.FileExists() ? (path, name) : (null, null);
    }

    public string GetEbookFilePath()
    {
        var epubExportDir = appFoldersService.ExportsAssetsFolder;

        epubExportDir.DeleteFiles(SearchOption.AllDirectories, "*.epub");

        return epubExportDir.SafePathCombine(
            $"dnt-{DateTime.IranNowUtc.Persian.Text.ShortDate.Replace(oldChar: '/', newChar: '-')}.epub")!;
    }
}
