using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class ParentUserFileEntity : BaseEntity
{
    [StringLength(1000)] public required string FileName { get; set; }

    public long FileSize { get; set; }

    [IgnoreAudit] public int NumberOfDownloads { get; set; }
}
