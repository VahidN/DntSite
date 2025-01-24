using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.PrivateMessages.EmailLayouts;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services;

public class JobsEmailsService(
    IEmailsFactoryService emailsFactoryService,
    ICommonService commonService,
    IStatService statService) : IJobsEmailsService
{
    public Task SendDailyNewsletterEmailAsync(IList<User> users,
        string content,
        DateTime yesterday,
        CancellationToken cancellationToken)
    {
        var emails = users.Select(x => x.EMail).ToList();

        var emailSubject = $"خلاصه مطالب {yesterday.ToPersianDateTextify()}";

        return emailsFactoryService.SendEmailToAllUsersAsync<DailyNewsletterEmail, DailyNewsletterEmailModel>(emails,
            messageId: "DailyNewsletter", inReplyTo: "", references: "DailyNewsletter", new DailyNewsletterEmailModel
            {
                Body = $"<b>{emailSubject}</b><br/><br/>{content}"
            }, emailSubject, addIp: false, cancellationToken);
    }

    public async Task SendNewPersianYearEmailsAsync(CancellationToken cancellationToken)
    {
        var emails = (await commonService.AllValidatedEmailsUsersAsync()).Select(x => x.EMail).ToList();

        await emailsFactoryService.SendEmailToAllUsersAsync<NewPersianYear, NewPersianYearModel>(emails,
            messageId: "NewPersianYear", inReplyTo: "", references: "NewPersianYear", new NewPersianYearModel(),
            emailSubject: "سال نو مبارک", addIp: false, cancellationToken);
    }

    public async Task SendDailyBirthDatesEmailAsync(CancellationToken cancellationToken)
    {
        var emails = (await statService.GetTodayBirthdayListAsync(SharedConstants.AYearAgo)).Select(x => x.EMail)
            .ToList();

        await emailsFactoryService.SendEmailToAllUsersAsync<Birthday, BirthdayModel>(emails,
            messageId: "DailyBirthDates", inReplyTo: "DailyBirthDates", references: "DailyBirthDates",
            new BirthdayModel(), emailSubject: "سال روز تولدتان مبارک", addIp: false, cancellationToken);
    }
}
