using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostsPdfExportService : IScopedService
{
    Task<List<int>> FindIdsNeedUpdateAsync(CancellationToken cancellationToken);

    Task CreateMergedPdfOfPostsTagsAsync(ExportType exportType, CancellationToken cancellationToken);

    Task ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(ExportType exportType,
        CancellationToken cancellationToken = default);

    ExportDocument? MapBlogPostToExportDocument(BlogPost? post, string siteRootUri);

    Task<ExportDocument?> MapBlogPostToExportDocumentAsync(int blogPostId, string siteRootUri);

    Task<IList<ExportDocument>> MapBlogPostsToExportDocumentsAsync(params IList<int>? blogPostIds);

    Task ExportBlogPostsToSeparatePdfFilesAsync(ExportType exportType,
        CancellationToken cancellationToken,
        params IList<int>? blogPostIds);
}
