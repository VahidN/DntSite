using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class AIDailyNewsJob(
    IAIDailyNewsService aiDailyNewsService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => aiDailyNewsService.StartProcessingNewsFeedsAsync(cancellationToken);
}
