﻿using DntSite.Web.Features.Common.Services.Contracts;
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
    public Task SendDailyNewsletterEmailAsync(IList<User> users, string url, string content, DateTime yesterday)
    {
        var emails = users.Select(x => x.EMail).ToList();

        return emailsFactoryService.SendEmailToAllUsersAsync<DailyNewsletterEmail, DailyNewsletterEmailModel>(emails,
            messageId: "DailyNewsletter", inReplyTo: "", references: "DailyNewsletter", new DailyNewsletterEmailModel
            {
                BaseUrl = url,
                Body = content
            }, $"خلاصه مطالب {yesterday.ToPersianDateTextify()}", addIp: false);
    }

    public async Task SendNewPersianYearEmailsAsync()
    {
        var emails = (await commonService.AllValidatedEmailsUsersAsync()).Select(x => x.EMail).ToList();

        await emailsFactoryService.SendEmailToAllUsersAsync<NewPersianYear, NewPersianYearModel>(emails,
            messageId: "NewPersianYear", inReplyTo: "", references: "NewPersianYear", new NewPersianYearModel(),
            emailSubject: "سال نو مبارک", addIp: false);
    }

    public async Task SendDailyBirthDatesEmailAsync()
    {
        var emails = (await statService.GetTodayBirthdayListAsync()).Select(x => x.EMail).ToList();

        await emailsFactoryService.SendEmailToAllUsersAsync<Birthday, BirthdayModel>(emails,
            messageId: "DailyBirthDates", inReplyTo: "DailyBirthDates", references: "DailyBirthDates",
            new BirthdayModel(), emailSubject: "سال روز تولدتان مبارک", addIp: false);

        await emailsFactoryService.SendEmailToAllAdminsAsync<Birthday, BirthdayModel>(messageId: "DailyBirthDates",
            inReplyTo: "DailyBirthDates", references: "DailyBirthDates", new BirthdayModel(),
            emailSubject: "سال روز تولدتان مبارک");
    }
}
