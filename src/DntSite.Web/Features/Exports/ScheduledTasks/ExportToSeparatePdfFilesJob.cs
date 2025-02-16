using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToSeparatePdfFilesJob(
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IQuestionsPdfExportService questionsPdfExportService,
    IDailyNewsPdfExportService dailyNewsPdfExportService) : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await questionsPdfExportService.ExportNotProcessedQuestionsToSeparatePdfFilesAsync(cancellationToken);
        await courseTopicsPdfExportService.ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(cancellationToken);
        await blogPostsPdfExportService.ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(cancellationToken);
        await dailyNewsPdfExportService.ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(cancellationToken);
    }
}
