using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class AIDailyNewsService(
    IUsersInfoService usersInfoService,
    IRssReaderService rssReaderService,
    IGeminiClientService geminiClientService,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IDailyNewsItemsService dailyNewsItemsService,
    ILogger<AIDailyNewsService> logger) : IAIDailyNewsService
{
    private static readonly List<string> ProcessedItems = [];

    private static readonly CompositeFormat PromptTemplate = CompositeFormat.Parse(format: """
        You are RaviAI, a developer-focused AI that processes programming news links
        and converts each URL into a structured Persian news record.

        Input:
        - A single URL pointing to a programming-related news or blog article:
        {0}

        Processing rules:
        1. Attempt to read and understand the main content of the URL.
        2. Prioritize factual news over opinions.
        3. Ignore ads, navigation text, and unrelated sections.
        4. Be conservative when information is incomplete.

        Output requirements:
        - Output MUST be valid JSON.
        - Do NOT include any text before or after the JSON.
        - The JSON structure MUST be consistent for all outputs.
        - Use UTF-8 Persian text with common English tech terms.
        - Make the output suitable for direct database storage.

        Primary output schema:
        {{
          "status": "ok",
          "title": "...",
          "summary": "...",
          "tags": ["...", "..."]
        }}

        Language and tone rules:
        - Output must be in Persian.
        - Use common English technical terms (e.g., React, Docker, Node.js).
        - Be factual, neutral.
        - Do not include opinions, emojis, or conversational text.

        Content rules:
        - Title:
          - Persian
          - Max 440 characters
          - News-style (neutral, informative)

        - Summary:
          - Three to five distinct paragraphs, focusing on the key points, main arguments, and conclusions.

        - Tags:
          - 3–5 items
          - Technical and developer-oriented
          - Use common English tech terms (TitleCase or PascalCase), suitable for developers

        Fallback rules:
        If any of the following occur:
        - URL is unreachable or blocked
        - Content is unreadable or too short
        - Content is not related to software development
        - Page is mostly opinion or lacks factual information

        Then output the fallback schema exactly:

        {{
          "status": "fallback",
          "reason": "unreachable | unreadable | not_programming | insufficient_content | low_signal_news",
          "title": null,
          "summary": null,
          "tags": []
        }}

        Additional constraints:
        - Do NOT guess or infer missing facts.
        - Do NOT translate code snippets.
        - Do NOT add emojis or formatting.
        - Maintain consistent terminology across outputs.
        """);

    public async Task StartProcessingNewsFeedsAsync(CancellationToken ct = default)
    {
        var appSetting = await cachedAppSettingsProvider.GetAppSettingsAsync();
        var apiKey = appSetting.GeminiNewsFeeds.ApiKey;
        var feeds = appSetting.GeminiNewsFeeds.NewsFeeds;

        if (apiKey.IsEmpty() || feeds.Count == 0)
        {
            return;
        }

        var aiUser = await usersInfoService.GetNewsLinksAIUserAsync();

        if (aiUser is null)
        {
            logger.LogWarning(message: "NewsLinksAIUser is not registered.");

            return;
        }

        foreach (var feedUrl in feeds)
        {
            try
            {
                var feedChannel = await rssReaderService.ReadRssAsync(feedUrl, ct);

                if (feedChannel.RssItems is null)
                {
                    logger.LogWarning(message: "`{FeedUrl}` feed is empty.", feedUrl);

                    continue;
                }

                var newFeedItems = feedChannel.RssItems
                    .Where(item => !ProcessedItems.Contains(item.Url, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                foreach (var feedItem in newFeedItems)
                {
                    var isSuccessfulResponse = await ProcessFeedItemAsync(apiKey, feedItem, aiUser, ct);

                    if (!isSuccessfulResponse)
                    {
                        return;
                    }

                    ProcessedItems.Add(feedItem.Url);

                    await Task.Delay(TimeSpan.FromSeconds(seconds: 30), ct);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Demystify(), message: "Error processing `{FeedUrl}`.", feedUrl);
            }
        }
    }

    private async Task<bool> ProcessFeedItemAsync(string apiKey, FeedItem feedItem, User aiUser, CancellationToken ct)
    {
        if ((await dailyNewsItemsService.CheckUrlHashAsync(feedItem.Url, id: null, isAdmin: false)).Stat ==
            OperationStat.Failed)
        {
            return true;
        }

        var prompt = string.Format(CultureInfo.InvariantCulture, PromptTemplate, feedItem.Url);

        var responseResult = await geminiClientService.RunGenerateContentPromptsAsync(new GeminiClientOptions
        {
            ApiVersion = GeminiApiVersions.V1Beta,
            ApiKey = apiKey,
            ModelId = "gemma-3-12b-it",
            SystemInstruction = null,
            Chats = [new GeminiChatRequest(prompt)],
            ResponseMimeType = "application/json"
        }, ct);

        if (responseResult.IsSuccessfulResponse is null or false)
        {
            logger.LogWarning(message: "`{FeedItemUrl}` -> `{ResponseBody}`.", feedItem.Url,
                responseResult.ResponseBody ?? "");

            return false;
        }

        var jsonResponse = responseResult.Result?.ResponseParts?.FirstOrDefault()?.Text;

        if (jsonResponse.IsEmpty())
        {
            logger.LogWarning(message: "`{FeedItemUrl}` -> `{ResponseBody}`.", feedItem.Url,
                responseResult.ResponseBody ?? "");

            return true;
        }

        AINewsOutputModel? outputInfo;

        try
        {
            outputInfo = JsonSerializer.Deserialize<AINewsOutputModel>(jsonResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Error processing `{FeedUrl}` -> `{JSON}` -> `{ResponseBody}`.",
                feedItem.Url, jsonResponse, responseResult.ResponseBody);

            return false;
        }

        if (outputInfo is null || outputInfo.Status?.Equals(value: "ok", StringComparison.OrdinalIgnoreCase) == false)
        {
            logger.LogWarning(message: "`{FeedItemUrl}` -> `{ResponseBody}`.", feedItem.Url,
                responseResult.ResponseBody ?? "");

            return true;
        }

        var dailyNewsItemModel = new DailyNewsItemModel
        {
            Title = outputInfo.Title ?? feedItem.Title,
            Url = feedItem.Url,
            DescriptionText = outputInfo.Summary ?? "",
            Tags = outputInfo.Tags ?? ["News"]
        };

        var newsItem = await dailyNewsItemsService.AddNewsItemAsync(dailyNewsItemModel, aiUser);
        await dailyNewsItemsService.NotifyAddOrUpdateChangesAsync(newsItem, dailyNewsItemModel, aiUser);

        return true;
    }
}
