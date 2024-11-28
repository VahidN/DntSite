using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISiteStatService : IScopedService
{
    public Task<PagedResultModel<User>> GetPagedTodayVisitedUsersListAsync(int pageNumber, int recordsPerPage);

    public Task<List<SiteStatsModel>> GetSiteStatAsync();
}
