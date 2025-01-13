using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Courses.Services;

public class CourseTopicsPdfExportService(
    IUnitOfWork uow,
    IAppSettingsService appSettingsService,
    IPdfExportService pdfExportService) : ICourseTopicsPdfExportService
{
    private readonly DbSet<Course> _courses = uow.DbSet<Course>();
    private readonly DbSet<CourseTopic> _courseTopics = uow.DbSet<CourseTopic>();
    private readonly WhatsNewItemType _itemType = WhatsNewItemType.AllCoursesTopics;

    public async Task CreateMergedPdfOfCoursesAsync(CancellationToken cancellationToken)
    {
        var courses = await _courses.AsNoTracking()
            .Include(x => x.CourseTopics)
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var course in courses)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var topicIds = course.CourseTopics.Where(x => !x.IsDeleted).Select(x => x.Id).ToList();

            if (await ShouldNotMergeItemsAsync(course, topicIds))
            {
                continue;
            }

            var docs = await MapCourseTopicsToExportDocumentsAsync(topicIds);

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.AllCourses, course.Id, course.Title, docs);

            await Task.Delay(TimeSpan.FromSeconds(seconds: 60), cancellationToken);
        }
    }

    public async Task ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(CancellationToken cancellationToken)
    {
        var availableIds = pdfExportService.GetAvailableExportedFiles(_itemType).Select(x => x.Id).ToList();

        var query = _courseTopics.AsNoTracking().Where(x => !x.IsDeleted);

        var idsNeedUpdate = availableIds.Count == 0
            ? await query.Select(x => x.Id).ToListAsync(cancellationToken)
            : await query.Where(x => !availableIds.Contains(x.Id)).Select(x => x.Id).ToListAsync(cancellationToken);

        await ExportCourseTopicsToSeparatePdfFilesAsync(cancellationToken, idsNeedUpdate);
    }

    public async Task<IList<ExportDocument>> MapCourseTopicsToExportDocumentsAsync(params IList<int>? postIds)
    {
        if (postIds is null || postIds.Count == 0)
        {
            return [];
        }

        var siteRootUri = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        var posts = await _courseTopics.AsNoTracking()
            .Include(topic => topic.Comments)
            .ThenInclude(topicComment => topicComment.User)
            .Include(topic => topic.User)
            .Include(topic => topic.Course)
            .ThenInclude(course => course.Tags)
            .Where(topic => !topic.IsDeleted && postIds.Contains(topic.Id))
            .OrderBy(topic => topic.Id)
            .ToListAsync();

        return posts.Select(x => MapCourseTopicToExportDocument(x, siteRootUri)!).ToList();
    }

    public async Task<IList<ExportDocument>> MapCourseTopicsToExportDocumentsAsync(params IList<Guid>? postIds)
    {
        if (postIds is null || postIds.Count == 0)
        {
            return [];
        }

        var siteRootUri = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        var posts = await _courseTopics.AsNoTracking()
            .Include(topic => topic.Comments)
            .ThenInclude(topicComment => topicComment.User)
            .Include(topic => topic.User)
            .Include(topic => topic.Course)
            .ThenInclude(course => course.Tags)
            .Where(topic => !topic.IsDeleted && postIds.Contains(topic.DisplayId))
            .OrderBy(topic => topic.Id)
            .ToListAsync();

        return posts.Select(x => MapCourseTopicToExportDocument(x, siteRootUri)!).ToList();
    }

    public async Task ExportCourseTopicsToSeparatePdfFilesAsync(CancellationToken cancellationToken,
        params IList<int>? postIds)
    {
        if (postIds is null || postIds.Count == 0)
        {
            return;
        }

        var docs = await MapCourseTopicsToExportDocumentsAsync(postIds);

        foreach (var doc in docs)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await pdfExportService.CreateSinglePdfFileAsync(_itemType, doc.Id, doc.Title, doc);
            await Task.Delay(TimeSpan.FromSeconds(value: 60), cancellationToken);
        }
    }

    public async Task<ExportDocument?> MapCoursePostToExportDocumentAsync(int postId, string siteRootUri)
    {
        var post = await _courseTopics.AsNoTracking()
            .Include(topic => topic.Comments)
            .ThenInclude(topicComment => topicComment.User)
            .Include(topic => topic.User)
            .Include(topic => topic.Course)
            .ThenInclude(course => course.Tags)
            .Where(topic => !topic.IsDeleted)
            .OrderBy(topic => topic.Id)
            .FirstOrDefaultAsync(topic => topic.Id == postId);

        return MapCourseTopicToExportDocument(post, siteRootUri);
    }

    public ExportDocument? MapCourseTopicToExportDocument(CourseTopic? post, string siteRootUri)
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
                    string.Create(CultureInfo.InvariantCulture,
                        $"{CoursesRoutingConstants.CoursesTopicBase}/{post.CourseId}/{post.DisplayId:D}"),
                    escapeRelativeUrl: false),
                Tags = post.Course.Tags.Select(y => y.Name).ToList(),
                Comments = MapCommentsToExportComment(post)
            };

    private async Task<bool> ShouldNotMergeItemsAsync(Course course, List<int> topicIds)
        => (await pdfExportService.GetExportFileLocationAsync(WhatsNewItemType.AllCourses, course.Id))?.IsReady ==
            true && !pdfExportService.HasChangedItem(WhatsNewItemType.AllCoursesTopics, topicIds);

    private static List<ExportComment> MapCommentsToExportComment(CourseTopic post)
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
