using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Posts.EmailLayouts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.Services;

public class BlogCommentsEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IBlogCommentsEmailsService
{
    public async Task PostReplySendEmailToAdminsAsync(BlogPostComment data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var post = await commonService.FindCommentPostAsync(data.ParentId);

        if (post is null)
        {
            return;
        }

        await emailsFactoryService.SendEmailToAllAdminsAsync<PostReplyToAdminsEmail, PostReplyToAdminsEmailModel>(
            Invariant($"PostReply/CommentId/{data.Id}"), inReplyTo: "",
            Invariant($"PostReply/BlogPostId/{data.ParentId}"), new PostReplyToAdminsEmailModel
            {
                Title = post.Title,
                Username = data.GuestUser.UserName,
                Body = data.Body,
                PmId = data.ParentId.ToString(CultureInfo.InvariantCulture),
                Stat = "عمومی",
                CommentId = data.Id.ToString(CultureInfo.InvariantCulture)
            }, $"پاسخ به : {post.Title}");
    }

    public async Task PostReplySendEmailToPersonAsync(BlogPostComment comment)
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

        var post = await commonService.FindCommentPostAsync(comment.ParentId);

        if (post is null)
        {
            return;
        }

        if (comment.UserId.HasValue && IsCommentatorAuthorOfPost(comment, post))
        {
            return; //don't send emails to me again.
        }

        var replyToComment = await commonService.FindCommentAsync(replyId.Value);

        if (replyToComment is null)
        {
            return;
        }

        if (IsAnonymousUser(replyToComment))
        {
            if (!replyToComment.GuestUser.Email.IsValidEmail())
            {
                return;
            }

            await emailsFactoryService.SendEmailAsync<PostReplyToPersonEmail, PostReplyToPersonEmailModel>(
                Invariant($"PostReply/CommentId/{comment.Id}"), inReplyTo: "",
                Invariant($"PostReply/BlogPostId/{comment.ParentId}"), new PostReplyToPersonEmailModel
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

        if (IsCommentatorMe(comment, replyToComment))
        {
            return;
        }

        if (replyToComment.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<PostReplyToPersonEmail, PostReplyToPersonEmailModel>(
                Invariant($"PostReply/CommentId/{comment.Id}"), inReplyTo: "",
                Invariant($"PostReply/BlogPostId/{comment.ParentId}"), new PostReplyToPersonEmailModel
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

    public Task ConvertedToReplySendEmailAsync(int postId, string title, int userIdValue)
        => emailsFactoryService.SendEmailToIdAsync<ConvertedToReply, ConvertedToReplyModel>(
            Invariant($"Reply/PostId/{postId}"), inReplyTo: "", Invariant($"Reply/PostId/{postId}"),
            new ConvertedToReplyModel
            {
                PostId = postId.ToString(CultureInfo.InvariantCulture)
            }, userIdValue, $"نظر تکمیلی جدید ارسالی برای مطلب: {title}");

    public async Task PostReplySendEmailToWritersAsync(BlogPostComment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        if (comment.IsDeleted)
        {
            return;
        }

        var post = await commonService.FindCommentPostAsync(comment.ParentId);

        if (post is null)
        {
            return;
        }

        if (comment.UserId.HasValue && IsCommentatorAuthorOfPost(comment, post))
        {
            return; //don't send emails to me again.
        }

        if (post.UserId is not null)
        {
            await emailsFactoryService.SendEmailToIdAsync<PostReplyToWritersEmail, PostReplyToWritersEmailModel>(
                Invariant($"PostReply/CommentId/{comment.Id}"), inReplyTo: "",
                Invariant($"PostReply/BlogPostId/{comment.ParentId}"), new PostReplyToWritersEmailModel
                {
                    Title = post.Title,
                    Username = comment.GuestUser.UserName,
                    Body = comment.Body,
                    PmId = comment.ParentId.ToString(CultureInfo.InvariantCulture),
                    CommentId = comment.Id.ToString(CultureInfo.InvariantCulture)
                }, post.UserId.Value, $"پاسخ به : {post.Title}");
        }
    }

    private static bool IsAnonymousUser(BlogPostComment replyToComment) => replyToComment.UserId is null;

    private static bool IsCommentatorAuthorOfPost(BlogPostComment comment, BlogPost post)
        => post.UserId is not null && comment.UserId is not null && comment.UserId.Value == post.UserId.Value;

    private static bool IsCommentatorMe(BlogPostComment comment, BlogPostComment replyToComment)
        => replyToComment.UserId is not null && comment.UserId.HasValue &&
           replyToComment.UserId.Value == comment.UserId.Value;
}
