using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.ScheduledTasks;

public class WebReadersListJob(IStatService statService, ICachedAppSettingsProvider cachedAppSettingsProvider)
    : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; } = true;

    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => statService.UpdateAllUsersRatingsAsync(cancellationToken);
}
