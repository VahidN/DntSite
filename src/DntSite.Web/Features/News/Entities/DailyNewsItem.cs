using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.News.Entities;

public class DailyNewsItem : BaseInteractiveEntity<DailyNewsItem, DailyNewsItemVisitor, DailyNewsItemBookmark,
    DailyNewsItemReaction, DailyNewsItemTag, DailyNewsItemComment, DailyNewsItemCommentVisitor,
    DailyNewsItemCommentBookmark, DailyNewsItemCommentReaction, DailyNewsItemUserFile, DailyNewsItemUserFileVisitor>
{
    public required string Url { set; get; }

    [IgnoreAudit] public string UrlHash { set; get; } = default!;

    public required string Title { set; get; }

    [MaxLength] public string? BriefDescription { set; get; }

    public string? PageThumbnail { set; get; }

    public bool PingbackSent { set; get; }

    public HttpStatusCode? LastHttpStatusCode { set; get; }

    public DateTime? LastHttpStatusCodeCheckDateTime { set; get; }
}
