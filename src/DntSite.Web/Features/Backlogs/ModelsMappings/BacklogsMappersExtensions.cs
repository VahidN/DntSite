using System.Text;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Backlogs.ModelsMappings;

public static class BacklogsMappersExtensions
{
    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(BacklogsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this Backlog item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.Backlogs.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.Backlogs,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }
}
