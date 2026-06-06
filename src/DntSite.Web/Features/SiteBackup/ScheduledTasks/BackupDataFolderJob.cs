using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.SiteBackup.Services.Contracts;

namespace DntSite.Web.Features.SiteBackup.ScheduledTasks;

public class BackupDataFolderJob(
    IWebSiteBackupService webSiteBackupService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => webSiteBackupService.CompressAndUploadDataFolderBackupFileAsync(cancellationToken);
}
