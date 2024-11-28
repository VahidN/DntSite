using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.Searches.Models;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface IFullTextSearchService : IDisposable, ISingletonService
{
    public bool IsDatabaseIndexed { get; }

    public void DeleteOldIndexFiles();

    public LuceneSearchResult? FindLuceneDocument(string? documentTypeIdHash);

    public void DeleteLuceneDocument(string? documentTypeIdHash);

    public void AddOrUpdateLuceneDocument(WhatsNewItemModel? item);

    public Task IndexTableAsync(IEnumerable<WhatsNewItemModel>? items, bool commitChanges = true);

    public void CommitChanges();

    public PagedResultModel<LuceneSearchResult> FindPagedPosts(string? searchText,
        int maxItems,
        int pageNumber,
        int pageSize,
        params string[]? searchInTheseFieldNames);

    public PagedResultModel<LuceneSearchResult> FindPagedSimilarPosts(string documentTypeIdHash,
        int maxItems,
        int pageNumber,
        int pageSize,
        params string[]? moreLikeTheseFieldNames);

    public int GetNumberOfDocuments();

    public PagedResultModel<LuceneSearchResult> GetAllPagedPosts(int pageNumber,
        int pageSize,
        string sortField,
        bool isDescending);
}
