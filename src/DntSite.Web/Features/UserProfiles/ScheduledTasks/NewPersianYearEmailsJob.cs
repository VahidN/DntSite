using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class NewPersianYearEmailsJob(
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IJobsEmailsService jobsEmailsService) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; }

    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : jobsEmailsService.SendNewPersianYearEmailsAsync(cancellationToken);
}
