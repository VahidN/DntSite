namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseInteractiveEntity<TSelfEntity, TVisitorEntity, TBookmarkEntity, TReactionEntity, TTagEntity,
    TCommentsEntity, TCommentsVisitorEntity, TCommentsBookmarkEntity, TCommentsReactionEntity, TUserFileEntity,
    TUserFileVisitorEntity> : BaseAuditedInteractiveEntity
    where TSelfEntity : BaseAuditedInteractiveEntity
    where TUserFileEntity : BaseUserFileEntity<TUserFileEntity, TSelfEntity, TUserFileVisitorEntity>
    where TUserFileVisitorEntity : BaseVisitorEntity<TUserFileEntity>
    where TVisitorEntity : BaseVisitorEntity<TSelfEntity>
    where TCommentsEntity : BaseCommentsEntity<TCommentsEntity, TSelfEntity, TCommentsVisitorEntity,
        TCommentsBookmarkEntity, TCommentsReactionEntity>
    where TBookmarkEntity : BaseBookmarkEntity<TSelfEntity>
    where TReactionEntity : BaseReactionEntity<TSelfEntity>
    where TTagEntity : BaseTagEntity<TSelfEntity>
    where TCommentsVisitorEntity : BaseVisitorEntity<TCommentsEntity>
    where TCommentsBookmarkEntity : BaseBookmarkEntity<TCommentsEntity>
    where TCommentsReactionEntity : BaseReactionEntity<TCommentsEntity>
{
    public virtual ICollection<TCommentsEntity> Comments { set; get; } = [];

    public virtual ICollection<TTagEntity> Tags { set; get; } = [];

    public virtual ICollection<TReactionEntity> Reactions { set; get; } = [];

    public virtual ICollection<TBookmarkEntity> Bookmarks { set; get; } = [];

    public virtual IEnumerable<TVisitorEntity> Visitors { set; get; } = [];

    public virtual IEnumerable<TUserFileEntity> UserFiles { set; get; } = [];
}
