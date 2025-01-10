using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostsPdfExportService : IScopedService
{
    public Task CreateMergedPdfOfPostsTagsAsync(CancellationToken cancellationToken);

    public Task ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(CancellationToken cancellationToken);

    public ExportDocument? MapBlogPostToExportDocument(BlogPost? post, string siteRootUri);

    public Task<ExportDocument?> MapBlogPostToExportDocumentAsync(int blogPostId, string siteRootUri);

    public Task<IList<ExportDocument>> MapBlogPostsToExportDocumentsAsync(params IList<int>? blogPostIds);

    public Task ExportBlogPostsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? blogPostIds);
}
