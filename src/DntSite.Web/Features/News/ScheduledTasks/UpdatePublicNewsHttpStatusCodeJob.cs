using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class UpdatePublicNewsHttpStatusCodeJob(
    IDailyNewsItemsService dailyNewsItemsService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => dailyNewsItemsService.UpdateAllNewsLastHttpStatusCodeAsync(UpdateNewsStatusAction.UpdatePublicOnes,
            cancellationToken);
}
