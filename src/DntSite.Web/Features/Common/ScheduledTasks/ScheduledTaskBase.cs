using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;

namespace DntSite.Web.Features.Common.ScheduledTasks;

public abstract class ScheduledTaskBase(ICachedAppSettingsProvider appSettingsProvider) : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var settings = await appSettingsProvider.GetAppSettingsAsync();

        if (!settings.SiteIsActive)
        {
            return;
        }

        await ExecuteAsync(settings, cancellationToken);
    }

    protected abstract Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken);
}
