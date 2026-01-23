using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.ScheduledTasks;

public class DeleteOrphansJob(
    IBlogPostDraftsService blogPostDraftsService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => blogPostDraftsService.DeleteConvertedBlogPostDraftsAsync(cancellationToken);
}
