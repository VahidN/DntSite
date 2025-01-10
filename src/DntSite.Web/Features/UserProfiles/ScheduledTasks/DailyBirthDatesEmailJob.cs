using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class DailyBirthDatesEmailJob(IJobsEmailsService jobsEmailsService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : jobsEmailsService.SendDailyBirthDatesEmailAsync(cancellationToken);
}
