using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.UserProfiles.EmailLayouts;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Common.Services;

public class EmailsFactoryService(
    ICommonService commonService,
    IBlazorStaticRendererService rendererService,
    IWebMailService webMailService,
    IHttpContextAccessor httpContextAccessor,
    IAppFoldersService appFoldersService) : IEmailsFactoryService
{
    private const string DefaultPickupFolderName = "SmtpPickup";
    private readonly TimeSpan _delayDelivery = TimeSpan.FromSeconds(value: 7);
    private readonly int _numberOfProcessedMessages = 30;

    public async Task SendEmailAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string? toEmail,
        string emailSubject,
        bool addIp)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!toEmail.IsValidEmail())
        {
            return;
        }

        await InitEmailModelAsync(model);

        var html = await rendererService.StaticRenderComponentAsync<TLayout>(new Dictionary<string, object?>
        {
            {
                "Model", model
            }
        });

        var userIp = await GetUserInfoAsync();

        if (addIp && !html.Contains(userIp, StringComparison.OrdinalIgnoreCase))
        {
            html += userIp;
        }

        await SendNormalEmailAsync(messageId, inReplyTo, references, html, toEmail, emailSubject);
    }

    public async Task SendEmailToAllAdminsNormalAsync(string messageId,
        string inReplyTo,
        string references,
        string html,
        string emailSubject)
    {
        var emails = (await commonService.GetAllActiveAdminsAsNoTrackingAsync()).Select(x => x.EMail).ToList();
        await SendEmailToAllUsersNormalAsync(emails, messageId, inReplyTo, references, html, emailSubject);
    }

    public async Task SendNormalEmailAsync(string messageId,
        string inReplyTo,
        string references,
        string htmlTemplateContent,
        string? toEmail,
        string subject)
    {
        if (!toEmail.IsValidEmail())
        {
            return;
        }

        var appSetting = await GetAppSettingsAsync() ??
                         throw new InvalidOperationException(message: "appSetting is null");

        var smtpServerSetting = appSetting.SmtpServerSetting;
        var pickupFolder = GetPickupFolderPath(smtpServerSetting);

        await webMailService.SendEmailAsync(new SmtpConfig
            {
                Server = smtpServerSetting.Address,
                Port = smtpServerSetting.Port,
                Username = smtpServerSetting.Username,
                Password = smtpServerSetting.Password,
                FromAddress = appSetting.SiteFromEmail!,
                FromName = appSetting.BlogName,
                UsePickupFolder = smtpServerSetting.UsePickupFolder,
                PickupFolder = pickupFolder
            }, [
                new MailAddress
                {
                    ToAddress = toEmail
                }
            ], string.Create(CultureInfo.InvariantCulture, $"{appSetting.BlogName} - {subject}"), htmlTemplateContent,
            headers: new MailHeaders
            {
                InReplyTo = inReplyTo,
                MessageId = messageId,
                References = references,
                UnSubscribeUrl =
                    appSetting.SiteRootUri.CombineUrl(UserProfilesRoutingConstants.EditProfile,
                        escapeRelativeUrl: false)
            }, shouldValidateServerCertificate: smtpServerSetting.ShouldValidateServerCertificate);
    }

    public async Task SendEmailToAllAdminsAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string emailSubject)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel
    {
        var emails = (await commonService.GetAllActiveAdminsAsNoTrackingAsync()).Select(x => x.EMail).ToList();

        await SendEmailToAllUsersAsync<TLayout, TLayoutModel>(emails, messageId, inReplyTo, references, model,
            emailSubject, addIp: true);
    }

    public async Task SendEmailToIdAsync<TLayout, TLayoutModel>(string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        int? toUserId,
        string emailSubject)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel
    {
        if (toUserId is null)
        {
            return;
        }

        var user = await commonService.FindUserAsync(toUserId);

        if (user is null)
        {
            return;
        }

        var toEmail = user.EMail;

        await SendEmailAsync<TLayout, TLayoutModel>(messageId, inReplyTo, references, model, toEmail, emailSubject,
            addIp: false);
    }

    public Task SendTextToAllAdminsAsync(string text)
        => SendEmailToAllAdminsAsync<SendTextToAdmins, SendTextToAdminsModel>(messageId: "Text", inReplyTo: "",
            references: "Text", new SendTextToAdminsModel
            {
                Text = text
            }, emailSubject: "جهت اطلاع");

    public async Task SendEmailToAllUsersAsync<TLayout, TLayoutModel>(IList<string> emails,
        string messageId,
        string inReplyTo,
        string references,
        TLayoutModel model,
        string emailSubject,
        bool addIp)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel
    {
        ArgumentNullException.ThrowIfNull(emails);
        var count = 0;

        foreach (var email in emails)
        {
            await SendEmailAsync<TLayout, TLayoutModel>(messageId, inReplyTo, references, model, email, emailSubject,
                addIp);

            count++;

            if (count % _numberOfProcessedMessages == 0)
            {
                await Task.Delay(_delayDelivery);
            }
        }
    }

    public async Task SendEmailToAllUsersNormalAsync(IList<string> emails,
        string messageId,
        string inReplyTo,
        string references,
        string html,
        string emailSubject)
    {
        ArgumentNullException.ThrowIfNull(emails);
        var count = 0;

        foreach (var email in emails)
        {
            await SendNormalEmailAsync(messageId, inReplyTo, references, html, email, emailSubject);

            count++;

            if (count % _numberOfProcessedMessages == 0)
            {
                await Task.Delay(_delayDelivery);
            }
        }
    }

    private string GetPickupFolderPath(SmtpServerSetting smtpServerSetting)
    {
        var folderName = string.IsNullOrWhiteSpace(smtpServerSetting.PickupFolderName)
            ? DefaultPickupFolderName
            : smtpServerSetting.PickupFolderName;

        return appFoldersService.GetWebRootAppDataFolderPath(folderName);
    }

    private Task<AppSetting?> GetAppSettingsAsync() => commonService.GetBlogConfigAsync();

    private async Task<string> GetUserInfoAsync()
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return string.Empty;
        }

        var ip = httpContextAccessor.HttpContext.GetIP();

        if (string.IsNullOrWhiteSpace(ip))
        {
            return string.Empty;
        }

        var user = await httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>()
            .GetCurrentUserAsync();

        return string.Create(CultureInfo.InvariantCulture,
            $"<br/><hr/><div align='center' dir='ltr'>Sent from IP: {ip} / {user.FriendlyName} / {user.UserId}</div>");
    }

    private async Task InitEmailModelAsync<TLayoutModel>(TLayoutModel model)
        where TLayoutModel : BaseEmailModel
    {
        var appSetting = await GetAppSettingsAsync() ??
                         throw new InvalidOperationException(message: "appSetting is null");

        model.EmailSig = appSetting.SiteEmailsSig ?? "";
        model.MsgDateTime = DateTime.UtcNow.ToLongPersianDateTimeString().ToPersianNumbers();
        model.SiteTitle = appSetting.BlogName;
        model.SiteRootUri = $"{appSetting.SiteRootUri.TrimEnd(trimChar: '/')}/";
    }
}
