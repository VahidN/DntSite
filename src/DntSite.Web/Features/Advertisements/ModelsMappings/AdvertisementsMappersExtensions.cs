using System.Text;
using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.DateTimeToolkit;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Advertisements.ModelsMappings;

public static class AdvertisementsMappersExtensions
{
    public const string AdvertisementTags = $"{nameof(Advertisement)}_Tags";

    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(AdvertisementsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this Advertisement item,
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
            Title = $"{WhatsNewItemType.AllAdvertisements.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.AllAdvertisements,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this AdvertisementComment item,
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
            Title = $"{WhatsNewItemType.AdvertisementComments.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{AdvertisementsRoutingConstants.AdvertisementsDetailsBase}/{item.ParentId}#comment-{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.AdvertisementComments,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static Advertisement MapWriteAdvertisementModelToAdvertisement(this WriteAdvertisementModel source,
        IAppAntiXssService antiXssService,
        Advertisement? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var advertisement = new Advertisement
        {
            Title = source.Title,
            Body = antiXssService.GetSanitizedHtml(source.Body),
            DueDate = source.DueDate.CombineDateWithTime(source.Hour ?? 0, source.Minute ?? 0)
        };

        if (destination is not null)
        {
            destination.Title = advertisement.Title;
            destination.Body = advertisement.Body;
            destination.DueDate = advertisement.DueDate;
        }

        return destination ?? advertisement;
    }

    public static WriteAdvertisementModel MapAdvertisementToWriteAdvertisementModel(this Advertisement source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new WriteAdvertisementModel
        {
            Title = source.Title,
            Body = source.Body,
            DueDate = source.DueDate ?? DateTime.UtcNow,
            Tags = source.Tags?.Select(tag => tag.Name).ToList() ?? [],
            Hour = source.DueDate.GetHour(),
            Minute = source.DueDate.GetMinute()
        };
    }
}
