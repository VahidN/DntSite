using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostsEmailsService : IScopedService
{
    Task DraftConvertedEmailAsync(BlogPost? blogPost);

    Task WriteArticleSendEmailAsync(BlogPost? blogPost);

    Task WriteDraftSendEmailAsync(BlogPostDraft? blogPost);

    Task DeleteDraftSendEmailAsync(BlogPostDraft? blogPost);
}
