using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IOnlineVisitorsService : ISingletonService
{
    OnlineVisitorsInfoModel GetOnlineVisitorsInfo();

    Task ProcessItemAsync(OnlineVisitorInfoModel item);

    PagedResultModel<OnlineVisitorInfoModel> GetPagedOnlineVisitorsList(int pageNumber,
        int recordsPerPage,
        bool isSpider);
}
