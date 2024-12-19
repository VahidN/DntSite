using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToSeparatePdfFilesJob(
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService,
    IQuestionsPdfExportService questionsPdfExportService) : IScheduledTask
{
    public async Task RunAsync()
    {
        if (IsShuttingDown)
        {
            return;
        }

        await questionsPdfExportService.ExportNotProcessedQuestionsToSeparatePdfFilesAsync();
        await courseTopicsPdfExportService.ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync();
        await blogPostsPdfExportService.ExportNotProcessedBlogPostsToSeparatePdfFilesAsync();
    }

    public bool IsShuttingDown { get; set; }
}
