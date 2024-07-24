namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseVisitorEntity<TForeignKeyEntity> : ParentVisitorEntity
    where TForeignKeyEntity : BaseEntity
{
    public virtual TForeignKeyEntity Parent { set; get; } = default!;

    public int ParentId { set; get; }
}
