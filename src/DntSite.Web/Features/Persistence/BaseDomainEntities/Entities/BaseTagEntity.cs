using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseTagEntity<TAssociatedEntity> : BaseAuditedEntity
    where TAssociatedEntity : BaseAuditedEntity
{
    public string Name { set; get; } = default!;

    [IgnoreAudit] public int InUseCount { set; get; }

    public virtual ICollection<TAssociatedEntity> AssociatedEntities { set; get; } = new List<TAssociatedEntity>();
}
