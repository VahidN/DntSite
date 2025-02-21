using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.ModelsMappings;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.RssFeeds.Models;
using EFCoreSecondLevelCacheInterceptor;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsPdfExportService(
    IUnitOfWork uow,
    IAppSettingsService appSettingsService,
    IPdfExportService pdfExportService,
    IDailyNewsScreenshotsService dailyNewsScreenshotsService) : IDailyNewsPdfExportService
{
    private readonly DbSet<DailyNewsItem> _dailyNews = uow.DbSet<DailyNewsItem>();
    private readonly DbSet<DailyNewsItemTag> _dailyNewsTags = uow.DbSet<DailyNewsItemTag>();

    public async Task ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(CancellationToken cancellationToken)
    {
        var availableIds = pdfExportService.GetAvailableExportedFiles(WhatsNewItemType.News).Select(x => x.Id).ToList();

        var query = _dailyNews.NotCacheable().AsNoTracking().Where(x => !x.IsDeleted);

        var idsNeedUpdate = availableIds.Count == 0
            ? await query.Select(x => x.Id).ToListAsync(cancellationToken)
            : await query.Where(x => !availableIds.Contains(x.Id)).Select(x => x.Id).ToListAsync(cancellationToken);

        await ExportDailyNewsToSeparatePdfFilesAsync(cancellationToken, idsNeedUpdate);
    }

    public async Task ExportDailyNewsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? dailyNewsItemIds)
    {
        if (dailyNewsItemIds is null || dailyNewsItemIds.Count == 0)
        {
            return;
        }

        var docs = await MapDailyNewsToExportDocumentsAsync(dailyNewsItemIds);

        foreach (var doc in docs)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.News, doc.Id, doc.Title, doc);
            await Task.Delay(TimeSpan.FromSeconds(value: 15), cancellationToken);
        }
    }

    public async Task<IList<ExportDocument>> MapDailyNewsToExportDocumentsAsync(params IList<int>? dailyNewsItemIds)
    {
        if (dailyNewsItemIds is null || dailyNewsItemIds.Count == 0)
        {
            return [];
        }

        var siteRootUri = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        var posts = await _dailyNews.NotCacheable()
            .AsNoTracking()
            .Include(dailyNewsItem => dailyNewsItem.Comments)
            .ThenInclude(dailyNewsItemComment => dailyNewsItemComment.User)
            .Include(dailyNewsItem => dailyNewsItem.User)
            .Include(dailyNewsItem => dailyNewsItem.Tags)
            .Where(dailyNewsItem => !dailyNewsItem.IsDeleted && dailyNewsItemIds.Contains(dailyNewsItem.Id))
            .OrderBy(dailyNewsItem => dailyNewsItem.Id)
            .ToListAsync();

        return posts.Select(x => MapDailyNewsItemToExportDocument(x, siteRootUri)!).ToList();
    }

    public async Task<ExportDocument?> MapDailyNewsItemToExportDocumentAsync(int dailyNewsItemId, string siteRootUri)
    {
        var post = await _dailyNews.NotCacheable()
            .AsNoTracking()
            .Include(dailyNewsItem => dailyNewsItem.Comments)
            .ThenInclude(dailyNewsItemComment => dailyNewsItemComment.User)
            .Include(dailyNewsItem => dailyNewsItem.User)
            .Include(dailyNewsItem => dailyNewsItem.Tags)
            .Where(dailyNewsItem => !dailyNewsItem.IsDeleted)
            .OrderBy(dailyNewsItem => dailyNewsItem.Id)
            .FirstOrDefaultAsync(dailyNewsItem => dailyNewsItem.Id == dailyNewsItemId);

        return MapDailyNewsItemToExportDocument(post, siteRootUri);
    }

    public ExportDocument? MapDailyNewsItemToExportDocument([NotNullIfNotNull(nameof(post))] DailyNewsItem? post,
        string siteRootUri)
        => post is null
            ? null
            : new ExportDocument
            {
                Id = post.Id,
                Title = post.Title,
                Body = GetPostDescription(post, siteRootUri),
                PersianDate = post.Audit.CreatedAtPersian,
                Author = post.User?.FriendlyName ?? "مهمان",
                Url = siteRootUri.CombineUrl(
                    string.Format(CultureInfo.InvariantCulture, NewsMappersExtensions.ParsedPostUrlTemplate, post.Id),
                    escapeRelativeUrl: false),
                Tags = post.Tags.Select(y => y.Name).ToList(),
                Comments = MapCommentsToExportComment(post)
            };

    public async Task CreateMergedPdfOfNewsTagsAsync(CancellationToken cancellationToken)
    {
        var tags = await _dailyNewsTags.NotCacheable().AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

        foreach (var tag in tags)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var tagPostsIds = await (from dailyNewsItem in _dailyNews.NotCacheable().AsNoTracking()
                from dailyNewsItemTag in dailyNewsItem.Tags
                where dailyNewsItemTag.Id == tag.Id
                select dailyNewsItem.Id).ToListAsync(cancellationToken);

            if (await ShouldNotMergeItemsAsync(tag, tagPostsIds))
            {
                continue;
            }

            var dailyNewsItemDocs = await MapDailyNewsToExportDocumentsAsync(tagPostsIds);

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.NewsTag, tag.Id, $"مطالب گروه {tag.Name}",
                dailyNewsItemDocs);

            await Task.Delay(TimeSpan.FromSeconds(seconds: 15), cancellationToken);
        }
    }

    private string GetPostDescription(DailyNewsItem post, string siteRootUri)
    {
        var body = post.BriefDescription ?? "";

        body = body.WrapInDirectionalDiv(fontFamily: "inherit", fontSize: "inherit");

        var image = dailyNewsScreenshotsService.GetNewsThumbImage(post, siteRootUri);

        if (!image.IsEmpty())
        {
            body += $"{image}";
        }

        body += $"<br><br><a href='{post.Url}'>مشاهده مطلب اصلی</a>";

        return body;
    }

    private async Task<bool> ShouldNotMergeItemsAsync(DailyNewsItemTag tag, List<int> tagPostsIds)
        => (await pdfExportService.GetExportFileLocationAsync(WhatsNewItemType.NewsTag, tag.Id))?.IsReady == true &&
           !pdfExportService.HasChangedItem(WhatsNewItemType.News, tagPostsIds);

    private static List<ExportComment> MapCommentsToExportComment(DailyNewsItem post)
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
