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

            await pdfExportService.CreateSinglePdfFileAsync(WhatsNewItemType.LearningPaths, item.Id, item.Title,
                [..blogPostDocs, ..courseTopicDocs, ..questionIds]);

            await Task.Delay(TimeSpan.FromSeconds(seconds: 7));
        }
    }

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
                    ConvertToListOfGuids(GetItemPostIds(contains: "/courses/topic/", links, segmentNumber: 4)),
                PostIds = ConvertToListOfInts(GetItemPostIds(contains: "/post/", links, segmentNumber: 2)),
                QuestionIds = ConvertToListOfInts(GetItemPostIds(contains: "/questions/details/", links,
                    segmentNumber: 3))
            });
        }

        return results;
    }

    private List<string> GetLinks(LearningPath item, string siteRootUri)
        => htmlHelperService.ExtractLinks(item.Description)
            .Where(link => link.IsValidUrl() && link.HaveTheSameDomain(siteRootUri))
            .ToList();

    private static List<int> ConvertToListOfInts(List<string> items)
        => items.Select(strId => strId.ToInt()).Where(id => id > 0).ToList();

    private static List<Guid> ConvertToListOfGuids(List<string> items)
    {
        var results = new List<Guid>();

        foreach (var item in items)
        {
            if (Guid.TryParse(item, out var guid))
            {
                results.Add(guid);
            }
        }

        return results;
    }

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
