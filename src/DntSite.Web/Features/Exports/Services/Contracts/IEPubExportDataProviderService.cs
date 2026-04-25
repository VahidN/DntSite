using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Exports.Models;

namespace DntSite.Web.Features.Exports.Services.Contracts;

public interface IEPubExportDataProviderService : IScopedService
{
    Task<EPubTocItems> GetEPubTocItemsAsync(CancellationToken cancellationToken);

    Task<PagedResultModel<EPubListItem>> GetArticlesAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken);

    Task<PagedResultModel<EPubListItem>> GetAuthorsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken);

    Task<PagedResultModel<EPubListItem>> GetArticleGroupsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken);

    Task<PagedResultModel<EPubListItem>> GetLearningPathsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken);

    Task<PagedResultModel<EPubListItem>> GetNewsAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken);

    Task<PagedResultModel<EPubListItem>> GetCoursesAsync(int pageNumber,
        int recordsPerPage,
        CancellationToken cancellationToken);

    Task<List<EPubListItem>> GetAllCoursesAsync(CancellationToken cancellationToken);

    Task<List<EPubListItem>> GetAllArticlesAsync(CancellationToken cancellationToken);

    Task<List<EPubListItem>> GetAllNewsAsync(CancellationToken cancellationToken);
}
