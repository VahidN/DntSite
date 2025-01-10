using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class NewPersianYearEmailsJob(IJobsEmailsService jobsEmailsService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : jobsEmailsService.SendNewPersianYearEmailsAsync(cancellationToken);
}
