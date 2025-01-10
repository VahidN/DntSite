using DntSite.Web.Features.AppConfigs.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.ScheduledTasks;

public class DotNetVersionCheckJob(IAppConfigsEmailsService appConfigsEmailsService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : appConfigsEmailsService.SendNewDotNetVersionEmailToAdminsAsync(cancellationToken);
}
