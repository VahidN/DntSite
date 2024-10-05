using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Stats.Entities;

public class SiteReferrer : BaseEntity
{
    [IgnoreAudit] public DateTime LastVisitTime { set; get; } = default!;

    public string ReferrerTitle { set; get; } = default!;

    public string ReferrerUrl { set; get; } = default!;

    public string DestinationUrl { set; get; } = default!;

    public string DestinationTitle { set; get; } = default!;

    [IgnoreAudit] public string VisitHash { set; get; } = default!;

    [IgnoreAudit] public int VisitsCount { set; get; }

    public bool IsLocalReferrer { set; get; }
}
