using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Searches.Services;

public class IndexedDataExplorerService(
    IFullTextSearchService fullTextSearchService,
    IUsersInfoService usersInfoService) : IIndexedDataExplorerService
{
    public async Task<PagedResultModel<LuceneSearchResult>> GetAllPagedIndexedDataAsync(int pageNumber, int pageSize)
    {
        var posts = fullTextSearchService.GetAllPagedPosts(pageNumber, pageSize, nameof(WhatsNewItemModel.PublishDate),
            isDescending: true);

        if (posts.TotalItems == 0)
        {
            return new PagedResultModel<LuceneSearchResult>();
        }

        await UpdateUsersInfoAsync(posts);

        return posts;
    }

    public async Task<PagedResultModel<LuceneSearchResult>> FindAllPagedIndexedDataAsync(string term,
        int maxItems,
        int pageNumber,
        int pageSize)
    {
        var posts = fullTextSearchService.FindPagedPosts(term, maxItems, pageNumber, pageSize);

        if (posts.TotalItems == 0)
        {
            return new PagedResultModel<LuceneSearchResult>();
        }

        await UpdateUsersInfoAsync(posts);

        return posts;
    }

    public async Task<PagedResultModel<LuceneSearchResult>> FindAllMoreLikeThisItemsAsync(string documentHashId,
        int maxItems,
        int pageNumber,
        int pageSize)
    {
        var posts = fullTextSearchService.FindPagedSimilarPosts(documentHashId, maxItems, pageNumber, pageSize);

        if (posts.TotalItems == 0)
        {
            return new PagedResultModel<LuceneSearchResult>();
        }

        await UpdateUsersInfoAsync(posts);

        return posts;
    }

    private async Task UpdateUsersInfoAsync(PagedResultModel<LuceneSearchResult>? posts)
    {
        if (posts is null || posts.TotalItems == 0)
        {
            return;
        }

        var userIds = posts.Data.Where(x => x.UserId.HasValue).Select(x => x.UserId).ToList();

        var users = await usersInfoService.FindUsersAsync(userIds);

        foreach (var post in posts.Data)
        {
            post.User = users.Find(user => user.Id == post.UserId);
        }
    }
}
