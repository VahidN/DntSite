using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class DisableInactiveUsersJob(
    IUserProfilesManagerService userProfilesManagerService,
    ICachedAppSettingsProvider appSettingsService) : IScheduledTask
{
    private const int DefaultMinMonthToStayActive = 12;

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var minMonthToStayActive = await GetMinMonthToStayActiveAsync();
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

    private async Task<int> GetMinMonthToStayActiveAsync()
    {
        var appSettings = await appSettingsService.GetAppSettingsAsync();
        var minMonthToStayActive = appSettings?.MinimumRequiredPosts.MinMonthToStayActive;

        return minMonthToStayActive is null or 0 ? DefaultMinMonthToStayActive : (int)minMonthToStayActive;
    }
}
