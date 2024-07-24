namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseSelfReferencingEntity<TSelfEntity> : BaseAuditedInteractiveEntity
    where TSelfEntity : BaseAuditedInteractiveEntity
{
    public virtual TSelfEntity? Reply { set; get; }

    public int? ReplyId { get; set; }

    public virtual ICollection<TSelfEntity>? Children { get; set; }
}
