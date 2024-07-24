using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class SiteStatService(IUnitOfWork uow) : ISiteStatService
{
    private readonly DbSet<ParentVisitorEntity> _parentVisitors = uow.DbSet<ParentVisitorEntity>();

    public async Task<List<SiteStatsModel>> GetSiteStatAsync()
    {
        var results = new List<SiteStatsModel>();

        var now = DateTime.UtcNow;

        foreach (SiteStatsDate siteStatsDate in Enum.GetValues(typeof(SiteStatsDate)))
        {
            var persianDateRange = "";
            var subStringLength = 0;

            switch (siteStatsDate)
            {
                case SiteStatsDate.Today:
                    persianDateRange = now.ToShortPersianDateString();
                    subStringLength = 10;

                    break;
                case SiteStatsDate.Yesterday:
                    persianDateRange = now.AddDays(value: -1).ToShortPersianDateString();
                    subStringLength = 10;

                    break;
                case SiteStatsDate.ThisMonth:
                    persianDateRange = now.ToShortPersianDateString()[..7];
                    subStringLength = 7;

                    break;
                case SiteStatsDate.ThisYear:
                    persianDateRange = now.ToShortPersianDateString()[..4];
                    subStringLength = 4;

                    break;
                case SiteStatsDate.LastMonth:
                    break;
                case SiteStatsDate.LastYear:
                    break;
                case SiteStatsDate.FromTheBeginning:
                    break;
                default:
                    continue;
            }

            var query = _parentVisitors.AsNoTracking()
                .Where(visitor => visitor.Audit.CreatedAtPersian.Substring(0, subStringLength) == persianDateRange);

            results.Add(new SiteStatsModel
            {
                SiteStatsDate = siteStatsDate,
                UniqueVisitorsCount =
                    await query.GroupBy(visitor => visitor.Audit.CreatedByUserIp)
                        .Select(grouping => grouping.Key)
                        .CountAsync(),
                VisitorsCount = await query.Select(visitor => visitor.Audit.CreatedByUserIp).Distinct().CountAsync(),
                VisitsCount = await query.CountAsync()
            });
        }

        return results;
    }
}
