namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

public abstract class ParentVisitorEntity : BaseEntity
{
    public VisitorUserAgent UserAgent { set; get; } = new();

    public VisitorOs Os { get; set; } = new();

    public VisitorDevice Device { get; set; } = new();

    public VisitorCountry Country { get; set; } = new();

    public VisitorReferrer Referrer { set; get; } = new();

    public bool IsFromFeed { set; get; }
}
