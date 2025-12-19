using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Exports.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTopicsPdfExportService : IScopedService
{
    Task CreateMergedPdfOfCoursesAsync(CancellationToken cancellationToken);

    Task<IList<ExportDocument>> MapCourseTopicsToExportDocumentsAsync(params IList<Guid>? postIds);

    Task<IList<ExportDocument>> MapCourseTopicsToExportDocumentsAsync(params IList<int>? postIds);

    ExportDocument? MapCourseTopicToExportDocument(CourseTopic? post, string siteRootUri);

    Task<ExportDocument?> MapCoursePostToExportDocumentAsync(int postId, string siteRootUri);

    Task ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(CancellationToken cancellationToken);

    Task ExportCourseTopicsToSeparatePdfFilesAsync(CancellationToken cancellationToken, params IList<int>? postIds);
}
