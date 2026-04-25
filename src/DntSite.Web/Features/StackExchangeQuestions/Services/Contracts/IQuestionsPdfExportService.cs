using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsPdfExportService : IScopedService
{
    Task<List<int>> FindIdsNeedUpdateAsync(CancellationToken cancellationToken);

    Task<IList<ExportDocument>> MapQuestionsToExportDocumentsAsync(params IList<int>? postIds);

    ExportDocument? MapQuestionToExportDocument(StackExchangeQuestion? post, string siteRootUri);

    Task<ExportDocument?> MapQuestionToExportDocumentAsync(int postId, string siteRootUri);

    Task ExportNotProcessedQuestionsToSeparatePdfFilesAsync(ExportType exportType,
        CancellationToken cancellationToken = default);

    Task ExportQuestionsToSeparatePdfFilesAsync(ExportType exportType,
        CancellationToken cancellationToken,
        params IList<int>? postIds);
}
