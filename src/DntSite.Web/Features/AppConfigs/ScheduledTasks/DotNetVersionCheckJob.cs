using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;

namespace DntSite.Web.Features.AppConfigs.ScheduledTasks;

public class DotNetVersionCheckJob(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppConfigsEmailsService appConfigsEmailsService) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : appConfigsEmailsService.SendNewDotNetVersionEmailToAdminsAsync(cancellationToken);
}
