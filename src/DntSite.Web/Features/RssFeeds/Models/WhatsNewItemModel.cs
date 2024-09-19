using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.RssFeeds.Models;

public class WhatsNewItemModel : FeedItem
{
    public required int Id { set; get; }

    public required string OriginalTitle { set; get; }

    public required WhatsNewItemType ItemType { set; get; }

    public User? User { set; get; }

    public required int? UserId { set; get; }

    public required Type? EntityType { set; get; }

    public string DocumentTypeIdHash
        => string.Create(CultureInfo.InvariantCulture, $"{Id}{ItemType.Value}").GetSha1Hash();

    public string DocumentContentHash => string.Create(CultureInfo.InvariantCulture,
            $"{Id}{ItemType.Value}{OriginalTitle}{Url}{PublishDate}{AuthorName}{Content}")
        .GetSha1Hash();
}
