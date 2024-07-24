using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class AuditBase
{
    [IgnoreAudit] public DateTime CreatedAt { set; get; }

    [IgnoreAudit] [StringLength(50)] public string CreatedAtPersian { set; get; } = default!;

    [IgnoreAudit] [StringLength(50)] public string CreatedByUserIp { set; get; } = default!;

    [IgnoreAudit] [StringLength(1000)] public string CreatedByUserAgent { set; get; } = default!;
}
