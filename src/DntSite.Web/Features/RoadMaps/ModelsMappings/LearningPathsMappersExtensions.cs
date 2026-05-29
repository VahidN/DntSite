using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.RoadMaps.ModelsMappings;

public static class LearningPathsMappersExtensions
{
    public const string LearningPathTags = $"{nameof(LearningPath)}_Tags";

    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(RoadMapsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this LearningPath item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? item.Description.GetBriefDescription(charLength: 450) : item.Description,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.LearningPaths.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = [..item.Tags.Select(x => x.Name)],
            ItemType = WhatsNewItemType.LearningPaths,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static LearningPath MapLearningPathModelToLearningPath(this LearningPathModel source,
        IAppAntiXssService antiXssService,
        LearningPath? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var learningPath = new LearningPath
        {
            Title = source.Title,
            Description = antiXssService.GetSanitizedHtml(source.Description)
        };

        if (destination is not null)
        {
            destination.Title = learningPath.Title;
            destination.Description = learningPath.Description;
        }

        return destination ?? learningPath;
    }

    public static LearningPathModel MapLearningPathToLearningPathModel(this LearningPath source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new LearningPathModel
        {
            Title = source.Title,
            Description = source.Description,
            Tags = source.Tags?.Select(tag => tag.Name).ToList() ?? []
        };
    }
}
