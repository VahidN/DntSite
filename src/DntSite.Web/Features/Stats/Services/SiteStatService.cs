using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Stats.Services;

public class SiteStatService(IUnitOfWork uow) : ISiteStatService
{
    private readonly DbSet<ParentVisitorEntity> _parentVisitors = uow.DbSet<ParentVisitorEntity>();

    public Task<PagedResultModel<User>> GetPagedTodayVisitedUsersListAsync(int pageNumber, int recordsPerPage)
    {
        var today = DateTime.UtcNow;

        var query = uow.DbSet<User>()
            .AsNoTracking()
            .Where(x => x.IsActive && x.LastVisitDateTime.HasValue && x.LastVisitDateTime.Value.Year == today.Year &&
                        x.LastVisitDateTime.Value.Month == today.Month && x.LastVisitDateTime.Value.Day == today.Day)
            .OrderByDescending(x => x.LastVisitDateTime)
            .ThenBy(x => x.FriendlyName);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage);
    }

    public async Task<List<SiteStatsModel>> GetSiteStatAsync()
    {
        var results = new List<SiteStatsModel>();

        var now = DateTime.UtcNow;

        foreach (var siteStatsDate in Enum.GetValues<SiteStatsDate>())
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
                case SiteStatsDate.LastYear:
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
