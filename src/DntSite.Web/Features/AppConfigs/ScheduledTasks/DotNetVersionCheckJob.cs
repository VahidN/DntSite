using DntSite.Web.Features.AppConfigs.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.ScheduledTasks;

public class DotNetVersionCheckJob(IAppConfigsEmailsService appConfigsEmailsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown ? Task.CompletedTask : appConfigsEmailsService.SendNewDotNetVersionEmailToAdminsAsync();

    public bool IsShuttingDown { get; set; }
}
