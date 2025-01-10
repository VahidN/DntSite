using DntSite.Web.Features.Backlogs.Services.Contracts;

namespace DntSite.Web.Features.Backlogs.ScheduledTasks;

public class ManageBacklogsJob(IBacklogsService backlogsService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : backlogsService.CancelOldOnesAsync(cancellationToken);
}
