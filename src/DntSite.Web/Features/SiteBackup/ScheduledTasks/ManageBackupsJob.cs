using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;

namespace DntSite.Web.Features.SiteBackup.ScheduledTasks;

public class ManageBackupsJob(
    IWebSiteBackupService webSiteBackupService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => webSiteBackupService.CreateSiteBackupAsync(cancellationToken);
}
