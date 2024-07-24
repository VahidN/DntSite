namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class VisitorOs
{
    [StringLength(1000)] public string? Family { get; set; }

    [StringLength(1000)] public string? Major { get; set; }

    [StringLength(1000)] public string? Minor { get; set; }

    [StringLength(1000)] public string? Patch { get; set; }

    [StringLength(1000)] public string? PatchMinor { get; set; }
}
