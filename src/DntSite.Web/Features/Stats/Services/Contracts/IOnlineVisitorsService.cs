using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IOnlineVisitorsService : ISingletonService, IDisposable
{
    OnlineVisitorsInfoModel GetOnlineVisitorsInfo();

    Task UpdateStatAsync(HttpContext context);

    PagedResultModel<OnlineVisitorInfoModel> GetPagedOnlineVisitorsList(int pageNumber,
        int recordsPerPage,
        bool isSpider);
}
