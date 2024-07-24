using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.News.Entities;

public class DailyNewsItemComment : BaseCommentsEntity<DailyNewsItemComment, DailyNewsItem, DailyNewsItemCommentVisitor,
    DailyNewsItemCommentBookmark, DailyNewsItemCommentReaction>
{
    public bool EmailsSent { set; get; }
}
