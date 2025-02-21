using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;
using EFCoreSecondLevelCacheInterceptor;

namespace DntSite.Web.Features.StackExchangeQuestions.Services;

public class QuestionsPdfExportService(
    IUnitOfWork uow,
    IAppSettingsService appSettingsService,
    IPdfExportService pdfExportService) : IQuestionsPdfExportService
{
    private readonly DbSet<StackExchangeQuestion> _questions = uow.DbSet<StackExchangeQuestion>();

    public async Task ExportNotProcessedQuestionsToSeparatePdfFilesAsync(CancellationToken cancellationToken)
    {
        var availableIds = pdfExportService.GetAvailableExportedFiles(WhatsNewItemType.Questions)
            .Select(x => x.Id)
            .ToList();

        var query = _questions.NotCacheable().AsNoTracking().Where(x => !x.IsDeleted);

        var idsNeedUpdate = availableIds.Count == 0
            ? await query.Select(x => x.Id).ToListAsync(cancellationToken)
            : await query.Where(x => !availableIds.Contains(x.Id)).Select(x => x.Id).ToListAsync(cancellationToken);

        await ExportQuestionsToSeparatePdfFilesAsync(cancellationToken, idsNeedUpdate);
    }

    public async Task<IList<ExportDocument>> MapQuestionsToExportDocumentsAsync(params IList<int>? postIds)
    {
        if (postIds is null || postIds.Count == 0)
        {
            return [];
        }

        var siteRootUri = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        var posts = await _questions.NotCacheable()
            .AsNoTracking()
            .Include(question => question.Comments)
            .ThenInclude(postComment => postComment.User)
            .Include(question => question.User)
            .Include(question => question.Tags)
            .Where(question => !question.IsDeleted && postIds.Contains(question.Id))
            .OrderBy(question => question.Id)
            .ToListAsync();

        return posts.Select(x => MapQuestionToExportDocument(x, siteRootUri)!).ToList();
    }

    public async Task ExportQuestionsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? postIds)
    {
        if (postIds is null || postIds.Count == 0)
        {
            return;
        }

        var docs = await MapQuestionsToExportDocumentsAsync(postIds);

        foreach (var doc in docs)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.Questions, doc.Id, doc.Title, doc);
            await Task.Delay(TimeSpan.FromSeconds(value: 15), cancellationToken);
        }
    }

    public async Task<ExportDocument?> MapQuestionToExportDocumentAsync(int postId, string siteRootUri)
    {
        var post = await _questions.NotCacheable()
            .AsNoTracking()
            .Include(question => question.Comments)
            .ThenInclude(postComment => postComment.User)
            .Include(question => question.User)
            .Include(question => question.Tags)
            .Where(question => !question.IsDeleted)
            .OrderBy(question => question.Id)
            .FirstOrDefaultAsync(question => question.Id == postId);

        return MapQuestionToExportDocument(post, siteRootUri);
    }

    public ExportDocument? MapQuestionToExportDocument(StackExchangeQuestion? post, string siteRootUri)
        => post is null
            ? null
            : new ExportDocument
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Description,
                PersianDate = post.Audit.CreatedAtPersian,
                Author = post.User?.FriendlyName ?? "مهمان",
                Url = siteRootUri.CombineUrl(
                    string.Format(CultureInfo.InvariantCulture, QuestionsMappersExtensions.ParsedPostUrlTemplate,
                        post.Id), escapeRelativeUrl: false),
                Tags = post.Tags.Select(y => y.Name).ToList(),
                Comments = MapCommentsToExportComment(post)
            };

    private static List<ExportComment> MapCommentsToExportComment(StackExchangeQuestion post)
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
