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
    private const int DeactivateSiteAfterDaysOfInactivity = 4;

    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(appSetting);
		
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var days = appSetting.DeactivateSiteAfterDaysOfInactivity;

        if (days <= 0)
        {
            days = DeactivateSiteAfterDaysOfInactivity;
        }

        var limit = DateTime.UtcNow.AddDays(-days);
        var siteIsActive = await usersInfoService.IsAnyActiveAdminPresentAsync(limit);
        await appSettingsService.ChangeSiteActiveStateAsync(siteIsActive);
    }
}
