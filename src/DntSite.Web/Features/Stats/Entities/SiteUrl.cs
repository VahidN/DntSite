using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Stats.Entities;

public class SiteUrl : BaseEntity
{
    [IgnoreAudit] public LastSiteUrlVisitorStat LastSiteUrlVisitorStat { get; set; } = new();

    public string Title { set; get; } = default!;

    public string Url { set; get; } = default!;

    public int VisitsCount { set; get; }

    [IgnoreAudit] public string UrlHash { set; get; } = default!;

    [IgnoreAudit] public bool IsProtectedPage { set; get; }

    [IgnoreAudit] public bool IsStaticFileUrl { set; get; }

    [NotMapped] public bool IsHidden => IsStaticFileUrl || IsDeleted || IsProtectedPage || Title.IsEmpty();
}
