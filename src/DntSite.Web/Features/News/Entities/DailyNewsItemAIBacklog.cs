using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.News.Entities;

public class DailyNewsItemAIBacklog : BaseAuditedEntity
{
    public required string Url { set; get; }

    [IgnoreAudit] public string UrlHash { set; get; } = default!;

    public string? Title { set; get; }

    public bool IsApproved { set; get; }

    public bool IsProcessed { set; get; }

    [IgnoreAudit] public int FetchRetries { set; get; }

    public virtual DailyNewsItem? DailyNewsItem { set; get; }

    public int? DailyNewsItemId { set; get; }
}
