using System.Text;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Courses.ModelsMappings;

public static class CourseMappersExtensions
{
    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(CoursesRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this CourseTopicComment item, string siteRootUri)
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
            Title = $"{WhatsNewItemType.CourseTopicsReplies.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{CoursesRoutingConstants.CoursesTopicBase}/{item.Parent.CourseId}/{item.Parent.DisplayId:D}#comment-{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.CourseTopicsReplies,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this CourseTopic item, string siteRootUri)
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
            Title = $"{WhatsNewItemType.AllCoursesTopics.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{CoursesRoutingConstants.CoursesTopicBase}/{item.CourseId}/{item.DisplayId:D}"),
                escapeRelativeUrl: false),
            Categories = item.Course.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.AllCoursesTopics,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this Course item, string siteRootUri)
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
            Title = $"{WhatsNewItemType.AllCourses.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.AllCourses,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }
}
