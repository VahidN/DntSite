using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Surveys.EmailLayouts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Services;

public class VotesEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IVotesEmailsService
{
    public Task VoteCommentSendEmailToAdminsAsync(SurveyComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var vote = comment.Parent;

        return emailsFactoryService.SendEmailToAllAdminsAsync<VoteReplyToAdminsEmail, VoteReplyToAdminsEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}/CommentId/{comment.Id}"), inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}"), new VoteReplyToAdminsEmailModel
            {
                Title = vote.Title,
                Username = comment.GuestUser.UserName,
                Body = comment.Body,
                Stat = "عمومی",
                CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                VoteId = vote.Id.ToString(CultureInfo.InvariantCulture)
            }, $"پاسخ به : {vote.Title}");
    }

    public async Task VoteCommentSendEmailToPersonAsync(SurveyComment comment)
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

        var vote = comment.Parent;

        if (comment.UserId.HasValue && IsVoteCommentatorAuthorOfIssue(comment, vote))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindVoteCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsVoteCommentAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService.SendEmailAsync<VoteReplyToPersonEmail, VoteReplyToPersonEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}/CommentId/{comment.Id}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}"), new VoteReplyToPersonEmailModel
                {
                    Title = vote.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    VoteId = vote.Id.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.GuestUser.Email, $"پاسخ به : {vote.Title}", addIp: false);

            return;
        }

        if (IsVoteCommentatorMe(comment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<VoteReplyToPersonEmail, VoteReplyToPersonEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}/CommentId/{comment.Id}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}"), new VoteReplyToPersonEmailModel
                {
                    Title = vote.Title,
                    ReplyToComment = replyToComment.Body,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                    VoteId = vote.Id.ToString(CultureInfo.InvariantCulture)
                }, replyToComment.UserId.Value, $"پاسخ به : {vote.Title}");
        }
    }

    public async Task VoteCommentSendEmailToWritersAsync(SurveyComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        if (comment.IsDeleted)
        {
            return;
        }

        var vote = comment.Parent;

        if (comment.UserId.HasValue && IsVoteCommentatorAuthorOfIssue(comment, vote))
        {
            return; //don't send emails to me again.
        }

        await emailsFactoryService.SendEmailToIdAsync<VoteReplyToWritersEmail, VoteReplyToWritersEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}/CommentId/{comment.Id}"), inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"Vote/{vote.Id}"), new VoteReplyToWritersEmailModel
            {
                Title = vote.Title,
                Username = comment.GuestUser.UserName,
                Body = comment.Body,
                CommentId = comment.Id.ToString(CultureInfo.InvariantCulture),
                VoteId = vote.Id.ToString(CultureInfo.InvariantCulture)
            }, vote.UserId, $"پاسخ به : {vote.Title}");
    }

    public Task VoteSendEmailAsync(Survey result)
    {
        ArgumentNullException.ThrowIfNull(result);

        var options = result.SurveyItems.Any(x => !x.IsDeleted)
            ? result.SurveyItems.Where(x => !x.IsDeleted).Select(x => x.Title).Aggregate((s1, s2) => s1 + "<br/>" + s2)
            : "";

        return emailsFactoryService.SendEmailToAllAdminsAsync<VoteEmail, VoteEmailModel>(messageId: "Vote",
            inReplyTo: "", references: "Vote", new VoteEmailModel
            {
                PmId = result.Id.ToString(CultureInfo.InvariantCulture),
                Title = result.Title,
                Options = options
            }, $"نظرسنجی جدید: {result.Title}");
    }

    private static bool IsVoteCommentAnonymousUser(SurveyComment? replyToComment) => replyToComment?.UserId is null;

    private static bool IsVoteCommentatorAuthorOfIssue(SurveyComment comment, Survey vote)
        => comment.UserId is not null && comment.UserId.Value == vote.UserId;

    private static bool IsVoteCommentatorMe(SurveyComment comment, SurveyComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;
}
