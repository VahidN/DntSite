using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Exports.Models;

namespace DntSite.Web.Features.Courses.Services.Contracts;

public interface ICourseTopicsPdfExportService : IScopedService
{
    public Task CreateMergedPdfOfCoursesAsync(CancellationToken cancellationToken);

    public Task<IList<ExportDocument>> MapCourseTopicsToExportDocumentsAsync(params IList<Guid>? postIds);

    public Task<IList<ExportDocument>> MapCourseTopicsToExportDocumentsAsync(params IList<int>? postIds);

    public ExportDocument? MapCourseTopicToExportDocument(CourseTopic? post, string siteRootUri);

    public Task<ExportDocument?> MapCoursePostToExportDocumentAsync(int postId, string siteRootUri);

    public Task ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(CancellationToken cancellationToken);

    public Task ExportCourseTopicsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? postIds);
}
