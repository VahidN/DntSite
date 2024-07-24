namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class VisitorDevice
{
    public bool IsSpider { set; get; }

    [StringLength(1000)] public string? Brand { get; set; }

    [StringLength(1000)] public string? Family { get; set; }

    [StringLength(1000)] public string? Model { get; set; }
}
