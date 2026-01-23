using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;

namespace DntSite.Web.Features.AppConfigs.ScheduledTasks;

public class DotNetVersionCheckJob(
    IAppConfigsEmailsService appConfigsEmailsService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => appConfigsEmailsService.SendNewDotNetVersionEmailToAdminsAsync(cancellationToken);
}
