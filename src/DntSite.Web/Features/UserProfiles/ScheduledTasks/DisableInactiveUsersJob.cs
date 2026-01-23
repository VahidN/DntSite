using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class DisableInactiveUsersJob(
    IUserProfilesManagerService userProfilesManagerService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    private const int DefaultMinMonthToStayActive = 12;

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
       if(appSetting is null)
       {
          return;
       }

		
        var minMonthToStayActive = GetMinMonthToStayActive(appSetting);
        await userProfilesManagerService.DisableInactiveUsersAsync(minMonthToStayActive);

        await NotifyInactiveUsersOnFridaysAsync(minMonthToStayActive, cancellationToken);
    }

    private async Task NotifyInactiveUsersOnFridaysAsync(int minMonthToStayActive, CancellationToken cancellationToken)
    {
        if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Friday)
        {
            await userProfilesManagerService.NotifyInactiveUsersAsync(minMonthToStayActive - 1, cancellationToken);
        }
    }

    private static int GetMinMonthToStayActive(AppSetting appSetting)
    {
        var minMonthToStayActive = appSetting.MinimumRequiredPosts.MinMonthToStayActive;

        return minMonthToStayActive == 0 ? DefaultMinMonthToStayActive : minMonthToStayActive;
    }
}
