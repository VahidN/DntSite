using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.EmailLayouts;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Services;

public class QuestionsEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IQuestionsEmailsService
{
    public async Task StackExchangeQuestionSendEmailAsync(StackExchangeQuestion result, string friendlyName)
    {
        ArgumentNullException.ThrowIfNull(result);

        await emailsFactoryService.SendEmailToAllAdminsAsync<QuestionsToAdminsEmails, QuestionsToAdminsEmailsModel>(
            Invariant($"StackExchangeQuestion/Id/{result.Id}"), inReplyTo: "",
            Invariant($"StackExchangeQuestion/Id/{result.Id}"), new QuestionsToAdminsEmailsModel
            {
                Title = result.Title,
                Body = result.Description,
                FriendlyName = friendlyName,
                Stat = "عمومی"
            }, $"پرسش جدید ارسالی:  {result.Title}");
    }

    public async Task PostQuestionCommentReplySendEmailToAdminsAsync(StackExchangeQuestionComment data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var post = await commonService.FindStackExchangeQuestionCommentPostAsync(data.ParentId);

        if (post is null)
        {
            return;
        }

        await emailsFactoryService
            .SendEmailToAllAdminsAsync<QuestionsReplyToAdminsEmail, QuestionsReplyToAdminsEmailModel>(
                Invariant($"StackExchangeQuestion/ReplyId/{data.Id}"), inReplyTo: "",
                Invariant($"StackExchangeQuestion/Id/{data.ParentId}"), new QuestionsReplyToAdminsEmailModel
                {
                    Title = post.Title,
                    Username = data.GuestUser.UserName,
                    Body = data.Body,
                    PmId = data.ParentId.ToString(CultureInfo.InvariantCulture),
                    Stat = "عمومی",
                    CommentId = data.Id.ToString(CultureInfo.InvariantCulture)
                }, $"پاسخ به : {post.Title}");
    }

    public async Task PostQuestionCommentsReplySendEmailToPersonAsync(StackExchangeQuestionComment comment)
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

        var post = await commonService.FindStackExchangeQuestionCommentPostAsync(comment.ParentId);

        if (post is null)
        {
            return;
        }

        if (comment.UserId.HasValue && IsPostCommentatorAuthorOfPost(comment, post))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindStackExchangeQuestionCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsQuestionCommentAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService.SendEmailAsync<QuestionsReplyToPersonEmail, QuestionsReplyToPersonEmailModel>(
                Invariant($"StackExchangeQuestion/ReplyId/{comment.Id}"), inReplyTo: "",
                Invariant($"StackExchangeQuestion/Id/{comment.ParentId}"), new QuestionsReplyToPersonEmailModel
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

        if (IsPostCommentatorMe(comment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is not null)
        {
            await emailsFactoryService
                .SendEmailToIdAsync<QuestionsReplyToPersonEmail, QuestionsReplyToPersonEmailModel>(
                    Invariant($"StackExchangeQuestion/ReplyId/{comment.Id}"), inReplyTo: "",
                    Invariant($"StackExchangeQuestion/Id/{comment.ParentId}"), new QuestionsReplyToPersonEmailModel
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

    public async Task PostQuestionCommentsReplySendEmailToWritersAsync(StackExchangeQuestionComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        if (comment.IsDeleted)
        {
            return;
        }

        var post = await commonService.FindStackExchangeQuestionCommentPostAsync(comment.ParentId);

        if (post is null)
        {
            return;
        }

        if (comment.UserId.HasValue && IsPostCommentatorAuthorOfPost(comment, post))
        {
            return; //don't send emails to me again.
        }

        await emailsFactoryService.SendEmailToIdAsync<QuestionsReplyToWritersEmail, QuestionsReplyToWritersEmailModel>(
            Invariant($"StackExchangeQuestion/ReplyId/{comment.Id}"), inReplyTo: "",
            Invariant($"StackExchangeQuestion/Id/{comment.ParentId}"), new QuestionsReplyToWritersEmailModel
            {
                Title = post.Title,
                Username = comment.GuestUser.UserName,
                Body = comment.Body,
                PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                CommentId = comment.Id.ToString(CultureInfo.InvariantCulture)
            }, post.UserId, $"پاسخ به : {post.Title}");
    }

    private static bool IsQuestionCommentAnonymousUser(StackExchangeQuestionComment replyToComment)
        => replyToComment.UserId is null;

    private static bool IsPostCommentatorAuthorOfPost(StackExchangeQuestionComment comment, StackExchangeQuestion post)
        => comment.UserId is not null && comment.UserId.Value == post.UserId;

    private static bool IsPostCommentatorMe(StackExchangeQuestionComment comment,
        StackExchangeQuestionComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;
}
