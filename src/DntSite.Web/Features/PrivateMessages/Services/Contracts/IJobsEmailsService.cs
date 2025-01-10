using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IJobsEmailsService : IScopedService
{
    public Task SendDailyNewsletterEmailAsync(IList<User> users,
        string content,
        DateTime yesterday,
        CancellationToken cancellationToken);

    public Task SendDailyBirthDatesEmailAsync(CancellationToken cancellationToken);

    public Task SendNewPersianYearEmailsAsync(CancellationToken cancellationToken);
}
