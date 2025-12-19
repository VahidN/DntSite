using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostsPdfExportService : IScopedService
{
    Task CreateMergedPdfOfPostsTagsAsync(CancellationToken cancellationToken);

    Task ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(CancellationToken cancellationToken);

    ExportDocument? MapBlogPostToExportDocument(BlogPost? post, string siteRootUri);

    Task<ExportDocument?> MapBlogPostToExportDocumentAsync(int blogPostId, string siteRootUri);

    Task<IList<ExportDocument>> MapBlogPostsToExportDocumentsAsync(params IList<int>? blogPostIds);

    Task ExportBlogPostsToSeparatePdfFilesAsync(CancellationToken cancellationToken, params IList<int>? blogPostIds);
}
