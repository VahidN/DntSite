using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class AIDailyNewsBacklogsJob(
    IDailyNewsItemAIBacklogService dailyNewsItemAiBacklogService,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    ILogger<AIDailyNewsBacklogsJob> logger) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
        if (!NetworkExtensions.IsConnectedToInternet(TimeSpan.FromSeconds(seconds: 2)))
        {
            logger.LogWarning(
                message: "There is no internet connection to run AddFeedItemsAsDailyNewsItemAIBacklogsAsync().");

            return;
        }

        await dailyNewsItemAiBacklogService.AddFeedItemsAsDailyNewsItemAIBacklogsAsync(cancellationToken);
    }
}
