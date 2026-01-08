using System.Text.RegularExpressions;
using DntSite.Web.Features.News.Models;

namespace DntSite.Web.Features.News.Utils;

public static partial class GeminiNewsApiParser
{
    [GeneratedRegex(
        pattern:
        @"STATUS:\s*(?<status>.*?)\s*REASON:\s*(?<reason>.*?)\s*TITLE:\s*(?<title>.*?)\s*SUMMARY:\s*(?<summary>.*?)\s*TAGS:\s*(?<tags>.*?)\s*--- END FALLBACK RECORD ---",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 3000)]
    private static partial Regex FallbackRecordPattern();

    [GeneratedRegex(
        pattern:
        @"STATUS:\s*(?<status>.*?)\s*TITLE:\s*(?<title>.*?)\s*SUMMARY:\s*(?<summary>.*?)\s*TAGS:\s*(?<tags>.*?)\s*--- END SUCCESS RECORD ---",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 3000)]
    private static partial Regex SuccessRecordPattern();

    public static GeminiApiResult? ParseGeminiOutput(this string? apiOutput)
    {
        if (string.IsNullOrWhiteSpace(apiOutput))
        {
            return null;
        }

        if (apiOutput.Contains(value: "--- START SUCCESS RECORD ---", StringComparison.OrdinalIgnoreCase))
        {
            return ParseSuccessRecord(apiOutput);
        }

        if (apiOutput.Contains(value: "--- START FALLBACK RECORD ---", StringComparison.OrdinalIgnoreCase))
        {
            return ParseFallbackRecord(apiOutput);
        }

        return null;
    }

    private static GeminiSuccessResult? ParseSuccessRecord(string apiOutput)
    {
        var match = SuccessRecordPattern().Match(apiOutput);

        if (!match.Success)
        {
            return null;
        }

        var result = new GeminiSuccessResult
        {
            Status = WebUtility.HtmlDecode(match.Groups[groupname: "status"].Value.Trim()),
            Title = WebUtility.HtmlDecode(match.Groups[groupname: "title"].Value.Trim()),
            Summary = WebUtility.HtmlDecode(match.Groups[groupname: "summary"].Value.Trim())
        };

        var tagsString = WebUtility.HtmlDecode(match.Groups[groupname: "tags"].Value.Trim());

        if (!string.IsNullOrEmpty(tagsString))
        {
            result.Tags = tagsString.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
        }

        return result;
    }

    private static GeminiFallbackResult? ParseFallbackRecord(string apiOutput)
    {
        var match = FallbackRecordPattern().Match(apiOutput);

        if (!match.Success)
        {
            return null;
        }

        return new GeminiFallbackResult
        {
            Status = match.Groups[groupname: "status"].Value.Trim(),
            Reason = match.Groups[groupname: "reason"].Value.Trim()
        };
    }
}
