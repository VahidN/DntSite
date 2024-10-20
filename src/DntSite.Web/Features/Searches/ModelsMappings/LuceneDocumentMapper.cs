using System.Text;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.RssFeeds.Utils;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.Utils;
using Lucene.Net.Documents;

namespace DntSite.Web.Features.Searches.ModelsMappings;

public static class LuceneDocumentMapper
{
    private const string DateTimeFormat = "yyyyMMddHHmmss.fffzzzzz";
    private const char CategoriesSeparator = ':';

    public const string IndexedTitle = nameof(IndexedTitle);

    private static readonly CompositeFormat ParsedSearchResultTemplate =
        CompositeFormat.Parse(
            format: "<a class='{0}' rel='noopener noreferrer' href='{1}'><span class='{2} {3}'>{4}</span> {5}</a>");

    public static string MapToTemplatedResult(this LuceneSearchResult? result,
        string? searchQuery,
        string linkCssClass = "dropdown-item",
        string badgeCssClass = "badge rounded-pill")
        => result is null
            ? ""
            : string.Format(CultureInfo.InvariantCulture, ParsedSearchResultTemplate, linkCssClass, result.Url,
                badgeCssClass, result.ItemType.BgColor, result.ItemType.Value,
                result.OriginalTitle.ToHighlightedText(searchQuery));

    public static Document MapToLuceneDocument(this WhatsNewItemModel post)
    {
        ArgumentNullException.ThrowIfNull(post);

        Document document =
        [
            new TextField(IndexedTitle, post.ItemType.IsCommentOrReply() ? "" : post.OriginalTitle, Field.Store.YES)
            {
                Boost = 2
            },
            new TextField(nameof(WhatsNewItemModel.OriginalTitle), post.OriginalTitle, Field.Store.YES),
            new TextField(nameof(WhatsNewItemModel.Url), post.Url, Field.Store.YES),
            new TextField(nameof(WhatsNewItemModel.PublishDate),
                post.PublishDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture), Field.Store.YES),
            new TextField(nameof(WhatsNewItemModel.ItemType), post.ItemType.Value, Field.Store.YES),
            new TextField(nameof(WhatsNewItemModel.AuthorName), post.AuthorName, Field.Store.YES),
            new TextField(nameof(WhatsNewItemModel.Content), post.Content, Field.Store.YES),
            new TextField(nameof(WhatsNewItemModel.Categories), string.Join(CategoriesSeparator, post.Categories),
                Field.Store.YES),

            // Document StringField instances are sort of keywords, they are not analyzed, they indexed as is (in its original case).
            new StringField(nameof(WhatsNewItemModel.Id), post.Id.ToString(CultureInfo.InvariantCulture),
                Field.Store.YES),
            new StringField(nameof(WhatsNewItemModel.DocumentTypeIdHash), post.DocumentTypeIdHash, Field.Store.YES),
            new StringField(nameof(WhatsNewItemModel.DocumentContentHash), post.DocumentContentHash, Field.Store.YES),
            new StringField(nameof(WhatsNewItemModel.UserId), post.UserId?.ToString(CultureInfo.InvariantCulture) ?? "",
                Field.Store.YES),
            new StringField(nameof(WhatsNewItemModel.EntityType), post.EntityType?.FullName ?? "", Field.Store.YES)
        ];

        return document.NormalizeDocument();
    }

    public static LuceneSearchResult MapToLuceneSearchResult(this Document document,
        int luceneDocId,
        float score,
        float scoreNorm)
    {
        ArgumentNullException.ThrowIfNull(document);

        var content = document.Get(nameof(WhatsNewItemModel.Content), CultureInfo.InvariantCulture);
        var publishDate = document.Get(nameof(WhatsNewItemModel.PublishDate), CultureInfo.InvariantCulture);

        return new LuceneSearchResult
        {
            Id = document.Get(nameof(WhatsNewItemModel.Id), CultureInfo.InvariantCulture).ToInt(),
            OriginalTitle = document.Get(nameof(WhatsNewItemModel.OriginalTitle), CultureInfo.InvariantCulture),
            Url = document.Get(nameof(WhatsNewItemModel.Url), CultureInfo.InvariantCulture),
            PublishDate =
                DateTimeOffset.ParseExact(publishDate, DateTimeFormat, DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.AssumeUniversal),
            ItemType =
                WhatsNewItemType.Get(document.Get(nameof(WhatsNewItemModel.ItemType), CultureInfo.InvariantCulture)),
            EntityType = Type.GetType(document.Get(nameof(WhatsNewItemModel.EntityType), CultureInfo.InvariantCulture)),
            AuthorName = document.Get(nameof(WhatsNewItemModel.AuthorName), CultureInfo.InvariantCulture),
            UserId = document.Get(nameof(WhatsNewItemModel.UserId), CultureInfo.InvariantCulture).ToInt(),
            Categories = document.GetCategories(),
            Content = content,
            Score = score * scoreNorm,
            LuceneDocId = luceneDocId
        };
    }

    public static LuceneSearchResult MapToLuceneSearchResult(this WhatsNewItemModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return new LuceneSearchResult
        {
            Id = item.Id,
            OriginalTitle = item.OriginalTitle,
            Url = item.Url,
            PublishDate = item.PublishDate,
            ItemType = item.ItemType,
            AuthorName = item.AuthorName,
            Content = item.Content,
            User = item.User,
            Title = item.Title,
            Categories = item.Categories,
            LastUpdatedTime = item.LastUpdatedTime,
            UserId = item.UserId,
            EntityType = item.EntityType
        };
    }

    private static IEnumerable<string> GetCategories(this Document document)
    {
        var categories = document.Get(nameof(WhatsNewItemModel.Categories), CultureInfo.InvariantCulture);

        return string.IsNullOrWhiteSpace(categories)
            ? Enumerable.Empty<string>()
            : categories.Split(CategoriesSeparator, StringSplitOptions.RemoveEmptyEntries);
    }
}
