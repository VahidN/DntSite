using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.RssFeeds.Models;

public class WhatsNewItemModel : FeedItem
{
    public User? User { set; get; }
}
