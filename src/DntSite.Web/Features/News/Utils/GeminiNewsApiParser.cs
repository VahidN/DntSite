using System.Text.RegularExpressions;
using DntSite.Web.Features.News.Models;

namespace DntSite.Web.Features.News.Utils;

public static partial class GeminiNewsApiParser
{
    private const string RaviAI = nameof(RaviAI);

    private const string SuccessRecordBegin = "=== RAVI_AI_SUCCESS_RECORD_BEGIN ===";
    private const string SuccessRecordEnd = "=== RAVI_AI_SUCCESS_RECORD_END ===";

    private const string FallbackRecordBegin = "=== RAVI_AI_FALLBACK_RECORD_BEGIN ===";
    private const string FallbackRecordEnd = "=== RAVI_AI_FALLBACK_RECORD_END ===";

    [GeneratedRegex(
        $@"STATUS:\s*(?<status>.*?)\s*REASON:\s*(?<reason>.*?)\s*TITLE:\s*(?<title>.*?)\s*SUMMARY:\s*(?<summary>.*?)\s*TAGS:\s*(?<tags>.*?)\s*{FallbackRecordEnd}",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 3000)]
    private static partial Regex FallbackRecordPattern();

    [GeneratedRegex(
        $@"STATUS:\s*(?<status>.*?)\s*TITLE:\s*(?<title>.*?)\s*SUMMARY:\s*(?<summary>.*?)\s*TAGS:\s*(?<tags>.*?)\s*{SuccessRecordEnd}",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 3000)]
    private static partial Regex SuccessRecordPattern();

    public static GeminiApiResult? ParseGeminiOutput(this string? apiOutput)
    {
        if (string.IsNullOrWhiteSpace(apiOutput))
        {
            return null;
        }

        if (apiOutput.Contains(SuccessRecordBegin, StringComparison.OrdinalIgnoreCase))
        {
            return ParseSuccessRecord(apiOutput);
        }

        if (apiOutput.Contains(FallbackRecordBegin, StringComparison.OrdinalIgnoreCase))
        {
            return ParseFallbackRecord(apiOutput);
        }

        return null;
    }

    private static GeminiSuccessResult? ParseSuccessRecord(string apiOutput)
    {
        var match = SuccessRecordPattern().Match(apiOutput);

        return match.Success switch
        {
            false => null,
            _ => new GeminiSuccessResult
            {
                Status = match.Groups[groupname: "status"].Value.GetNormalizedAIText(processCodes: false),
                Title = match.Groups[groupname: "title"].Value.GetNormalizedAIText(processCodes: false),
                Summary = match.Groups[groupname: "summary"].Value.GetNormalizedAIText(processCodes: true),
                Tags =
                [
                    RaviAI,
                    ..match.Groups[groupname: "tags"]
                        .Value.GetNormalizedAIText(processCodes: false)
                        .Split([','], StringSplitOptions.RemoveEmptyEntries)
                        .Select(tag => tag.Trim())
                ]
            }
        };
    }

    private static GeminiFallbackResult? ParseFallbackRecord(string apiOutput)
    {
        var match = FallbackRecordPattern().Match(apiOutput);

        return match.Success switch
        {
            false => null,
            _ => new GeminiFallbackResult
            {
                Status = match.Groups[groupname: "status"].Value.Trim(),
                Reason = match.Groups[groupname: "reason"].Value.Trim().ToEnum<GeminiFallbackReason>()
            }
        };
    }
}
