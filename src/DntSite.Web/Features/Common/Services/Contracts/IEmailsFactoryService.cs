using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Common.Services.Contracts;

public interface IEmailsFactoryService : IScopedService
{
    public Task SendTextToAllAdminsAsync(string text);

    public Task SendNormalEmailAsync(string messageId,
        string inReplyTo,
        string references,
        string htmlTemplateContent,
        string? toEmail,
        string subject);

    public Task SendEmailAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string? toEmail,
        string emailSubject,
        bool addIp)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    public Task SendEmailToIdAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        int? toUserId,
        string emailSubject)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    public Task SendEmailToAllAdminsAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string emailSubject)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    public Task SendEmailToAllAdminsNormalAsync(string messageId,
        string inReplyTo,
        string references,
        string html,
        string emailSubject);

    public Task SendEmailToAllUsersAsync<TLayout, TLayoutModel>(IList<string> emails,
        string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string emailSubject,
        bool addIp)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    public Task SendEmailToAllUsersNormalAsync(IList<string> emails,
        string messageId,
        string inReplyTo,
        string references,
        string html,
        string emailSubject);
}
