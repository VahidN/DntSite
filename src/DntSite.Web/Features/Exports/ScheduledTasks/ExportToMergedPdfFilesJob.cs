using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToMergedPdfFilesJob(
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ILearningPathPdfExportsService learningPathPdfExportsService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IDailyNewsPdfExportService dailyNewsPdfExportService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        await learningPathPdfExportsService.CreateMergedPdfOfLearningPathsAsync(cancellationToken);
        await courseTopicsPdfExportService.CreateMergedPdfOfCoursesAsync(cancellationToken);
        await blogPostsPdfExportService.CreateMergedPdfOfPostsTagsAsync(cancellationToken);
        await dailyNewsPdfExportService.CreateMergedPdfOfNewsTagsAsync(cancellationToken);
    }
}
