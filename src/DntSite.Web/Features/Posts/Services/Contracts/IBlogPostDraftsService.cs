using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostDraftsService : IScopedService
{
    public Task MarkAsDeletedAsync(BlogPostDraft? draft);

    public Task<BlogPostDraft> AddBlogPostDraftAsync(WriteDraftModel model);

    public Task UpdateBlogPostDraftAsync(WriteDraftModel model, BlogPostDraft draft);

    public ValueTask<BlogPostDraft?> FindBlogPostDraftAsync(int id);

    public Task<List<BlogPostDraft>> FindUsersNotConvertedBlogPostDraftsAsync(int userId);

    public Task<List<BlogPostDraft>> FindAllNotConvertedBlogPostDraftsAsync();

    public Task RunConvertDraftsToPostsJobAsync(CancellationToken cancellationToken);

    public Task DeleteConvertedBlogPostDraftsAsync(CancellationToken cancellationToken);

    public Task DeleteDraftAsync(BlogPostDraft draft);

    public Task<List<BlogPostDraft>> ComingSoonItemsAsync();

    public Task<BlogPostDraft?> FindBlogPostDraftIncludeUserAsync(int id);

    public Task<OperationResult<DailyNewsItem?>> ConvertDraftToLinkAsync(DailyNewsItemModel data, int draftId);
}
