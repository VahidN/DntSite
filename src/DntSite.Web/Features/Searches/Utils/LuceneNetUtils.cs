using System.Text.RegularExpressions;
using Humanizer;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;

namespace DntSite.Web.Features.Searches.Utils;

public static class LuceneNetUtils
{
    public static string ToHighlightedText(this string? text, string? lookup, string cssClass = "text-primary")
        => string.IsNullOrWhiteSpace(lookup) || string.IsNullOrWhiteSpace(text)
            ? ""
            : Regex.Replace(text, Regex.Escape(lookup), match => $"<span class='{cssClass}'>{match.Value}</span>",
                RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(value: 3));

    public static string SearchByPartialWords(this string bodyTerm, string matchingType, bool convertToLowercase)
    {
        if (string.IsNullOrWhiteSpace(bodyTerm))
        {
            return string.Empty;
        }

        bodyTerm = bodyTerm.Trim()
            .Replace(oldValue: "*", newValue: " ", StringComparison.OrdinalIgnoreCase)
            .Replace(oldValue: "?", newValue: " ", StringComparison.OrdinalIgnoreCase)
            .Replace(oldValue: "~", newValue: " ", StringComparison.OrdinalIgnoreCase)
            .Replace(oldValue: "-", newValue: " ", StringComparison.OrdinalIgnoreCase);

        var terms = bodyTerm.Trim()
            .Humanize()
            .Split(separator: ' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => $"{x.Trim()}{matchingType}");

        bodyTerm = string.Join(separator: " ", terms);

        return convertToLowercase
            ? bodyTerm.ToLowerInvariant() // the StandardAnalyzer generates lower-case terms.
            : bodyTerm;
    }

    public static Query ParseQuery(this QueryParser parser, string searchQuery, bool convertToLowercase)
    {
        ArgumentNullException.ThrowIfNull(parser);
        ArgumentNullException.ThrowIfNull(searchQuery);

        try
        {
            var searchTerms = searchQuery.Trim();
            searchTerms = convertToLowercase ? searchTerms.ToLowerInvariant() : searchTerms;

            return parser.Parse(searchTerms);
        }
        catch (ParseException)
        {
            var searchTerms = QueryParserBase.Escape(searchQuery.Trim());
            searchTerms = convertToLowercase ? searchTerms.ToLowerInvariant() : searchTerms;

            return parser.Parse(searchTerms);
        }
    }

    public static Document NormalizeDocument(this Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        foreach (var field in document)
        {
            switch (field)
            {
                case StringField stringField:
                    stringField.SetStringValue(stringField.GetStringValue(CultureInfo.InvariantCulture)
                        .ApplyCorrectYeKe());

                    break;
                case TextField textField:
                    textField.SetStringValue(textField.GetStringValue(CultureInfo.InvariantCulture).ApplyCorrectYeKe());

                    break;
            }
        }

        return document;
    }
}
