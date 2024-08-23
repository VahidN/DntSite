using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.RssFeeds.Models;

public class WhatsNewItemModel : FeedItem
{
    public required int Id { set; get; }

    public required string OriginalTitle { set; get; }

    public required WhatsNewItemType ItemType { set; get; }

    public User? User { set; get; }
}
