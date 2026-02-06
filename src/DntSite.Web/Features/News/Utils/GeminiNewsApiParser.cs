using System.Text.RegularExpressions;
using DntSite.Web.Features.News.Models;

namespace DntSite.Web.Features.News.Utils;

public static class GeminiNewsApiParser
{
    private const string SuccessBegin = "=== RAVI_AI_SUCCESS_RECORD_BEGIN ===";
    private const string FallbackBegin = "=== RAVI_AI_FALLBACK_RECORD_BEGIN ===";

    private static readonly string[] CommonFields = ["STATUS", "TITLE", "SUMMARY", "TAGS", "REASON"];

    private static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(value: 3);

    public static GeminiApiResult? ParseGeminiOutput(this string apiOutput)
    {
        var input = Normalize(apiOutput);

        if (input.Contains(SuccessBegin, StringComparison.OrdinalIgnoreCase))
        {
            return ParseSuccess(input);
        }

        if (input.Contains(FallbackBegin, StringComparison.OrdinalIgnoreCase))
        {
            return ParseFallback(input);
        }

        return null;
    }

    private static string BuildBoundaryRegex()
    {
        var escaped = CommonFields.Select(Regex.Escape);

        return $@"(?=\n(?:{string.Join(separator: '|', escaped)})\s*:|\n===|\z)";
    }

    private static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var text = input.Replace(oldValue: "\r\n", newValue: "\n", StringComparison.Ordinal).Trim();

        // STATUS ok / fallback بدون colon
        text = Regex.Replace(text, pattern: @"\bSTATUS\s+(ok|fallback)\b", replacement: "STATUS: $1",
            RegexOptions.IgnoreCase, MatchTimeout);

        // Null variants
        text = Regex.Replace(text, pattern: @"\bNULL\b", replacement: "Null", RegexOptions.IgnoreCase, MatchTimeout);

        return text;
    }

    private static string? ExtractField(string input, string label)
    {
        var boundary = BuildBoundaryRegex();

        var pattern = $"""
                       {Regex.Escape(label)}\s*:\s*
                       (?<value>.*?)
                       {boundary}
                       """;

        var match = Regex.Match(input, pattern,
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace, MatchTimeout);

        if (!match.Success)
        {
            return null;
        }

        var value = match.Groups[groupname: "value"].Value.Trim();

        return string.Equals(value, b: "Null", StringComparison.OrdinalIgnoreCase) ? null : value;
    }

    private static GeminiSuccessResult ParseSuccess(string input)
    {
        var title = ExtractField(input, label: "TITLE");
        var summary = ExtractField(input, label: "SUMMARY");
        var tagsRaw = ExtractField(input, label: "TAGS");

        return new GeminiSuccessResult
        {
            Status = "ok",
            Title = title.GetNormalizedAIText(processCodes: false),
            Summary = summary.GetNormalizedAIText(processCodes: true),
            Tags = ParseTags(tagsRaw.GetNormalizedAIText(processCodes: false))
        };
    }

    private static GeminiFallbackResult ParseFallback(string input)
    {
        var reasonRaw = ExtractField(input, label: "REASON");

        return new GeminiFallbackResult
        {
            Status = "fallback",
            Reason = reasonRaw?.Trim().ToEnum<GeminiFallbackReason>()
        };
    }

    private static List<string>? ParseTags(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        return raw.Trim('[', ']')
            .Split(separator: ',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .ToList();
    }
}
