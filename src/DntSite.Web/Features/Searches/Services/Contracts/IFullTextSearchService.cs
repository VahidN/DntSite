using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.Searches.Models;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface IFullTextSearchService : IDisposable, ISingletonService
{
    bool IsDatabaseIndexed { get; }

    void DeleteOldIndexFiles();

    LuceneSearchResult? FindLuceneDocument(string? documentTypeIdHash);

    void DeleteLuceneDocument(string? documentTypeIdHash);

    void AddOrUpdateLuceneDocument(WhatsNewItemModel? item);

    Task IndexTableAsync(IEnumerable<WhatsNewItemModel>? items, bool commitChanges = true);

    void CommitChanges();

    PagedResultModel<LuceneSearchResult> FindPagedPosts(string searchText,
        int maxItems,
        int pageNumber,
        int pageSize,
        params string[]? searchInTheseFieldNames);

    PagedResultModel<LuceneSearchResult> FindPagedSimilarPosts(string documentTypeIdHash,
        int maxItems,
        int pageNumber,
        int pageSize,
        params string[]? moreLikeTheseFieldNames);

    int GetNumberOfDocuments();

    PagedResultModel<LuceneSearchResult> GetAllPagedPosts(int pageNumber,
        int pageSize,
        string sortField,
        bool isDescending);
}
