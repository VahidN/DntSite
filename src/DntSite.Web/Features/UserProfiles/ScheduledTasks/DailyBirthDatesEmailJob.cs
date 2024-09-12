using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class DailyBirthDatesEmailJob(IJobsEmailsService jobsEmailsService) : IScheduledTask
{
    public Task RunAsync() => IsShuttingDown ? Task.CompletedTask : jobsEmailsService.SendDailyBirthDatesEmailAsync();

    public bool IsShuttingDown { get; set; }
}
