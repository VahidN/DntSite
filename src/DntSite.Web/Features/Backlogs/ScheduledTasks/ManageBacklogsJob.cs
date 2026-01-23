using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;

namespace DntSite.Web.Features.Backlogs.ScheduledTasks;

public class ManageBacklogsJob(IBacklogsService backlogsService, ICachedAppSettingsProvider cachedAppSettingsProvider)
    : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => backlogsService.CancelOldOnesAsync(cancellationToken);
}
