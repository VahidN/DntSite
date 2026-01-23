using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.AppConfigs.ScheduledTasks;

public class CheckAdminsLastVisitJob(IUsersInfoService usersInfoService, IAppSettingsService appSettingsService)
    : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
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
