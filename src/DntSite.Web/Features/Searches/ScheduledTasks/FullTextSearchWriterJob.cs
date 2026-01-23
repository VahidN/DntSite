using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.ScheduledTasks;

public class FullTextSearchWriterJob(
    IFullTextSearchWriterService fullTextSearchWriterService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => fullTextSearchWriterService.IndexDatabaseAsync(forceStart: false, cancellationToken);
}
