using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToSeparatePdfFilesJob(
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IQuestionsPdfExportService questionsPdfExportService,
    IDailyNewsPdfExportService dailyNewsPdfExportService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; } = true;

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        await questionsPdfExportService.ExportNotProcessedQuestionsToSeparatePdfFilesAsync(ExportType.PdfFile,
            cancellationToken);

        await courseTopicsPdfExportService.ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(ExportType.PdfFile,
            cancellationToken);

        await blogPostsPdfExportService.ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(ExportType.PdfFile,
            cancellationToken);

        await dailyNewsPdfExportService.ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(ExportType.PdfFile,
            cancellationToken);
    }
}
