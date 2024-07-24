using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.UserProfiles.Entities;

public class UserUsedPassword : BaseAuditedEntity
{
    [StringLength(1000)] [IgnoreAudit] public required string HashedPassword { get; set; }
}
