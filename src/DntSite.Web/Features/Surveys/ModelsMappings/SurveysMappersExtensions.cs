using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.DateTimeToolkit;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.Surveys.RoutingConstants;

namespace DntSite.Web.Features.Surveys.ModelsMappings;

public static class SurveysMappersExtensions
{
    public const string SurveyTags = $"{nameof(Survey)}_Tags";

    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(SurveysRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this Survey item,
        string siteRootUri,
        bool showBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(item);

        var content = item.SurveyItems.Any(x => !x.IsDeleted)
            ? item.SurveyItems.Where(x => !x.IsDeleted).Select(x => x.Title).Aggregate((s1, s2) => s1 + "<br/>" + s2)
            : "";

        return new WhatsNewItemModel
        {
            User = item.User,
            AuthorName = item.User?.FriendlyName ?? item.GuestUser.UserName,
            Content = showBriefDescription ? content.GetBriefDescription(charLength: 450) : content,
            PublishDate = new DateTimeOffset(item.Audit.CreatedAt),
            LastUpdatedTime =
                new DateTimeOffset(item.AuditActions.Count > 0
                    ? item.AuditActions[^1].CreatedAt
                    : item.Audit.CreatedAt),
            Title = $"{WhatsNewItemType.AllVotes.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = [..item.Tags.Select(x => x.Name)],
            ItemType = WhatsNewItemType.AllVotes,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this SurveyComment item,
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
            Title = $"{WhatsNewItemType.VotesReplies.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{SurveysRoutingConstants.SurveysArchiveDetailsBase}/{item.ParentId}#comment-{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.VotesReplies,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static Survey MapVoteModelToSurvey(this VoteModel source,
        IAppAntiXssService antiXssService,
        Survey? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var dueDate = source.ExpirationDate.CombineDateWithTime(source.Hour ?? 0, source.Minute ?? 0);

        var survey = new Survey
        {
            Title = source.Title,
            Description = antiXssService.GetSanitizedHtml(source.Description),
            DueDate = dueDate
        };

        if (destination is not null)
        {
            destination.Title = survey.Title;
            destination.Description = survey.Description;
            destination.DueDate = survey.DueDate;
        }

        return destination ?? survey;
    }

    public static VoteModel MapSurveyToVoteModel(this Survey source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var dueDate = source.DueDate?.ToIranTimeZoneDateTime();

        return new VoteModel
        {
            Title = source.Title,
            Description = source.Description,
            ExpirationDate = dueDate,
            Hour = dueDate?.Hour,
            Minute = dueDate?.Minute,
            VoteItems = source.SurveyItems.Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => x.Title)
                .ToList()
                .ConvertListToMultiLineText(),
            Tags = source.Tags?.Select(tag => tag.Name).ToList() ?? []
        };
    }
}
