using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.News.Utils;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class AIDailyNewsService(
    IUsersInfoService usersInfoService,
    IRssReaderService rssReaderService,
    IGeminiClientService geminiClientService,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IDailyNewsItemsService dailyNewsItemsService,
    BaseHttpClient baseHttpClient,
    IEmailsFactoryService emailsFactoryService,
    IYoutubeScreenshotsService youtubeScreenshots,
    ILogger<AIDailyNewsService> logger) : IAIDailyNewsService
{
    private const int QuotaLimit = 15000;
    private const int MaxTextSize = 3800;

    private static readonly CompositeFormat PromptTemplate = CompositeFormat.Parse(format: """
        You are RaviAI, a developer-focused AI that processes programming news content
        and converts the main content into a structured Persian news record.
        You MUST output the record using a strict, delimited, and non-JSON text format.

        Input:
        - Main content of a programming-related news or blog article:
        [Content of the article is provided below]

        Processing rules:
        1. Prioritize factual programming news content.
        2. Ignore ads, navigation text, and unrelated sections.
        3. Be conservative when information is incomplete.

        Output requirements:
        - Output MUST be a single block of text following the [SUCCESS RECORD] or [FALLBACK RECORD] format exactly.
        - Do NOT include any text before or after the required record block.
        - Use UTF-8 Persian text with common English tech terms.
        - The output format MUST be consistent for all outputs.

        Primary output format (For successful processing):
        --- START SUCCESS RECORD ---
        STATUS: ok
        TITLE: [Persian Title, Max 440 characters, neutral news-style]
        SUMMARY: [Three to five distinct paragraphs, focusing on the key points, main arguments, and conclusions]
        TAGS: [3-5 English technical terms, PascalCase/TitleCase, comma-separated]
        --- END SUCCESS RECORD ---

        Fallback output format (If processing fails):
        --- START FALLBACK RECORD ---
        STATUS: fallback
        REASON: [Choose one: Unreadable | NotProgramming | InsufficientContent | LowSignalNews]
        TITLE: Null
        SUMMARY: Null
        TAGS: Null
        --- END FALLBACK RECORD ---

        Language and tone rules:
        - All non-technical text must be in Persian.
        - Use common English technical terms (e.g., React, Docker).
        - Be factual, neutral.

        Content rules:
        - Title: Max 440 characters, news-style.
        - Tags: 3–5 items, technical and developer-oriented, English (e.g., TypeScript, CloudComputing).

        Additional constraints:
        - Do NOT guess or infer missing facts.
        - Do NOT include opinions, emojis, or conversational text.
        - Do NOT translate code snippets.

        Fallback rules:
        If any of the following occur:
        - Content is unreadable or too short
        - Content is not related to software development

        [ARTICLE CONTENT START]
        Title:
        {0}

        HTML Body:
        {1}
        [ARTICLE CONTENT END]
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
                var newFeedItems = await GetNewFeedItemsAsync(feedUrl, ct);

                foreach (var feedItem in newFeedItems)
                {
                    var isSuccessfulResponse = await ProcessFeedItemAsync(apiKey, feedItem, aiUser, ct);

                    if (!isSuccessfulResponse)
                    {
                        return;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(seconds: 30), ct);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Demystify(), message: "Error processing `{FeedUrl}`.", feedUrl);
            }
        }
    }

    private async Task<List<FeedItem>> GetNewFeedItemsAsync(string feedUrl, CancellationToken ct)
    {
        var feedChannel = await rssReaderService.ReadRssAsync(feedUrl, ct);

        if (feedChannel.RssItems is null)
        {
            logger.LogWarning(message: "`{FeedUrl}` feed is empty.", feedUrl);

            return [];
        }

        var newLinks =
            await dailyNewsItemsService.GetNotProcessedLinksAsync(
                feedChannel.RssItems.Select(feedItem => feedItem.Url).Distinct(), ct);

        return [..feedChannel.RssItems.Where(item => newLinks.Contains(item.Url, StringComparer.OrdinalIgnoreCase))];
    }

    private async Task<bool> ProcessFeedItemAsync(string apiKey, FeedItem feedItem, User aiUser, CancellationToken ct)
    {
        try
        {
            if ((await dailyNewsItemsService.CheckUrlHashAsync(feedItem.Url, id: null, isAdmin: false)).Stat ==
                OperationStat.Failed)
            {
                return true;
            }

            var prompt = await CreatePromptAsync(feedItem, ct);

            if (prompt.IsEmpty())
            {
                logger.LogWarning(message: "Not a good source -> `{FeedItemUrl}`.", feedItem.Url);

                return true;
            }

            var responseResult = await geminiClientService.RunGenerateContentPromptsAsync(new GeminiClientOptions
            {
                ApiVersion = GeminiApiVersions.V1Beta,
                ApiKey = apiKey,
                ModelId = "gemma-3-12b-it",
                Chats = [new GeminiChatRequest(prompt)]
            }, ct);

            if (responseResult.IsSuccessfulResponse is null or false)
            {
                logger.LogWarning(
                    message: "!IsSuccessfulResponse -> `{FeedItemUrl}` -> {ErrorMessage} ->`{ResponseBody}`.",
                    feedItem.Url, responseResult.ErrorResponse?.Error?.Message ?? "",
                    responseResult.ResponseBody ?? "");

                await emailsFactoryService.SendTextToAllAdminsAsync(
                    responseResult.ResponseBody ?? "!IsSuccessfulResponse",
                    emailSubject: "Gemini Client Service Error");

                return false;
            }

            var apiResponse = responseResult.Result?.ResponseParts?.FirstOrDefault()?.Text;

            if (apiResponse.IsEmpty())
            {
                logger.LogWarning(message: "ApiResponse -> IsEmpty -> `{FeedItemUrl}` -> `{ResponseBody}`.",
                    feedItem.Url, responseResult.ResponseBody ?? "");

                return true;
            }

            var geminiApiResult = apiResponse.ParseGeminiOutput();

            switch (geminiApiResult)
            {
                case GeminiFallbackResult fallbackResult:
                    logger.LogWarning(
                        message: "`GeminiFallbackResult -> {FeedItemUrl}` -> {Reason} -> `{ResponseBody}`.",
                        feedItem.Url, fallbackResult.Reason, responseResult.ResponseBody ?? "");

                    return true;
                case GeminiSuccessResult success:
                    var dailyNewsItemModel = new DailyNewsItemModel
                    {
                        Title = success.Title ?? feedItem.Title,
                        Url = feedItem.Url,
                        DescriptionText = success.Summary ?? "",
                        Tags = success.Tags ?? ["News"]
                    };

                    var newsItem = await dailyNewsItemsService.AddNewsItemAsync(dailyNewsItemModel, aiUser);
                    await dailyNewsItemsService.NotifyAddOrUpdateChangesAsync(newsItem, dailyNewsItemModel, aiUser);

                    break;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Error processing `{FeedUrl}`.", feedItem.Url);
        }

        return true;
    }

    private async Task<string?> CreatePromptAsync(FeedItem feedItem, CancellationToken ct)
    {
        var description = youtubeScreenshots.IsYoutubeVideo(feedItem.Url).Success
            ? await youtubeScreenshots.GetYoutubeVideoDescriptionAsync(feedItem.Url, ct) ?? ""
            : await baseHttpClient.HttpClient.HtmlToTextAsync(feedItem.Url, logger, ct);

        if (description.Trim().IsEmpty())
        {
            return null;
        }

        var estimatedTokens = description.EstimateMaxOutputTokens();

        if (estimatedTokens >= QuotaLimit || description.Length >= MaxTextSize)
        {
            description = description.GetBriefDescription(MaxTextSize);
        }

        return string.Format(CultureInfo.InvariantCulture, PromptTemplate, feedItem.Title, description);
    }
}
