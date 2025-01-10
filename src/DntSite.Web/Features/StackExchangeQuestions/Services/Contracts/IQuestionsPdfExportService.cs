using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsPdfExportService : IScopedService
{
    public Task<IList<ExportDocument>> MapQuestionsToExportDocumentsAsync(params IList<int>? postIds);

    public ExportDocument? MapQuestionToExportDocument(StackExchangeQuestion? post, string siteRootUri);

    public Task<ExportDocument?> MapQuestionToExportDocumentAsync(int postId, string siteRootUri);

    public Task ExportNotProcessedQuestionsToSeparatePdfFilesAsync(CancellationToken cancellationToken);

    public Task ExportQuestionsToSeparatePdfFilesAsync(CancellationToken cancellationToken, params IList<int>? postIds);
}
