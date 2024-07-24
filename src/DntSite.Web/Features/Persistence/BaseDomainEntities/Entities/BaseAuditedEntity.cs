using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class BaseAuditedEntity : BaseEntity
{
    // NOTE: We don't need CreatedAt, etc here, because `x.AuditActions[0].At` is equal to CreatedAt, etc.

    // NOTE: This must be exactly a List<>!
    // If we use an ICollection<> here, we won't be able to use `indexing` on post.AuditActions[0].UserId
    // If we use an IList<> here, we should use AsNoTracking() with queries.
    [MaxLength] [IgnoreAudit] public List<AuditAction> AuditActions { get; set; } = new();
}
