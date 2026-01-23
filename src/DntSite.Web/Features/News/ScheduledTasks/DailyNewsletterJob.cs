using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.ScheduledTasks;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class DailyNewsletterJob(
    IUsersInfoService usersService,
    IDailyNewsletter dailyNewsletter,
    IJobsEmailsService jobsEmailsService,
    ICachedAppSettingsProvider cachedAppSettingsProvider) : ScheduledTaskBase(cachedAppSettingsProvider)
{
    protected override async Task ExecuteAsync(AppSetting appSetting, CancellationToken cancellationToken)
    {
       if(appSetting is null)
       {
          return;
       }

        var users = await usersService.GetAllDailyEmailReceiversListAsync(SharedConstants.AYearAgo,
            sendToAllEachMonth: true);

        var showBriefDescription = appSetting.ShowRssBriefDescription;
        var dateTime = DateTime.UtcNow.ToIranTimeZoneDateTime().AddDays(value: -1);
        var content = await dailyNewsletter.GetEmailContentAsync(dateTime, showBriefDescription);

        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }

        await jobsEmailsService.SendDailyNewsletterEmailAsync(users, content, dateTime, cancellationToken);
    }
}
