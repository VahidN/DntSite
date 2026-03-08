using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.ScheduledTasks;

public class SendActivationEmailsJob(
    IUsersManagerEmailsService emailsService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : AppSettingAwareScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override bool ShouldNotBeExecutedIfSiteIsNotActive { get; set; } = true;

    protected override Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
        => emailsService.ResetNotActivatedUsersAndSendEmailAsync(SharedConstants.AYearAgo, cancellationToken);
}
