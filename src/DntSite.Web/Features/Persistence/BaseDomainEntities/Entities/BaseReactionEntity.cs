using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[IgnoreSoftDelete]
public abstract class BaseReactionEntity<TForeignKeyEntity> : ParentReactionEntity
    where TForeignKeyEntity : BaseAuditedEntity
{
    public virtual TForeignKeyEntity Parent { set; get; } = default!;

    public int ParentId { set; get; }
}
