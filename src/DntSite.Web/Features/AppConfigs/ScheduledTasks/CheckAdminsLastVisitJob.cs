using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.ScheduledTasks;

public class CheckAdminsLastVisitJob(
    IUsersInfoService usersInfoService,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IAppSettingsService appSettingsService) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var limit = DateTime.UtcNow.AddDays(value: -2);
        var siteIsActive = await usersInfoService.IsAnyActiveAdminPresentAsync(limit);
        await appSettingsService.ChangeSiteActiveStateAsync(siteIsActive);
    }
}
