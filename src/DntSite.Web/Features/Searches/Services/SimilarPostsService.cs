using System.Text;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Services;

public class SimilarPostsService(IFullTextSearchService fullTextSearchService) : ISimilarPostsService
{
    public string GetSimilarPostsHtmlBody(string? documentTypeIdHash, int maxItems = 11)
    {
        if (documentTypeIdHash.IsEmpty())
        {
            return string.Empty;
        }

        var similarPosts = fullTextSearchService
            .FindPagedSimilarPosts(documentTypeIdHash, maxItems, pageNumber: 1, maxItems)
            .Data.Where(x
                => !string.Equals(x.DocumentTypeIdHash, documentTypeIdHash, StringComparison.OrdinalIgnoreCase))
            .DistinctBy(x => x.Url)
            .OrderBy(x => x.ItemType.Value)
            .ThenByDescending(x => x.Score)
            .ToList();

        if (similarPosts.Count == 0)
        {
            return string.Empty;
        }

        var html = new StringBuilder();

        html.AppendLine(value: "<div class='card mt-3 mb-2 shadow-sm'>");

        html.AppendLine(
            value: "<div class='card-header entry-title shadow-sm d-flex flex-row flex-wrap align-items-center'>");

        html.AppendLine(value: "<strong>مطالب مشابه</strong>");
        html.AppendLine(value: "</div>");

        html.AppendLine(value: "<ul class='list-group'>");

        foreach (var item in similarPosts)
        {
            html.AppendFormat(CultureInfo.InvariantCulture,
                format: "<a class='list-group-item list-group-item-action d-flex align-items-center' href='{0}'>",
                item.Url);

            html.AppendFormat(CultureInfo.InvariantCulture, format: "<span class='badge {0} rounded-pill me-2'>",
                item.ItemType.BgColor);

            html.Append(item.ItemType.Value);
            html.Append(value: "</span>");
            html.Append(item.OriginalTitle);
            html.Append(value: "</a>");
        }

        html.AppendLine(value: "</ul>");
        html.AppendLine(value: "</div>");

        return html.ToString();
    }
}
