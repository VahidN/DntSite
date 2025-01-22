using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.UserProfiles.EmailLayouts;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Services;

public class AdminsEmailsService(
    ICommonService commonService,
    IEmailsFactoryService emailsFactoryService,
    ICurrentUserService currentUserService) : IAdminsEmailsService
{
    public async Task PostNewReferrersEmailAsync(Uri destUri, Uri sourceUri)
    {
        ArgumentNullException.ThrowIfNull(destUri);
        ArgumentNullException.ThrowIfNull(sourceUri);

        var adminUrl = destUri.GetLeftPart(UriPartial.Authority) + "/ReferrersList";
        var emails = (await commonService.GetAllActiveAdminsAsNoTrackingAsync()).Select(x => x.EMail).ToList();

        await emailsFactoryService.SendEmailToAllUsersAsync<NewReferrersEmail, NewReferrersEmailModel>(emails,
            messageId: "NewReferrer", inReplyTo: "", references: "NewReferrer", new NewReferrersEmailModel
            {
                Source = sourceUri.ToString(),
                Dest = destUri.ToString(),
                AdminUrl = adminUrl
            }, emailSubject: "ارجاع دهنده جدید برای تائید", addIp: true);
    }

    public Task CommonFileEditedSendEmailAsync(string name, string description)
        => emailsFactoryService.SendEmailToAllAdminsAsync<CommonFileEditedEmail, CommonFileEditedEmailModel>(
            messageId: "CommonFileEdited", inReplyTo: "", references: "CommonFileEdited", new CommonFileEditedEmailModel
            {
                Name = name,
                Description = description
            }, $"ویرایش فایل:  {name}");

    public async Task UploadFileSendEmailAsync(string path, string actionUrl, string formattedFileSize)
        => await emailsFactoryService.SendEmailToAllAdminsAsync<FileUploadEmail, FileUploadEmailModel>(
            messageId: "RedactorUpload", inReplyTo: "", references: "RedactorUpload", new FileUploadEmailModel
            {
                ActionUrl = actionUrl,
                FriendlyName = (await currentUserService.GetCurrentUserAsync())?.User?.FriendlyName ??
                               SharedConstants.GuestUserName,
                FormattedFileSize = formattedFileSize
            }, $"آپلود فایل جدید:  {path}");

    public Task SendRecycleEmailAsync(string id, string title, string body)
        => emailsFactoryService.SendEmailToAllAdminsAsync<RecycleEmail, RecycleEmailModel>(messageId: "SendRecycle",
            inReplyTo: "", references: "SendRecycle", new RecycleEmailModel
            {
                Id = id,
                Title = title,
                Body = body
            }, $"بازیابی مطلب: {title}");

    public Task TagEditedSendEmailAsync<TLayout, TLayoutModel>(TLayoutModel data)
        where TLayout : IComponent
        where TLayoutModel : BaseEmailModel
        => emailsFactoryService.SendEmailToAllAdminsAsync<TLayout, TLayoutModel>(messageId: "TagEdited", inReplyTo: "",
            references: "TagEdited", data, emailSubject: "ویرایش نام گروه‌ها");
}
