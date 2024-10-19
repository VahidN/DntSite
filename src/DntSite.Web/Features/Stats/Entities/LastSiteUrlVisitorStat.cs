using UAParser;

namespace DntSite.Web.Features.Stats.Entities;

[ComplexType]
public class LastSiteUrlVisitorStat
{
    public DateTime VisitTime { set; get; } = default!;

    [StringLength(maximumLength: 100)] public string Ip { set; get; } = default!;

    [StringLength(maximumLength: 1000)] public string UserAgent { set; get; } = default!;

    [StringLength(maximumLength: 1000)] public string? DisplayName { set; get; }

    public bool IsSpider { set; get; }

    [NotMapped] public ClientInfo? ClientInfo { set; get; }
}
