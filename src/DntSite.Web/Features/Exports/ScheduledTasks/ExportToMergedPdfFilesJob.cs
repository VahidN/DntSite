using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Services.Contracts;

namespace DntSite.Web.Features.Exports.ScheduledTasks;

public class ExportToMergedPdfFilesJob(
    ILearningPathPdfExportsService learningPathPdfExportsService,
    ICourseTopicsPdfExportService courseTopicsPdfExportService) : IScheduledTask
{
    public async Task RunAsync()
    {
        if (IsShuttingDown)
        {
            return;
        }

        await learningPathPdfExportsService.CreateMergedPdfOfLearningPathsAsync();
        await courseTopicsPdfExportService.CreateMergedPdfOfCoursesAsync();
    }

    public bool IsShuttingDown { get; set; }
}
