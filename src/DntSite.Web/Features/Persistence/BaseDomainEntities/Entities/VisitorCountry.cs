namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class VisitorCountry
{
    [StringLength(1000)] public string? Code { get; set; }

    [StringLength(1000)] public string? Name { get; set; }
}
