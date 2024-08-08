using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.News.EmailLayouts;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IDailyNewsEmailsService
{
    public Task ConvertedDailyNewsItemsSendEmailAsync(int id, string title, int toInt)
        => emailsFactoryService.SendEmailToIdAsync<ConvertedToLink, ConvertedToLinkModel>(
            Invariant($"DailyNewsItem/Id/{id}"), inReplyTo: "", Invariant($"DailyNewsItem/Id/{id}"),
            new ConvertedToLinkModel(), toInt, $"لینک جدید ارسالی:  {title}");

    public async Task DailyNewsItemsSendEmailAsync(DailyNewsItem result, string friendlyName)
    {
        ArgumentNullException.ThrowIfNull(result);

        await emailsFactoryService.SendEmailToAllAdminsAsync<DailyLinksToAdminsEmails, DailyLinksToAdminsEmailsModel>(
            Invariant($"DailyNewsItem/Id/{result.Id}"), inReplyTo: "", Invariant($"DailyNewsItem/Id/{result.Id}"),
            new DailyLinksToAdminsEmailsModel
            {
                NewsId = result.Id,
                Url = result.Url,
                Title = result.Title,
                Body = result.BriefDescription ?? "",
                FriendlyName = friendlyName,
                Stat = "عمومی"
            }, $"لینک جدید ارسالی:  {result.Title}");
    }

    public async Task PostNewsReplySendEmailToAdminsAsync(DailyNewsItemComment data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var post = await commonService.FindNewsCommentPostAsync(data.ParentId);

        if (post is null)
        {
            return;
        }

        await emailsFactoryService
            .SendEmailToAllAdminsAsync<PostNewsReplyToAdminsEmail, PostNewsReplyToAdminsEmailModel>(
                Invariant($"DailyNewsItem/ReplyId/{data.Id}"), inReplyTo: "",
                Invariant($"DailyNewsItem/Id/{data.ParentId}"), new PostNewsReplyToAdminsEmailModel
                {
                    Title = post.Title,
                    Username = data.GuestUser.UserName,
                    Body = data.Body,
                    PmId = data.ParentId.ToString(CultureInfo.InvariantCulture),
                    Stat = "عمومی",
                    CommentId = data.Id.ToString(CultureInfo.InvariantCulture)
                }, $"پاسخ به : {post.Title}");
    }

    public async Task PostNewsReplySendEmailToPersonAsync(DailyNewsItemComment comment)
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

        var post = await commonService.FindNewsCommentPostAsync(comment.ParentId);

        if (post is null)
        {
            return;
        }

        if (comment.UserId.HasValue && IsNewsCommentatorAuthorOfPost(comment, post))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindNewsCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsNewsAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService.SendEmailAsync<PostNewsReplyToPersonEmail, PostNewsReplyToPersonEmailModel>(
                Invariant($"DailyNewsItem/ReplyId/{comment.Id}"), inReplyTo: "",
                Invariant($"DailyNewsItem/Id/{comment.ParentId}"), new PostNewsReplyToPersonEmailModel
                {
                    Title = post.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.GuestUser.Email, $"پاسخ به : {post.Title}", addIp: false);

            return;
        }

        if (IsNewsCommentatorMe(comment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<PostNewsReplyToPersonEmail, PostNewsReplyToPersonEmailModel>(
                Invariant($"DailyNewsItem/ReplyId/{comment.Id}"), inReplyTo: "",
                Invariant($"DailyNewsItem/Id/{comment.ParentId}"), new PostNewsReplyToPersonEmailModel
                {
                    Title = post.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.UserId.Value, $"پاسخ به : {post.Title}");
        }
    }

    public async Task PostNewsReplySendEmailToWritersAsync(DailyNewsItemComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        if (comment.IsDeleted)
        {
            return;
        }

        var post = await commonService.FindNewsCommentPostAsync(comment.ParentId);

        if (post is null)
        {
            return;
        }

        if (comment.UserId.HasValue && IsNewsCommentatorAuthorOfPost(comment, post))
        {
            return; //don't send emails to me again.
        }

        await emailsFactoryService.SendEmailToIdAsync<PostNewsReplyToWritersEmail, PostNewsReplyToWritersEmailModel>(
            Invariant($"DailyNewsItem/ReplyId/{comment.Id}"), inReplyTo: "",
            Invariant($"DailyNewsItem/Id/{comment.ParentId}"), new PostNewsReplyToWritersEmailModel
            {
                Title = post.Title,
                Username = comment.GuestUser.UserName,
                Body = comment.Body,
                PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                CommentId = comment.Id.ToString(CultureInfo.InvariantCulture)
            }, post.UserId, $"پاسخ به : {post.Title}");
    }

    private static bool IsNewsAnonymousUser(DailyNewsItemComment replyToComment) => replyToComment.UserId is null;

    private static bool IsNewsCommentatorAuthorOfPost(DailyNewsItemComment comment, DailyNewsItem post)
        => comment.UserId is not null && comment.UserId.Value == post.UserId;

    private static bool IsNewsCommentatorMe(DailyNewsItemComment comment, DailyNewsItemComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;
}
