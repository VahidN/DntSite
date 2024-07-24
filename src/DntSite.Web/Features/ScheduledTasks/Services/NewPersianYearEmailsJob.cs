using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.ScheduledTasks.Services;

public class NewPersianYearEmailsJob(IJobsEmailsService jobsEmailsService) : IScheduledTask
{
    public Task RunAsync() => IsShuttingDown ? Task.CompletedTask : jobsEmailsService.SendNewPersianYearEmailsAsync();

    public bool IsShuttingDown { get; set; }
}
