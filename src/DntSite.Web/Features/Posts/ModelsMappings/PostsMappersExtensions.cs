using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Posts.ModelsMappings;

public static class PostsMappersExtensions
{
    public const string BlogPostTags = $"{nameof(BlogPost)}_Tags";

    public static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(PostsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this BlogPostComment item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
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

    public static WhatsNewItemModel MapToAuthorWhatsNewItemModel(this BlogPost item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
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
            Categories = [..item.Tags.Select(x => x.Name)],
            ItemType = WhatsNewItemType.Author,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToTagWhatsNewItemModel(this BlogPost item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
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
            Categories = [..item.Tags.Select(x => x.Name)],
            ItemType = WhatsNewItemType.Tag,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToPostWhatsNewItemModel(this BlogPost item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Body.GetBriefDescription(charLength: 450) : item.Body,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.Posts.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = [..item.Tags.Select(x => x.Name)],
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

    public static BlogPost MapWriteArticleModelToBlogPost(this WriteArticleModel source,
        IAppAntiXssService antiXssService,
        BlogPost? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var body = source.ArticleBody;

        var blogPost = new BlogPost
        {
            Title = source.Title,
            Body = antiXssService.GetSanitizedHtml(body),
            BriefDescription = body.GetBriefDescription(charLength: 450),
            ReadingTimeMinutes = body.MinReadTime(wordsPerMinute: 180)
        };

        if (destination is not null)
        {
            destination.Title = blogPost.Title;
            destination.Body = blogPost.Body;
            destination.BriefDescription = blogPost.BriefDescription;
            destination.ReadingTimeMinutes = blogPost.ReadingTimeMinutes;
        }

        return destination ?? blogPost;
    }

    public static WriteArticleModel MapBlogPostToWriteArticleModel(this BlogPost source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new WriteArticleModel
        {
            Title = source.Title,
            ArticleBody = source.Body,
            ArticleTags = source.Tags?.Select(tag => tag.Name).ToList() ?? []
        };
    }

    public static BlogPostDraft MapWriteDraftModelToBlogPostDraft(this WriteDraftModel source,
        IAppAntiXssService antiXssService,
        ICurrentUserService currentUserService,
        BlogPostDraft? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);
        ArgumentNullException.ThrowIfNull(currentUserService);

        var dateTimeToShow = new PersianDateTime(source.PersianDateYear, source.PersianDateMonth, source.PersianDateDay,
            source.Hour, source.Minute, second: 0).DateTimeUtc;

        var body = source.ArticleBody;

        var draft = new BlogPostDraft
        {
            Title = source.Title,
            Body = antiXssService.GetSanitizedHtml(body),
            IsConverted = false,
            ReadingTimeMinutes = body.MinReadTime(wordsPerMinute: 180),
            DateTimeToShow = dateTimeToShow,
            UserId = currentUserService.GetCurrentUserId(),
            Tags = []
        };

        if (!currentUserService.IsCurrentUserAdmin())
        {
            draft.DateTimeToShow = null;
        }

        if (destination is not null)
        {
            destination.Title = draft.Title;
            destination.Body = draft.Body;
            destination.IsConverted = draft.IsConverted;
            destination.ReadingTimeMinutes = draft.ReadingTimeMinutes;
            destination.DateTimeToShow = draft.DateTimeToShow;
            destination.UserId = draft.UserId;
            destination.DateTimeToShow = draft.DateTimeToShow;
        }

        return destination ?? draft;
    }

    public static WriteDraftModel MapBlogPostDraftToWriteDraftModel(this BlogPostDraft source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var dateTime = source.DateTimeToShow ?? DateTime.UtcNow;
        var iranDate = dateTime.ToIranTimeZoneDateTime();
        var persianDate = dateTime.ToPersianYearMonthDay();

        return new WriteDraftModel
        {
            Title = source.Title,
            ArticleBody = source.Body,
            ReadingTimeMinutes = source.Body.MinReadTime(wordsPerMinute: 180),
            Hour = source.DateTimeToShow == null ? 23 : iranDate.Hour,
            Minute = source.DateTimeToShow == null ? 55 : iranDate.Minute,
            PersianDateYear = persianDate?.Year ?? 0,
            PersianDateMonth = persianDate?.Month ?? 1,
            PersianDateDay = persianDate?.Day ?? 1
        };
    }
}
