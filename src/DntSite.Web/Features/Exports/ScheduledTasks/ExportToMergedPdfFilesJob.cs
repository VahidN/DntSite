using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToMergedPdfFilesJob(
    IBlogPostsPdfExportService blogPostsPdfExportService,
    ILearningPathPdfExportsService learningPathPdfExportsService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService) : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await learningPathPdfExportsService.CreateMergedPdfOfLearningPathsAsync(cancellationToken);
        await courseTopicsPdfExportService.CreateMergedPdfOfCoursesAsync(cancellationToken);
        await blogPostsPdfExportService.CreateMergedPdfOfPostsTagsAsync(cancellationToken);
    }
}
