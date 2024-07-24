namespace DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

[ComplexType]
public class VisitorReferrer
{
    [StringLength(1500)] public string? Title { set; get; }

    [StringLength(1500)] public string? Url { set; get; }

    [StringLength(1500)] public string? OriginalUrl { set; get; }

    [StringLength(1500)] public string? OriginalTitle { set; get; }
}
