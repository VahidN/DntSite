using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class DailyNewsletterJob(
    IUsersInfoService usersService,
    IDailyNewsletter dailyNewsletter,
    IJobsEmailsService jobsEmailsService,
    ICommonService commonService) : IScheduledTask
{
    public async Task RunAsync()
    {
        if (IsShuttingDown)
        {
            return;
        }

        var users = await usersService.GetAllDailyEmailReceiversListAsync();
        var yesterday = DateTime.UtcNow.AddDays(value: -1);
        var appSetting = await commonService.GetBlogConfigAsync();

        if (appSetting is null)
        {
            throw new InvalidOperationException(message: "appSetting is null");
        }

        var url = $"{appSetting.SiteRootUri.TrimEnd(trimChar: '/')}/";
        var content = await dailyNewsletter.GetEmailContentAsync(url, yesterday);

        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }

        await jobsEmailsService.SendDailyNewsletterEmailAsync(users, url, content, yesterday);
    }

    public bool IsShuttingDown { get; set; }
}
