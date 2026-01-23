using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToSeparatePdfFilesJob(
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IQuestionsPdfExportService questionsPdfExportService,
    IDailyNewsPdfExportService dailyNewsPdfExportService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        await questionsPdfExportService.ExportNotProcessedQuestionsToSeparatePdfFilesAsync(cancellationToken);
        await courseTopicsPdfExportService.ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(cancellationToken);
        await blogPostsPdfExportService.ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(cancellationToken);
        await dailyNewsPdfExportService.ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(cancellationToken);
    }
}
