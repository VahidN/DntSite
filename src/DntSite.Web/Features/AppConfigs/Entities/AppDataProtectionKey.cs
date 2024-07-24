using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.AppConfigs.Entities;

public class AppDataProtectionKey : BaseAuditedEntity
{
    public required string FriendlyName { get; set; }

    [IgnoreAudit] public required string XmlData { get; set; }
}
