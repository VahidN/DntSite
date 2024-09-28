using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class DisableInactiveUsersJob(
    IUserProfilesManagerService userProfilesManagerService,
    IAppSettingsService appSettingsService) : IScheduledTask
{
    private const int DefaultMinMonthToStayActive = 12;

    public async Task RunAsync()
    {
        if (IsShuttingDown)
        {
            return;
        }

        var appSettings = await appSettingsService.GetAppSettingsAsync();
        var minMonthToStayActive = appSettings?.MinimumRequiredPosts.MinMonthToStayActive;

        if (minMonthToStayActive is null or 0)
        {
            minMonthToStayActive = DefaultMinMonthToStayActive;
        }

        await userProfilesManagerService.DisableInactiveUsersAsync(minMonthToStayActive.Value);
    }

    public bool IsShuttingDown { get; set; }
}
