using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IPrivateMessagesEmailsService : IScopedService
{
    public Task SendEmailContactUsAsync(ContactUsModel data, User toUser, User fromUser, int firstMessageId);

    public Task PrivateMessagesSendEmailAsync(string body,
        string pm,
        string title,
        int toUserId,
        string fromUserFriendlyName);

    public Task SendPublicContactUsAsync(PublicContactUsModel data);

    public Task SendMassEmailAsync(IList<string> emails, string title, string body);
}
