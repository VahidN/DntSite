using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.PrivateMessages.Entities;

public class MassEmail : BaseAuditedEntity
{
    public required string NewsTitle { set; get; }

    [MaxLength] public required string NewsBody { set; get; }

    public bool EmailsSent { set; get; }
}
