namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public class AffectedColumn
{
    public string Name { set; get; } = default!;

    public string? Value { set; get; }
}
