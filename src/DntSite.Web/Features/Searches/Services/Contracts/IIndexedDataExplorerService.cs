using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface IIndexedDataExplorerService : IScopedService
{
    public Task<PagedResultModel<LuceneSearchResult>> GetAllPagedIndexedDataAsync(int pageNumber, int pageSize);

    public Task<PagedResultModel<LuceneSearchResult>> FindAllPagedIndexedDataAsync(string term,
        int maxItems,
        int pageNumber,
        int pageSize);

    public Task<PagedResultModel<LuceneSearchResult>> FindAllMoreLikeThisItemsAsync(string documentHashId,
        int maxItems,
        int pageNumber,
        int pageSize);
}
