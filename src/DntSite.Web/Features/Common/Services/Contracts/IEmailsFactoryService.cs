using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Common.Services.Contracts;

public interface IEmailsFactoryService : IScopedService
{
    Task InitEmailModelAsync<TLayoutModel>(TLayoutModel model)
        where TLayoutModel : BaseEmailModel;

    Task SendTextToAllAdminsAsync(string text, string emailSubject = "جهت اطلاع");

    Task SendNormalEmailAsync(string messageId,
        string inReplyTo,
        string references,
        string htmlTemplateContent,
        string? toEmail,
        string subject);

    Task SendEmailAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string? toEmail,
        string emailSubject,
        bool addIp)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    Task SendEmailToIdAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        int? toUserId,
        string emailSubject)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    Task SendEmailToAllAdminsAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string emailSubject,
        CancellationToken cancellationToken = default)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    Task SendEmailToAllAdminsNormalAsync(string messageId,
        string inReplyTo,
        string references,
        string html,
        string emailSubject);

    Task SendEmailToAllUsersAsync<TLayout, TLayoutModel>(IList<string> emails,
        string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string emailSubject,
        bool addIp,
        CancellationToken cancellationToken = default)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel;

    Task SendEmailToAllUsersNormalAsync(IList<string> emails,
        string messageId,
        string inReplyTo,
        string references,
        string html,
        string emailSubject);
}
