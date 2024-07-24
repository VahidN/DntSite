namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseBookmarkEntity<TForeignKeyEntity> : ParentBookmarkEntity
    where TForeignKeyEntity : BaseAuditedEntity
{
    public virtual TForeignKeyEntity Parent { set; get; } = default!;

    public int ParentId { set; get; }
}
