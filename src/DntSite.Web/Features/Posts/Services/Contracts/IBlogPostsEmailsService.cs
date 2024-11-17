using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostsEmailsService : IScopedService
{
    public Task DraftConvertedEmailAsync(BlogPost? blogPost);

    public Task WriteArticleSendEmailAsync(BlogPost? blogPost);

    public Task WriteDraftSendEmailAsync(BlogPostDraft? blogPost);

    public Task DeleteDraftSendEmailAsync(BlogPostDraft? blogPost);
}
