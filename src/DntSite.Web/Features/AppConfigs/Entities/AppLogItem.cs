using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.AppConfigs.Entities;

[IgnoreSoftDelete]
public class AppLogItem : BaseAuditedEntity
{
    [IgnoreAudit] public int EventId { get; set; }

    [IgnoreAudit] public required string Url { get; set; }

    [IgnoreAudit] public required string LogLevel { get; set; }

    [IgnoreAudit] public required string Logger { get; set; }

    [IgnoreAudit] public required string Message { get; set; }
}
