using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.PrivateMessages.ScheduledTasks;

public class EmptyPMsJob(
    IPrivateMessagesService privateMessagesService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => privateMessagesService.DeleteAllAsync(cancellationToken);
}
