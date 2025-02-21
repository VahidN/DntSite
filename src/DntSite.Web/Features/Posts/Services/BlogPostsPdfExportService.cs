using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.ModelsMappings;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;
using EFCoreSecondLevelCacheInterceptor;

namespace DntSite.Web.Features.Posts.Services;

public class BlogPostsPdfExportService(
    IUnitOfWork uow,
    IAppSettingsService appSettingsService,
    IPdfExportService pdfExportService) : IBlogPostsPdfExportService
{
    private readonly DbSet<BlogPost> _blogPosts = uow.DbSet<BlogPost>();
    private readonly DbSet<BlogPostTag> _blogPostsTags = uow.DbSet<BlogPostTag>();

    public async Task ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(CancellationToken cancellationToken)
    {
        var availableIds = pdfExportService.GetAvailableExportedFiles(WhatsNewItemType.Posts)
            .Select(x => x.Id)
            .ToList();

        var query = _blogPosts.NotCacheable()
            .AsNoTracking()
            .Where(x => !x.IsDeleted && (!x.NumberOfRequiredPoints.HasValue || x.NumberOfRequiredPoints.Value == 0));

        var idsNeedUpdate = availableIds.Count == 0
            ? await query.Select(x => x.Id).ToListAsync(cancellationToken)
            : await query.Where(x => !availableIds.Contains(x.Id)).Select(x => x.Id).ToListAsync(cancellationToken);

        await ExportBlogPostsToSeparatePdfFilesAsync(cancellationToken, idsNeedUpdate);
    }

    public async Task ExportBlogPostsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? blogPostIds)
    {
        if (blogPostIds is null || blogPostIds.Count == 0)
        {
            return;
        }

        var docs = await MapBlogPostsToExportDocumentsAsync(blogPostIds);

        foreach (var doc in docs)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.Posts, doc.Id, doc.Title, doc);
            await Task.Delay(TimeSpan.FromSeconds(value: 15), cancellationToken);
        }
    }

    public async Task<IList<ExportDocument>> MapBlogPostsToExportDocumentsAsync(params IList<int>? blogPostIds)
    {
        if (blogPostIds is null || blogPostIds.Count == 0)
        {
            return [];
        }

        var siteRootUri = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        var posts = await _blogPosts.NotCacheable()
            .AsNoTracking()
            .Include(blogPost => blogPost.Comments)
            .ThenInclude(blogPostComment => blogPostComment.User)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Where(blogPost => !blogPost.IsDeleted && blogPostIds.Contains(blogPost.Id))
            .OrderBy(blogPost => blogPost.Id)
            .ToListAsync();

        return posts.Select(x => MapBlogPostToExportDocument(x, siteRootUri)!).ToList();
    }

    public async Task<ExportDocument?> MapBlogPostToExportDocumentAsync(int blogPostId, string siteRootUri)
    {
        var post = await _blogPosts.NotCacheable()
            .AsNoTracking()
            .Include(blogPost => blogPost.Comments)
            .ThenInclude(blogPostComment => blogPostComment.User)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Where(blogPost => !blogPost.IsDeleted)
            .OrderBy(blogPost => blogPost.Id)
            .FirstOrDefaultAsync(blogPost => blogPost.Id == blogPostId);

        return MapBlogPostToExportDocument(post, siteRootUri);
    }

    public ExportDocument? MapBlogPostToExportDocument([NotNullIfNotNull(nameof(post))] BlogPost? post,
        string siteRootUri)
        => post is null
            ? null
            : new ExportDocument
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                PersianDate = post.Audit.CreatedAtPersian,
                Author = post.User?.FriendlyName ?? "مهمان",
                Url = siteRootUri.CombineUrl(
                    string.Format(CultureInfo.InvariantCulture, PostsMappersExtensions.ParsedPostUrlTemplate, post.Id),
                    escapeRelativeUrl: false),
                Tags = post.Tags.Select(y => y.Name).ToList(),
                Comments = MapCommentsToExportComment(post)
            };

    public async Task CreateMergedPdfOfPostsTagsAsync(CancellationToken cancellationToken)
    {
        var tags = await _blogPostsTags.NotCacheable().AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

        foreach (var tag in tags)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var tagPostsIds = await (from blogPost in _blogPosts.NotCacheable().AsNoTracking()
                from blogPostTag in blogPost.Tags
                where blogPostTag.Id == tag.Id
                select blogPost.Id).ToListAsync(cancellationToken);

            if (await ShouldNotMergeItemsAsync(tag, tagPostsIds))
            {
                continue;
            }

            var blogPostDocs = await MapBlogPostsToExportDocumentsAsync(tagPostsIds);

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.Tag, tag.Id, $"مطالب گروه {tag.Name}",
                blogPostDocs);

            await Task.Delay(TimeSpan.FromSeconds(seconds: 15), cancellationToken);
        }
    }

    private async Task<bool> ShouldNotMergeItemsAsync(BlogPostTag tag, List<int> tagPostsIds)
        => (await pdfExportService.GetExportFileLocationAsync(WhatsNewItemType.Tag, tag.Id))?.IsReady == true &&
           !pdfExportService.HasChangedItem(WhatsNewItemType.Posts, tagPostsIds);

    private static List<ExportComment> MapCommentsToExportComment(BlogPost post)
    {
        var results = new List<ExportComment>();
        var comments = post.Comments.Where(comment => !comment.IsDeleted).ToList();

        results.AddRange(comments.Where(comment => !string.IsNullOrWhiteSpace(comment.Body))
            .Select(comment => new ExportComment
            {
                Id = comment.Id,
                ParentItemId = comment.ReplyId,
                Body = comment.Body,
                PersianDate = comment.Audit.CreatedAtPersian,
                Author = comment.User?.FriendlyName ?? "مهمان"
            }));

        return results;
    }
}
