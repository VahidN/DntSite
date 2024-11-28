using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IJobsEmailsService : IScopedService
{
    public Task SendDailyNewsletterEmailAsync(IList<User> users, string content, DateTime yesterday);

    public Task SendDailyBirthDatesEmailAsync();

    public Task SendNewPersianYearEmailsAsync();
}
