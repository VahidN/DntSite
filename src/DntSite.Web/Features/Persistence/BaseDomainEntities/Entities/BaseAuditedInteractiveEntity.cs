using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseAuditedInteractiveEntity : BaseAuditedEntity
{
    [IgnoreAudit] public Rating Rating { set; get; } = new();

    [IgnoreAudit] public EntityStat EntityStat { set; get; } = new();
}
