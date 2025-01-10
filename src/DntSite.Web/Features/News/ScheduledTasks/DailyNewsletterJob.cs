using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class DailyNewsletterJob(
    IUsersInfoService usersService,
    IDailyNewsletter dailyNewsletter,
    IJobsEmailsService jobsEmailsService) : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var users = await usersService.GetAllDailyEmailReceiversListAsync();
        var dateTime = DateTime.UtcNow.ToIranTimeZoneDateTime().AddDays(value: -1);

        var content = await dailyNewsletter.GetEmailContentAsync(dateTime);

        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }

        await jobsEmailsService.SendDailyNewsletterEmailAsync(users, content, dateTime, cancellationToken);
    }
}
