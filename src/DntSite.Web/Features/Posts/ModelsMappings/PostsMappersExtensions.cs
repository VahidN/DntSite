using System.Text;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Posts.ModelsMappings;

public static class PostsMappersExtensions
{
    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(PostsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this BlogPostComment item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.Comments.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{PostsRoutingConstants.PostBase}/{item.ParentId}#comment-{item.Id}"), escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.Comments,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToAuthorWhatsNewItemModel(this BlogPost item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.Author.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture, $"{PostsRoutingConstants.PostBase}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.Author,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToTagWhatsNewItemModel(this BlogPost item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.Tag.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture, $"{PostsRoutingConstants.PostBase}/{item.Id}"),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.Tag,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToPostWhatsNewItemModel(this BlogPost item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.Posts.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.Posts,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToPostWhatsNewItemModel(this BlogPostDraft item, string siteRootUri)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = "به زودی ...",
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.AllDrafts.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(PostsRoutingConstants.ComingSoon2, escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.AllDrafts,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }
}
