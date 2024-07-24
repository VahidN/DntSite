using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.SideBar.Entities;

public class CustomSidebar : BaseAuditedEntity
{
    [MaxLength] public string? Description { set; get; }

    public bool IsPublic { set; get; }
}
