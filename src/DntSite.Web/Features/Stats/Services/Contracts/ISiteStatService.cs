using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISiteStatService : IScopedService
{
    Task<List<SiteStatsModel>> GetSiteStatAsync();
}
