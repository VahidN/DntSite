using System.Text;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.ModelsMappings;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Services;

public class PdfExportService(
    IHtmlToPdfGenerator htmlToPdfGenerator,
    IAppFoldersService appFoldersService,
    IAppSettingsService appSettingsService,
    ILockerService lockerService,
    IFileNameSanitizerService fileNameSanitizerService,
    ICacheService cacheService,
    ILogger<PdfExportService> logger) : IPdfExportService
{
    private const string PdfPageTemplateFileName = "pdf-page-template.html";
    private readonly TimeSpan _lockTimeout = TimeSpan.FromMinutes(value: 30);

    private string? _siteRootUri;

    public async Task<ExportFileLocation?> GetExportFileLocationAsync(WhatsNewItemType? itemType, int id)
    {
        if (itemType is null)
        {
            return null;
        }

        var cacheKey = GetCacheKey(itemType, id);

        return await cacheService.GetOrAddAsync(cacheKey, nameof(PdfExportService), async () =>
        {
            var siteRootUri = _siteRootUri ??= (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;
            var domain = new Uri(siteRootUri).Host;

            var outputPdfFileName = string.Create(CultureInfo.InvariantCulture, $"{domain}-{itemType.Name}-{id}.pdf")
                .ToLowerInvariant();

            var outputFolder = GetExportsOutputFolder(itemType);
            var outputPdfFilePath = Path.Combine(outputFolder, outputPdfFileName);
            var fileExists = outputPdfFilePath.FileExists();

            return new ExportFileLocation
            {
                IsReady = fileExists,
                OutputFolder = outputFolder,
                OutputPdfFileName = outputPdfFileName,
                OutputPdfFilePath = outputPdfFilePath,
                OutputPdfFileSize = fileExists ? new FileInfo(outputPdfFilePath).Length.ToFormattedFileSize() : "",
                OutputPdfFileUrl = fileExists
                    ? siteRootUri.CombineUrl(
                        outputPdfFilePath.Replace(appFoldersService.GetWebRootAppDataFolderPath(), newValue: "",
                            StringComparison.OrdinalIgnoreCase), escapeRelativeUrl: false)
                    : string.Empty
            };
        }, DateTimeOffset.UtcNow.AddDays(days: 1));
    }

    public string GetExportsOutputFolder(WhatsNewItemType itemType)
    {
        ArgumentNullException.ThrowIfNull(itemType);

        var path = Path.Combine(appFoldersService.ExportsPath, itemType.Name.ToLowerInvariant());

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }

    public string? GetPhysicalFilePath(string? itemType, string? name)
    {
        if (itemType is null || name is null)
        {
            return null;
        }

        var outputFolder = Path.Combine(appFoldersService.ExportsPath, itemType.ToLowerInvariant());
        var safeFile = fileNameSanitizerService.IsSafeToDownload(outputFolder, $"{name.ToLowerInvariant()}.pdf");

        return !safeFile.IsSafeToDownload ? null : safeFile.SafeFilePath;
    }

    public void RebuildExports()
    {
        appFoldersService.ExportsPath.DeleteFiles(SearchOption.AllDirectories, ".pdf");
        cacheService.RemoveAllCachedEntries(nameof(PdfExportService));
    }

    public IList<(int Id, FileInfo FileInfo)> GetAvailableExportedFiles(WhatsNewItemType itemType)
    {
        var files = new DirectoryInfo(GetExportsOutputFolder(itemType)).GetFiles(searchPattern: "*.pdf");

        return files.Length == 0
            ? []
            : files.Select(item => (
                    Path.GetFileNameWithoutExtension(item.FullName)
                        .Split(separator: '-', StringSplitOptions.RemoveEmptyEntries)[^1]
                        .ToInt(), item))
                .ToList();
    }

    public bool HasChangedItem(WhatsNewItemType itemType, IList<int>? postIds)
    {
        if (postIds is null || postIds.Count == 0)
        {
            return false;
        }

        var files = GetAvailableExportedFiles(itemType);

        if (files.Count == 0)
        {
            return false;
        }

        var postFiles = files.Where(x => postIds.Contains(x.Id)).ToList();

        if (postFiles.Count == 0)
        {
            return false;
        }

        var lastWriteTimeUtc = postFiles.Max(x => x.FileInfo.CreationTimeUtc.ToDateOnly());

        var yesterday = DateTime.UtcNow.AddDays(value: -1).ToDateOnly();
        var today = DateTime.UtcNow.ToDateOnly();

        return today == lastWriteTimeUtc || yesterday == lastWriteTimeUtc;
    }

    public async Task InvalidateExportedFilesAsync(WhatsNewItemType itemType, params IList<int>? docIds)
    {
        ArgumentNullException.ThrowIfNull(itemType);

        if (docIds is null || docIds.Count == 0)
        {
            return;
        }

        using var @lock = await lockerService.LockAsync<PdfExportService>(_lockTimeout);

        foreach (var id in docIds)
        {
            var location = await GetExportFileLocationAsync(itemType, id);
            location?.OutputPdfFilePath.TryDeleteFile(logger);
            cacheService.Remove(GetCacheKey(itemType, id));
        }
    }

    public async Task<string?> CreateSinglePdfFileAsync(WhatsNewItemType itemType,
        int id,
        string title,
        params IList<ExportDocument> docs)
    {
        ArgumentNullException.ThrowIfNull(docs);
        ArgumentNullException.ThrowIfNull(itemType);

        using var @lock = await lockerService.LockAsync<PdfExportService>(_lockTimeout);

        string? tempHtmlDocFilePath = null;
        string? outputPdfFilePath = null;

        try
        {
            tempHtmlDocFilePath = await CreateMergedHtmlDocFileAsync(title, docs);

            if (tempHtmlDocFilePath.IsEmpty())
            {
                return null;
            }

            outputPdfFilePath = (await GetExportFileLocationAsync(itemType, id))?.OutputPdfFilePath;

            if (outputPdfFilePath.IsEmpty())
            {
                return null;
            }

            outputPdfFilePath.TryDeleteFile();

            var metadata = await CreatePdfDocumentMetadataAsync(itemType, id, title, docs);

            await htmlToPdfGenerator.GeneratePdfFromHtmlAsync(new HtmlToPdfGeneratorOptions
            {
                SourceHtmlFileOrUri = tempHtmlDocFilePath,
                OutputFilePath = outputPdfFilePath,
                DocumentMetadata = metadata
            });

            cacheService.Remove(GetCacheKey(itemType, id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "CreatePdfFileAsync({Id}, `{Type}`:`{Name}`:`{Title}`): ", id,
                itemType.Value, itemType.Name, title);

            outputPdfFilePath.TryDeleteFile(logger);
            outputPdfFilePath = null;
        }
        finally
        {
            tempHtmlDocFilePath.TryDeleteFile(logger);
        }

        return outputPdfFilePath;
    }

    private static string GetCacheKey(WhatsNewItemType itemType, int id)
        => string.Create(CultureInfo.InvariantCulture, $"___{itemType.Name}_{id}___").ToLowerInvariant();

    private async Task<string?> CreateMergedHtmlDocFileAsync(string title, params IList<ExportDocument>? docs)
    {
        if (docs is null || docs.Count == 0)
        {
            return null;
        }

        var mergedBodySb = new StringBuilder();

        foreach (var doc in docs)
        {
            mergedBodySb.AppendLine(doc.ToHtmlDocumentBody());
        }

        var htmlDoc = string.Format(CultureInfo.InvariantCulture, await GetPageTemplateContentAsync(), title.ApplyRle(),
            mergedBodySb.ToString());

        htmlDoc = htmlDoc.ToHtmlWithLocalImageUrls(appFoldersService.GetFolderPath(FileType.Image),
            appFoldersService.GetFolderPath(FileType.CourseImage), appFoldersService.GetFolderPath(FileType.NewsThumb));

        var tempHtmlDocFilePath = Path.Combine(appFoldersService.ExportsAssetsFolder, $"temp-{Guid.NewGuid():N}.html");
        await File.WriteAllTextAsync(tempHtmlDocFilePath, htmlDoc);

        return tempHtmlDocFilePath;
    }

    private async Task<PdfDocumentMetadata> CreatePdfDocumentMetadataAsync(WhatsNewItemType itemType,
        int id,
        string title,
        params IList<ExportDocument> docs)
    {
        var author = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        if (docs.Count == 1)
        {
            return new PdfDocumentMetadata
            {
                Title = docs[index: 0].Title,
                Subject = string.Create(CultureInfo.InvariantCulture, $"{itemType.Value}/{docs[index: 0].Id}"),
                Author = docs[index: 0].Author,
                Creator = author,
                Keywords = docs[index: 0].Tags.Count > 0 ? docs[index: 0].Tags.Aggregate((s1, s2) => $"{s1}, {s2}") : ""
            };
        }

        var tags = docs.SelectMany(x => x.Tags).Distinct().ToList();

        return new PdfDocumentMetadata
        {
            Title = title,
            Subject = string.Create(CultureInfo.InvariantCulture, $"{itemType.Value}/{id}"),
            Author = author,
            Creator = author,
            Keywords = tags.Count > 0 ? tags.Aggregate((s1, s2) => $"{s1}, {s2}") : ""
        };
    }

    private async Task<string> GetPageTemplateContentAsync()
    {
        var exportsAssetsFolder = appFoldersService.ExportsAssetsFolder;
        var pageTemplatePath = Path.Combine(exportsAssetsFolder, PdfPageTemplateFileName);
        var pageTemplateContent = await File.ReadAllTextAsync(pageTemplatePath);

        return pageTemplateContent;
    }
}
