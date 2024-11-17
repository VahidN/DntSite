namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public class AuditAction : AuditBase
{
    public int? IdentityName { set; get; }

    public AuditActionType Action { set; get; }

    public List<AffectedColumn> AffectedColumns { get; set; } = [];
}
