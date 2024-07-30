namespace DntSite.Web.Features.RssFeeds.Models;

public class WhatsNewFeedChannel : FeedChannel
{
    public new IEnumerable<WhatsNewItemModel> RssItems { set; get; } = [];
}
