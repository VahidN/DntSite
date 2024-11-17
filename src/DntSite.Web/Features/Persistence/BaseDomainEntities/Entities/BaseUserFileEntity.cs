namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseUserFileEntity<TSelfEntity, TForeignKeyEntity, TVisitorEntity> : ParentUserFileEntity
    where TSelfEntity : BaseUserFileEntity<TSelfEntity, TForeignKeyEntity, TVisitorEntity>
    where TForeignKeyEntity : BaseAuditedEntity
    where TVisitorEntity : BaseVisitorEntity<TSelfEntity>
{
    public virtual TForeignKeyEntity Parent { set; get; } = default!;

    public int ParentId { set; get; }

    public virtual ICollection<TVisitorEntity> Visitors { set; get; } = [];
}
