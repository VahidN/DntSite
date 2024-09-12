using DntSite.Web.Features.Backlogs.Services.Contracts;

namespace DntSite.Web.Features.Backlogs.ScheduledTasks;

public class ManageBacklogsJob(IBacklogsService backlogsService) : IScheduledTask
{
    public Task RunAsync() => IsShuttingDown ? Task.CompletedTask : backlogsService.CancelOldOnesAsync();

    public bool IsShuttingDown { get; set; }
}
