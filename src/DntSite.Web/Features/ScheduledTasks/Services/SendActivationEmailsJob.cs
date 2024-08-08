using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.ScheduledTasks.Services;

public class SendActivationEmailsJob(IUsersManagerEmailsService emailsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown ? Task.CompletedTask : emailsService.ResetNotActivatedUsersAndSendEmailAsync(from: null);

    public bool IsShuttingDown { get; set; }
}
