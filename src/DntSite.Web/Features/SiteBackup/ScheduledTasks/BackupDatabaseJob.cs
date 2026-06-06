using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;

namespace DntSite.Web.Features.SiteBackup.ScheduledTasks;

public class BackupDatabaseJob(
    IWebSiteBackupService webSiteBackupService,
    IEPubExportService ePubExportService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        await webSiteBackupService.CreateDatabaseBackupAsync(cancellationToken);
        await ePubExportService.StartAsync(cancellationToken);
    }
}
