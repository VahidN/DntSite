using DntSite.Web.Features.PrivateMessages.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.PrivateMessages.Services.Contracts;

public interface IPrivateMessagesEmailsService : IScopedService
{
    Task SendEmailContactUsAsync(ContactUsModel data, User toUser, User fromUser, int firstMessageId);

    Task PrivateMessagesSendEmailAsync(string body, string pm, string title, int toUserId, string fromUserFriendlyName);

    Task SendPublicContactUsAsync(PublicContactUsModel data);

    Task SendMassEmailAsync(IList<string> emails, string title, string body);
}
