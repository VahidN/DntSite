using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;

namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface IIndexedDataExplorerService : IScopedService
{
    Task<PagedResultModel<LuceneSearchResult>> GetAllPagedIndexedDataAsync(int pageNumber, int pageSize);

    Task UpdateUsersInfoAsync(PagedResultModel<LuceneSearchResult>? posts);
}
