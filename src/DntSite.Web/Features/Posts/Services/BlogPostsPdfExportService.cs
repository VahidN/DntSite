using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.ModelsMappings;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Posts.Services;

public class BlogPostsPdfExportService(
    IUnitOfWork uow,
    IAppSettingsService appSettingsService,
    IPdfExportService pdfExportService) : IBlogPostsPdfExportService
{
    private readonly DbSet<BlogPost> _blogPosts = uow.DbSet<BlogPost>();
    private readonly DbSet<BlogPostTag> _blogPostsTags = uow.DbSet<BlogPostTag>();

    private readonly WhatsNewItemType _itemType = WhatsNewItemType.Posts;

    public async Task ExportNotProcessedBlogPostsToSeparatePdfFilesAsync()
    {
        var availableIds = pdfExportService.GetAvailableExportedFilesIds(_itemType);

        var query = _blogPosts.AsNoTracking()
            .Where(x => !x.IsDeleted && (!x.NumberOfRequiredPoints.HasValue || x.NumberOfRequiredPoints.Value == 0));

        var idsNeedUpdate = availableIds is null || availableIds.Count == 0
            ? await query.Select(x => x.Id).ToListAsync()
            : await query.Where(x => !availableIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();

        await ExportBlogPostsToSeparatePdfFilesAsync(idsNeedUpdate);
    }

    public async Task ExportBlogPostsToSeparatePdfFilesAsync(params IList<int>? blogPostIds)
    {
        if (blogPostIds is null || blogPostIds.Count == 0)
        {
            return;
        }

        var docs = await MapBlogPostsToExportDocumentsAsync(blogPostIds);

        foreach (var doc in docs)
        {
            await pdfExportService.CreateSinglePdfFileAsync(_itemType, doc.Id, doc.Title, doc);
            await Task.Delay(TimeSpan.FromSeconds(value: 7));
        }
    }

    public async Task<IList<ExportDocument>> MapBlogPostsToExportDocumentsAsync(params IList<int>? blogPostIds)
    {
        if (blogPostIds is null || blogPostIds.Count == 0)
        {
            return [];
        }

        var siteRootUri = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        var posts = await _blogPosts.AsNoTracking()
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
        var post = await _blogPosts.AsNoTracking()
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

    public async Task CreateMergedPdfOfPostsTagsAsync()
    {
        var tags = await _blogPostsTags.AsNoTracking().OrderBy(x => x.Id).ToListAsync();

        foreach (var tag in tags)
        {
            var tagPostsIds = await (from blogPost in _blogPosts.AsNoTracking()
                from blogPostTag in blogPost.Tags
                where blogPostTag.Id == tag.Id
                select blogPost.Id).ToListAsync();

            var blogPostDocs = await MapBlogPostsToExportDocumentsAsync(tagPostsIds);

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.Tag, tag.Id, $"مطالب گروه: {tag.Name}",
                blogPostDocs);

            await Task.Delay(TimeSpan.FromSeconds(seconds: 7));
        }
    }

    private static List<ExportComment> MapCommentsToExportComment(BlogPost post)
    {
        var results = new List<ExportComment>();
        var comments = post.Comments.Where(comment => !comment.IsDeleted).ToList();

        results.AddRange(comments.Where(comment => !string.IsNullOrWhiteSpace(comment.Body))
            .Select(comment => new ExportComment
            {
                Id = comment.Id,
                ReplyToId = comment.ReplyId,
                Body = comment.Body,
                PersianDate = comment.Audit.CreatedAtPersian,
                Author = comment.User?.FriendlyName ?? "مهمان"
            }));

        return results;
    }
}
