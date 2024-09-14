using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IJobsEmailsService : IScopedService
{
    Task SendDailyNewsletterEmailAsync(IList<User> users, string content, DateTime yesterday);

    Task SendDailyBirthDatesEmailAsync();

    Task SendNewPersianYearEmailsAsync();
}
