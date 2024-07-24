using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.ScheduledTasks.Services;

public class EmptyPMsJob(IPrivateMessagesService privateMessagesService) : IScheduledTask
{
    public Task RunAsync() => IsShuttingDown ? Task.CompletedTask : privateMessagesService.DeleteAllAsync();

    public bool IsShuttingDown { get; set; }
}
