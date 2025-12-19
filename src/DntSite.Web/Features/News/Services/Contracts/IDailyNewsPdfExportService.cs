using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsPdfExportService : IScopedService
{
    Task CreateMergedPdfOfNewsTagsAsync(CancellationToken cancellationToken);

    Task ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(CancellationToken cancellationToken);

    ExportDocument? MapDailyNewsItemToExportDocument(DailyNewsItem? post, string siteRootUri);

    Task<ExportDocument?> MapDailyNewsItemToExportDocumentAsync(int dailyNewsItemId, string siteRootUri);

    Task<IList<ExportDocument>> MapDailyNewsToExportDocumentsAsync(params IList<int>? dailyNewsItemIds);

    Task ExportDailyNewsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? dailyNewsItemIds);
}
