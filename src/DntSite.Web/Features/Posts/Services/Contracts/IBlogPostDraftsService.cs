using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostDraftsService : IScopedService
{
    Task MarkAsDeletedAsync(BlogPostDraft? draft);

    Task<BlogPostDraft> AddBlogPostDraftAsync(WriteDraftModel? model);

    Task UpdateBlogPostDraftAsync(WriteDraftModel? model, BlogPostDraft draft);

    ValueTask<BlogPostDraft?> FindBlogPostDraftAsync(int id);

    Task<List<BlogPostDraft>> FindUsersNotConvertedBlogPostDraftsAsync(int userId);

    Task<List<BlogPostDraft>> FindAllNotConvertedBlogPostDraftsAsync();

    Task RunConvertDraftsToPostsJobAsync(CancellationToken cancellationToken);

    Task DeleteConvertedBlogPostDraftsAsync(CancellationToken cancellationToken);

    Task DeleteDraftAsync(BlogPostDraft draft);

    Task<List<BlogPostDraft>> ComingSoonItemsAsync();

    Task<BlogPostDraft?> FindBlogPostDraftIncludeUserAsync(int id);

    Task<OperationResult<DailyNewsItem?>> ConvertDraftToLinkAsync(DailyNewsItemModel data, int draftId);
}
