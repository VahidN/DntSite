using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class SendActivationEmailsJob(IUsersManagerEmailsService emailsService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : emailsService.ResetNotActivatedUsersAndSendEmailAsync(from: null, cancellationToken);
}
