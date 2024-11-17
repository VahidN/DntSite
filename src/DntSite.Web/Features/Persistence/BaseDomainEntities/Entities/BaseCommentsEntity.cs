namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseCommentsEntity<TSelfEntity, TForeignKeyEntity, TVisitorEntity, TBookmarkEntity,
    TReactionEntity> : BaseSelfReferencingEntity<TSelfEntity>
    where TSelfEntity : BaseCommentsEntity<TSelfEntity, TForeignKeyEntity, TVisitorEntity, TBookmarkEntity,
        TReactionEntity>
    where TForeignKeyEntity : BaseAuditedInteractiveEntity
    where TVisitorEntity : BaseVisitorEntity<TSelfEntity>
    where TBookmarkEntity : BaseBookmarkEntity<TSelfEntity>
    where TReactionEntity : BaseReactionEntity<TSelfEntity>
{
    [MaxLength] public required string Body { set; get; }

    public virtual TForeignKeyEntity Parent { set; get; } = default!;

    public int ParentId { set; get; }

    public virtual ICollection<TReactionEntity> Reactions { set; get; } = [];

    public virtual ICollection<TBookmarkEntity> Bookmarks { set; get; } = [];

    public virtual ICollection<TVisitorEntity> Visitors { set; get; } = [];
}
