using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Stats.Entities;

public class SiteReferrer : BaseEntity
{
    [IgnoreAudit] public DateTime LastVisitTime { set; get; }

    public string ReferrerTitle { set; get; } = null!;

    public string ReferrerUrl { set; get; } = null!;

    [IgnoreAudit] public string VisitHash { set; get; } = null!;

    [IgnoreAudit] public int VisitsCount { set; get; }

    public bool IsLocalReferrer { set; get; }

    public virtual SiteUrl? DestinationSiteUrl { set; get; }

    public int? DestinationSiteUrlId { set; get; }
}
