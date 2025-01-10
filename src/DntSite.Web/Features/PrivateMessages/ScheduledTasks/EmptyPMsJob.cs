using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.PrivateMessages.ScheduledTasks;

public class EmptyPMsJob(IPrivateMessagesService privateMessagesService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : privateMessagesService.DeleteAllAsync(cancellationToken);
}
