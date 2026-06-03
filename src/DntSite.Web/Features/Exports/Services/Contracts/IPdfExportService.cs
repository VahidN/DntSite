using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Services.Contracts;

public interface IPdfExportService : IScopedService
{
    string GetPageTemplateContent();

    Task<string> GetHtmlDocFilePathAsync(WhatsNewItemType itemType, int id);

    void RebuildExports();

    string? GetPhysicalFilePath(string? itemType, string? name);

    Task InvalidateExportedFilesAsync(WhatsNewItemType itemType, params IList<int>? docIds);

    bool HasChangedItem(WhatsNewItemType itemType, IList<int>? postIds);

    IList<(int Id, FileInfo FileInfo)> GetAvailableExportedFiles(WhatsNewItemType itemType);

    string GetExportsOutputFolder(WhatsNewItemType itemType);

    Task<ExportFileLocation?> GetExportFileLocationAsync(WhatsNewItemType? itemType, int id);

    Task<string?> CreateSinglePdfFileAsync(ExportType exportType,
        WhatsNewItemType itemType,
        int id,
        string title,
        bool deleteHtmlDocAtTheEnd,
        params IList<ExportDocument> docs);
}
