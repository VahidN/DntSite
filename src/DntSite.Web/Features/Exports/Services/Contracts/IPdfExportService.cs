using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Services.Contracts;

public interface IPdfExportService : IScopedService
{
    void RebuildExports();

    string? GetPhysicalFilePath(string? itemType, string? name);

    Task InvalidateExportedFilesAsync(WhatsNewItemType itemType, params IList<int>? docIds);

    bool HasChangedItem(WhatsNewItemType itemType, IList<int>? postIds);

    IList<(int Id, FileInfo FileInfo)> GetAvailableExportedFiles(WhatsNewItemType itemType);

    string GetExportsOutputFolder(WhatsNewItemType itemType);

    Task<ExportFileLocation?> GetExportFileLocationAsync(WhatsNewItemType? itemType, int id);

    Task<string?> CreateSinglePdfFileAsync(WhatsNewItemType itemType,
        int id,
        string title,
        params IList<ExportDocument> docs);
}
