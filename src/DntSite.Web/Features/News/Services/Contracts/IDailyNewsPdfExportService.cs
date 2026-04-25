using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsPdfExportService : IScopedService
{
    Task<List<int>> FindIdsNeedUpdateAsync(CancellationToken cancellationToken);

    Task CreateMergedPdfOfNewsTagsAsync(ExportType exportType, CancellationToken cancellationToken);

    Task ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(ExportType exportType,
        CancellationToken cancellationToken = default);

    ExportDocument? MapDailyNewsItemToExportDocument(DailyNewsItem? post, string siteRootUri);

    Task<ExportDocument?> MapDailyNewsItemToExportDocumentAsync(int dailyNewsItemId, string siteRootUri);

    Task<IList<ExportDocument>> MapDailyNewsToExportDocumentsAsync(params IList<int>? dailyNewsItemIds);

    Task ExportDailyNewsToSeparatePdfFilesAsync(ExportType exportType,
        CancellationToken cancellationToken,
        params IList<int>? dailyNewsItemIds);
}
