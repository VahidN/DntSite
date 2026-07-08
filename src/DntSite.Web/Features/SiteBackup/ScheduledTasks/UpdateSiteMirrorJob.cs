using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.SiteBackup.ScheduledTasks;

public class UpdateSiteMirrorJob(
    IQuestionsPdfExportService questionsPdfExportService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IBlogPostsPdfExportService blogPostsPdfExportService,
    IDailyNewsPdfExportService dailyNewsPdfExportService,
    IEPubExportService ePubExportService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        await questionsPdfExportService.ExportNotProcessedQuestionsToSeparatePdfFilesAsync(ExportType.HtmlFile,
            cancellationToken);

        await courseTopicsPdfExportService.ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(ExportType.HtmlFile,
            cancellationToken);

        await blogPostsPdfExportService.ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(ExportType.HtmlFile,
            cancellationToken);

        await dailyNewsPdfExportService.ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(ExportType.HtmlFile,
            cancellationToken);

        await ePubExportService.StartAsync(uploadFile: false, deleteFileAtEnd: true, cancellationToken);
    }
}
