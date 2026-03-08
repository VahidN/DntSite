using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class AIDailyNewsJob(
    IAIDailyNewsService aiDailyNewsService,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    ILogger<AIDailyNewsJob> logger) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; } = true;

    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        if (!NetworkExtensions.IsConnectedToInternet(TimeSpan.FromSeconds(seconds: 2)))
        {
            logger.LogWarning(message: "There is no internet connection to run StartProcessingNewsFeedsAsync().");

            return;
        }

        await aiDailyNewsService.StartProcessingNewsFeedsAsync(cancellationToken);
    }
}
