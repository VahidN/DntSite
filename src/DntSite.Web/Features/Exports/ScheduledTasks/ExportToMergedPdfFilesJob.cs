using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToMergedPdfFilesJob(
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ILearningPathPdfExportsService learningPathPdfExportsService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IDailyNewsPdfExportService dailyNewsPdfExportService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; } = true;

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        await learningPathPdfExportsService.CreateMergedPdfOfLearningPathsAsync(ExportType.PdfFile, cancellationToken);
        await courseTopicsPdfExportService.CreateMergedPdfOfCoursesAsync(ExportType.PdfFile, cancellationToken);
        await blogPostsPdfExportService.CreateMergedPdfOfPostsTagsAsync(ExportType.PdfFile, cancellationToken);
        await dailyNewsPdfExportService.CreateMergedPdfOfNewsTagsAsync(ExportType.PdfFile, cancellationToken);
    }
}
