using DntSite.Web.Features.AppConfigs.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.ScheduledTasks;

public class FreeSpaceCheckJob(IAppConfigsEmailsService appConfigsEmailsService) : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await appConfigsEmailsService.SendHasNotRemainingSpaceEmailToAdminsAsync(cancellationToken);
    }
}
