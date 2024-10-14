using System.Text;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;

namespace DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;

public static class QuestionsMappersExtensions
{
    private static readonly CompositeFormat ParsedPostUrlTemplate =
        CompositeFormat.Parse(QuestionsRoutingConstants.PostUrlTemplate);

    public static WhatsNewItemModel MapToWhatsNewItemModel(this StackExchangeQuestionComment item, string siteRootUri)
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

    public static WhatsNewItemModel MapToWhatsNewItemModel(this StackExchangeQuestion item, string siteRootUri)
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
            Title = $"{WhatsNewItemType.Questions.Value}: {item.Title}",
            OriginalTitle = item.Title,
            Url = siteRootUri.CombineUrl(string.Format(CultureInfo.InvariantCulture, ParsedPostUrlTemplate, item.Id),
                escapeRelativeUrl: false),
            Categories = item.Tags.Select(x => x.Name),
            ItemType = WhatsNewItemType.Questions,
            Id = item.Id,
            UserId = item.UserId,
            EntityType = item.GetType()
        };
    }
}
