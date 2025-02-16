using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsPdfExportService : IScopedService
{
    public Task CreateMergedPdfOfNewsTagsAsync(CancellationToken cancellationToken);

    public Task ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(CancellationToken cancellationToken);

    public ExportDocument? MapDailyNewsItemToExportDocument(DailyNewsItem? post, string siteRootUri);

    public Task<ExportDocument?> MapDailyNewsItemToExportDocumentAsync(int dailyNewsItemId, string siteRootUri);

    public Task<IList<ExportDocument>> MapDailyNewsToExportDocumentsAsync(params IList<int>? dailyNewsItemIds);

    public Task ExportDailyNewsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? dailyNewsItemIds);
}
