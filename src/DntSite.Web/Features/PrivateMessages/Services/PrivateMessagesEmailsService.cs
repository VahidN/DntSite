using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.PrivateMessages.EmailLayouts;
using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.PrivateMessages.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services;

public class PrivateMessagesEmailsService(
    ICommonService commonService,
    IEmailsFactoryService emailsFactoryService,
    IPasswordHasherService passwordHasherService) : IPrivateMessagesEmailsService
{
    public async Task PrivateMessagesSendEmailAsync(string body,
        string pm,
        string title,
        int toUserId,
        string fromUserFriendlyName)
    {
        var toUser = await commonService.FindUserAsync(toUserId);
        var idHash = passwordHasherService.GetSha256Hash(pm);

        await emailsFactoryService.SendEmailAsync<ContactUsEmail, ContactUsEmailModel>(
            Invariant($"PrivateMessage/id/{idHash}"), "", Invariant($"PrivateMessage/id/{idHash}"),
            new ContactUsEmailModel
            {
                FriendlyName = fromUserFriendlyName,
                Title = $"پاسخ به : {title}",
                Body = body,
                PmId = pm
            }, toUser?.EMail, "پیام خصوصی: " + $"پاسخ به : {title}", false);

        await emailsFactoryService.SendEmailToAllAdminsAsync<ContactUsEmail, ContactUsEmailModel>(
            Invariant($"PrivateMessage/id/{idHash}"), "", Invariant($"PrivateMessage/id/{idHash}"),
            new ContactUsEmailModel
            {
                FriendlyName = fromUserFriendlyName,
                Title = title,
                Body = body,
                PmId = pm
            }, $"پیام خصوصی: {title} : به :{toUser?.FriendlyName} : از طرف : {fromUserFriendlyName}");
    }

    public async Task SendPublicContactUsAsync(PublicContactUsModel data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var items = new PublicContactUsEmailModel
        {
            Title = data.Title,
            FromUserNameEmail = data.FromUserNameEmail,
            FromUserName = data.FromUserName,
            DescriptionText = data.DescriptionText
        };

        var emails = (await commonService.GetAllActiveAdminsAsNoTrackingAsync()).Select(x => x.EMail).ToList();

        await emailsFactoryService.SendEmailToAllUsersAsync<PublicContactUsEmail, PublicContactUsEmailModel>(emails,
            "PublicContactUs", "", "PublicContactUs", items, $"تماس با ما: {data.Title}", true);
    }

    public async Task SendEmailContactUsAsync(ContactUsModel data, User toUser, User fromUser, int firstMessageId)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(toUser);
        ArgumentNullException.ThrowIfNull(fromUser);

        var pmId = firstMessageId.ToString(CultureInfo.InvariantCulture);
        var idHash = passwordHasherService.GetSha256Hash(pmId);

        await emailsFactoryService.SendEmailAsync<ContactUsEmail, ContactUsEmailModel>(
            Invariant($"PrivateMessage/id/{idHash}"), "", Invariant($"PrivateMessage/id/{idHash}"),
            new ContactUsEmailModel
            {
                FriendlyName = fromUser.FriendlyName,
                Title = data.Title,
                Body = data.DescriptionText,
                PmId = pmId
            }, toUser.EMail, $"پیام خصوصی: {data.Title}", false);

        await emailsFactoryService.SendEmailToAllAdminsAsync<ContactUsEmail, ContactUsEmailModel>("ContactUs", "",
            "ContactUs", new ContactUsEmailModel
            {
                FriendlyName = fromUser.FriendlyName,
                Title = data.Title,
                Body = data.DescriptionText,
                PmId = pmId
            }, $"پیام خصوصی: {data.Title} : به :{toUser.FriendlyName} : از طرف : {fromUser.FriendlyName}");
    }

    public Task SendMassEmailAsync(IList<string> emails, string title, string body)
        => emailsFactoryService.SendEmailToAllUsersAsync<NewMassEmail, NewMassEmailModel>(emails, "MassEmail", "",
            "MassEmail", new NewMassEmailModel
            {
                Body = body
            }, title, false);
}
