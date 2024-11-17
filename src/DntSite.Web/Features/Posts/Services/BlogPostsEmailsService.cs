using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Posts.EmailLayouts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.Services;

public class BlogPostsEmailsService(ICommonService commonService, IEmailsFactoryService emailsFactoryService)
    : IBlogPostsEmailsService
{
    public async Task DraftConvertedEmailAsync(BlogPost? blogPost)
    {
        if (blogPost is null)
        {
            return;
        }

        var emails = (await commonService.GetAllActiveAdminsAsNoTrackingAsync()).Select(x => x.EMail).ToList();

        await emailsFactoryService.SendEmailToAllUsersAsync<NewConvertedBlogPost, NewConvertedBlogPostModel>(emails,
            messageId: "DraftConverted", inReplyTo: "", references: "DraftConverted", new NewConvertedBlogPostModel
            {
                Source = blogPost.Title,
                Dest = blogPost.Body
            }, $"مطلب جدید تبدیل شده: {blogPost.Title}", addIp: false);

        await emailsFactoryService.SendEmailAsync<NewConvertedBlogPost, NewConvertedBlogPostModel>(
            messageId: "DraftConverted", inReplyTo: "", references: "DraftConverted", new NewConvertedBlogPostModel
            {
                Source = blogPost.Title,
                Dest = blogPost.Body
            }, blogPost.User?.EMail, $"مطلب جدید تبدیل شده: {blogPost.Title}", addIp: false);
    }

    public Task WriteArticleSendEmailAsync(BlogPost? blogPost)
        => blogPost is null
            ? Task.CompletedTask
            : emailsFactoryService.SendEmailToAllAdminsAsync<WriteArticleEmail, WriteArticleEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"WriteArticle/{blogPost.Id}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"WriteArticle/{blogPost.Id}"), new WriteArticleEmailModel
                {
                    Title = blogPost.Title,
                    Body = blogPost.Body,
                    PmId = blogPost.Id.ToString(CultureInfo.InvariantCulture)
                }, $"مطلب جدید: {blogPost.Title}");

    public Task WriteDraftSendEmailAsync(BlogPostDraft? blogPost)
        => blogPost is null
            ? Task.CompletedTask
            : emailsFactoryService.SendEmailToAllAdminsAsync<NewDraftEmail, NewDraftEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"WriteDraft/{blogPost.Id}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"WriteDraft/{blogPost.Id}"), new NewDraftEmailModel
                {
                    Title = blogPost.Title,
                    Body = blogPost.Body,
                    PmId = blogPost.Id.ToString(CultureInfo.InvariantCulture),
                    IsReady = blogPost.IsReady ? "آماده انتشار" : "در حال ویرایش"
                }, $"پیش نویس جدید: {blogPost.Title}");

    public Task DeleteDraftSendEmailAsync(BlogPostDraft? blogPost)
        => blogPost is null
            ? Task.CompletedTask
            : emailsFactoryService.SendEmailToAllAdminsAsync<NewDraftDeletedEmail, NewDraftEmailModel>(
                string.Create(CultureInfo.InvariantCulture, $"DeleteDraft/{blogPost.Id}"), inReplyTo: "",
                string.Create(CultureInfo.InvariantCulture, $"DeleteDraft/{blogPost.Id}"), new NewDraftEmailModel
                {
                    Title = blogPost.Title,
                    Body = blogPost.Body,
                    PmId = blogPost.Id.ToString(CultureInfo.InvariantCulture),
                    IsReady = "حذف شده"
                }, $"حذف پیش نویس جدید: {blogPost.Title}");
}
