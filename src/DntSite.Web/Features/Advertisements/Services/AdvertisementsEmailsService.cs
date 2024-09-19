using DntSite.Web.Features.Advertisements.EmailLayouts;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;

namespace DntSite.Web.Features.Advertisements.Services;

public class AdvertisementsEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IAdvertisementsEmailsService
{
    public Task AddAdvertisementSendEmailAsync(Advertisement advertisement)
    {
        ArgumentNullException.ThrowIfNull(advertisement);

        return emailsFactoryService.SendEmailToAllAdminsAsync<WriteAdvertisementEmail, WriteAdvertisementEmailModel>(
            messageId: "AddAdvertisement", inReplyTo: "", references: "AddAdvertisement",
            new WriteAdvertisementEmailModel
            {
                Title = advertisement.Title,
                Body = advertisement.Body,
                PmId = advertisement.Id.ToString(CultureInfo.InvariantCulture)
            }, $"تبلیغ جدید: {advertisement.Title}");
    }

    public Task AdvertisementCommentSendEmailToAdminsAsync(AdvertisementComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var data = comment.Parent;

        return emailsFactoryService
            .SendEmailToAllAdminsAsync<AdvertisementReplyToAdminsEmail, AdvertisementReplyToAdminsEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"AdvertisementId/{data.Id}/CommentId/{comment.Id}"),
                inReplyTo: "", string.Create(CultureInfo.InvariantCulture, $"AdvertisementId/{data.Id}"),
                new AdvertisementReplyToAdminsEmailModel
                {
                    Title = data.Title,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    Stat = "عمومی",
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    AdvertisementId = data.Id.ToString(CultureInfo.InvariantCulture)
                }, $"پاسخ به : {data.Title}");
    }

    public async Task AdvertisementCommentSendEmailToPersonAsync(AdvertisementComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var replyId = comment.ReplyId;

        if (replyId is null)
        {
            return;
        }

        if (comment.IsDeleted)
        {
            return;
        }

        var advertisement = comment.Parent;

        if (comment.UserId.HasValue && IsAdvertisementCommentatorAuthorOfIssue(comment, advertisement))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindAdvertisementCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsAdvertisementCommentAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService
                .SendEmailAsync<AdvertisementReplyToPersonEmail, AdvertisementReplyToPersonEmailModel>(
                    string.Create(CultureInfo.InvariantCulture,
                        $"AdvertisementId/{advertisement.Id}/CommentId/{comment.Id}"), inReplyTo: "",
                    string.Create(CultureInfo.InvariantCulture, $"AdvertisementId/{advertisement.Id}"),
                    new AdvertisementReplyToPersonEmailModel
                    {
                        Title = advertisement.Title,
                        ReplyToComment = replyToComment.Body,
                        Username = comment.GuestUser.UserName,
                        Body = comment.Body,
                        CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                        AdvertisementId = advertisement.Id.ToString(CultureInfo.InvariantCulture)
                    }, replyToComment.GuestUser.Email, $"پاسخ به : {advertisement.Title}", addIp: false);

            return;
        }

        if (IsAdvertisementCommentatorMe(comment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is null)
        {
            return;
        }

        await emailsFactoryService
            .SendEmailToIdAsync<AdvertisementReplyToPersonEmail, AdvertisementReplyToPersonEmailModel>(
                string.Create(CultureInfo.InvariantCulture,
                    $"AdvertisementId/{advertisement.Id}/CommentId/{comment.Id}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"AdvertisementId/{advertisement.Id}"),
                new AdvertisementReplyToPersonEmailModel
                {
                    Title = advertisement.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    AdvertisementId = advertisement.Id.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.UserId.Value, $"پاسخ به : {advertisement.Title}");
    }

    public async Task AdvertisementCommentSendEmailToWritersAsync(AdvertisementComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        if (comment.IsDeleted)
        {
            return;
        }

        var advertisement = comment.Parent;

        if (comment.UserId.HasValue && IsAdvertisementCommentatorAuthorOfIssue(comment, advertisement))
        {
            return; //don't send emails to me again.
        }

        if (advertisement.UserId is null)
        {
            return;
        }

        await emailsFactoryService
            .SendEmailToIdAsync<AdvertisementReplyToWritersEmail, AdvertisementReplyToWritersEmailModel>(
                string.Create(CultureInfo.InvariantCulture,
                    $"AdvertisementId/{advertisement.Id}/CommentId/{comment.Id}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"AdvertisementId/{advertisement.Id}"),
                new AdvertisementReplyToWritersEmailModel
                {
                    Title = advertisement.Title,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    AdvertisementId = advertisement.Id.ToString(CultureInfo.InvariantCulture)
                }, advertisement.UserId.Value, $"پاسخ به : {advertisement.Title}");
    }

    private static bool IsAdvertisementCommentAnonymousUser(AdvertisementComment? replyToComment)
        => replyToComment?.UserId is null;

    private static bool IsAdvertisementCommentatorAuthorOfIssue(AdvertisementComment comment,
        Advertisement advertisement)
        => comment.UserId is not null && comment.UserId.Value == advertisement.UserId;

    private static bool IsAdvertisementCommentatorMe(AdvertisementComment comment, AdvertisementComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;
}
