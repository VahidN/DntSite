using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;

namespace DntSite.Web.Features.Common.ScheduledTasks;

public abstract class AppSettingAwareScheduledTaskBase(ICachedAppSettingsProvider appSettingsProvider) : IScheduledTask
{
    protected abstract bool ShouldNotBeExecutedIfSiteIsNotActive { set; get; }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var settings = await appSettingsProvider.GetAppSettingsAsync();

        if (!settings.SiteIsActive && ShouldNotBeExecutedIfSiteIsNotActive)
        {
            return;
        }

        await ExecuteAsync(settings, cancellationToken);
    }

    protected abstract Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken);
}
