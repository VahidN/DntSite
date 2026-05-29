using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;

namespace DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;

public static class QuestionsMappersExtensions
{
    public const string StackExchangeQuestionTags = $"{nameof(StackExchangeQuestion)}_Tags";

    public static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(QuestionsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this StackExchangeQuestionComment item,
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
            Title = $"{WhatsNewItemType.QuestionsComments.Value}: {item.Parent.Title}",
            OriginalTitle = item.Parent.Title,
            Url = siteRootUri.CombineUrl(
                string.Create(CultureInfo.InvariantCulture,
                    $"{QuestionsRoutingConstants.QuestionsDetailsBase}/{item.ParentId}#comment-{item.Id}"),
                escapeRelativeUrl: false),
            Categories = [],
            ItemType = WhatsNewItemType.QuestionsComments,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static WhatsNewItemModel MapToWhatsNewItemModel(this StackExchangeQuestion item,
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
            Title = $"{WhatsNewItemType.Questions.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = [..item.Tags.Select(x => x.Name)],
            ItemType = WhatsNewItemType.Questions,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }

    public static StackExchangeQuestion MapQuestionModelToStackExchangeQuestion(this QuestionModel source,
        IAppAntiXssService antiXssService,
        StackExchangeQuestion? destination = null)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(antiXssService);

        var stackExchangeQuestion = new StackExchangeQuestion
        {
            Title = source.Title,
            Description = antiXssService.GetSanitizedHtml(source.Description)
        };

        if (destination is not null)
        {
            destination.Title = stackExchangeQuestion.Title;
            destination.Description = stackExchangeQuestion.Description;
        }

        return destination ?? stackExchangeQuestion;
    }

    public static QuestionModel MapStackExchangeQuestionToQuestionModel(this StackExchangeQuestion source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new QuestionModel
        {
            Title = source.Title,
            Description = source.Description,
            Tags = source.Tags?.Select(tag => tag.Name).ToList() ?? []
        };
    }
}
