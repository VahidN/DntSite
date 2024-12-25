using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.RoadMaps.Services;

public class LearningPathPdfExportsService(
    IUnitOfWork uow,
    IHtmlHelperService htmlHelperService,
    IAppSettingsService appSettingsService,
    IPdfExportService pdfExportService,
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IQuestionsPdfExportService questionsPdfExportService) : ILearningPathPdfExportsService
{
    private readonly DbSet<LearningPath> _learningPaths = uow.DbSet<LearningPath>();

    public async Task CreateMergedPdfOfLearningPathsAsync()
    {
        var items = await GetLinkIdsAsync();

        foreach (var item in items)
        {
            var blogPostDocs = await blogPostsPdfExportService.MapBlogPostsToExportDocumentsAsync(item.PostIds);

            var courseTopicDocs =
                await courseTopicsPdfExportService.MapCourseTopicsToExportDocumentsAsync(item.CourseTopicIds);

            var questionIds = await questionsPdfExportService.MapQuestionsToExportDocumentsAsync(item.QuestionIds);

            if (!HasChangedItem(item.PostIds, courseTopicDocs.Select(x => x.Id).ToList(), item.QuestionIds))
            {
                continue;
            }

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.LearningPaths, item.Id, item.Title,
                [..blogPostDocs, ..courseTopicDocs, ..questionIds]);

            await Task.Delay(TimeSpan.FromSeconds(seconds: 7));
        }
    }

    private bool HasChangedItem(IList<int> postIds, IList<int> questionIds, IList<int> courseTopicIds)
        => pdfExportService.HasChangedItem(WhatsNewItemType.Posts, postIds) ||
           pdfExportService.HasChangedItem(WhatsNewItemType.AllCoursesTopics, courseTopicIds) ||
           pdfExportService.HasChangedItem(WhatsNewItemType.Questions, questionIds);

    private async Task<List<LearningPathLinksModel>> GetLinkIdsAsync()
    {
        var results = new List<LearningPathLinksModel>();

        var siteRootUri = (await appSettingsService.GetAppSettingModelAsync()).SiteRootUri;

        var items = await _learningPaths.Include(x => x.Tags).Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();

        foreach (var item in items)
        {
            var links = GetLinks(item, siteRootUri);

            if (links.Count == 0)
            {
                continue;
            }

            results.Add(new LearningPathLinksModel
            {
                Id = item.Id,
                Title = item.Title,
                Tags = item.Tags.Where(x => !x.IsDeleted).Select(x => x.Name).ToList(),
                CourseTopicIds =
                    GetItemPostIds(contains: "/courses/topic/", links, segmentNumber: 4)
                        .TryConvertToListOfT<Guid>(ignoreParsingFailures: true),
                PostIds =
                    GetItemPostIds(contains: "/post/", links, segmentNumber: 2)
                        .TryConvertToListOfT<int>(ignoreParsingFailures: true),
                QuestionIds = GetItemPostIds(contains: "/questions/details/", links, segmentNumber: 3)
                    .TryConvertToListOfT<int>(ignoreParsingFailures: true)
            });
        }

        return results;
    }

    private List<string> GetLinks(LearningPath item, string siteRootUri)
        => htmlHelperService.ExtractLinks(item.Description)
            .Where(link => link.IsValidUrl() && link.HaveTheSameDomain(siteRootUri))
            .ToList();

    private List<string> GetItemPostIds(string contains, List<string> links, int segmentNumber)
    {
        var postIds = new List<string>();

        foreach (var link in links.Where(x => x.Contains(contains, StringComparison.OrdinalIgnoreCase)))
        {
            var uri = new Uri(link);

            if (uri.Segments.Length <= segmentNumber)
            {
                continue;
            }

            var id = uri.Segments[segmentNumber]
                .Replace(oldValue: "/", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Trim();

            postIds.Add(id);
        }

        return postIds;
    }
}
