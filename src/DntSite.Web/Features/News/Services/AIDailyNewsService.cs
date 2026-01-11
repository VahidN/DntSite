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

    private static readonly string[] Models =
    [
        "gemma-3-12b-it", "gemma-3-1b-it", "gemma-3-27b-it", "gemma-3-2b-it", "gemma-3-4b-it", "gemma-3n-e2b-it"
    ];

    private static readonly CompositeFormat PromptTemplate = CompositeFormat.Parse(format: """
        You are RaviAI, a developer-focused AI that processes programming news content.
        Your ONLY goal is to convert the main content into a structured Persian news record.
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

        Language and tone rules:
        - EXTREMELY IMPORTANT: The 'Title' and 'Summary' fields MUST be generated in PERFECT, STANDARD PERSIAN (Farsi).
        - You are NOT allowed to use English for the main body of the Title or Summary.
        - All non-technical text must be in Persian.
        - Use common English technical terms (e.g., React, Docker) ONLY within the Persian text.
        - Be factual, neutral.

        Primary output format (For successful processing):
        --- START SUCCESS RECORD ---
        STATUS: ok
        TITLE: [Persian Title, Max 440 characters, neutral news-style]
        SUMMARY: [Persian language is mandatory. Three to five distinct paragraphs, focusing on the key points, main arguments, and conclusions]
        TAGS: [3-5 English technical terms, PascalCase/TitleCase, comma-separated (e.g., TypeScript, CloudComputing)]
        --- END SUCCESS RECORD ---

        Fallback output format (If processing fails):
        --- START FALLBACK RECORD ---
        STATUS: fallback
        REASON: [Choose one: Unreadable | NotProgramming | InsufficientContent | LowSignalNews | LanguageFailure]
        TITLE: Null
        SUMMARY: Null
        TAGS: Null
        --- END FALLBACK RECORD ---


        Content rules:
        - Title: Persian language is mandatory. Max 440 characters, news-style(neutral, informative).
        - Tags: 3–5 items, technical and developer-oriented, English (e.g., TypeScript, CloudComputing).

        Additional constraints:
        - IF you cannot generate the Title or Summary perfectly in Persian, you MUST output the Fallback record with Reason set to LanguageFailure.
        - Do NOT guess or infer missing facts.
        - Do NOT include opinions, emojis, or conversational text.
        - Do NOT translate code snippets.

        Fallback rules:
        If any of the following occur:
        - Content is unreadable or too short
        - EXTREMELY IMPORTANT: Content is not related to Microsoft .NET software development and its related technologies

        [ARTICLE CONTENT START]
        Title:
        {0}

        HTML Body:
        {1}
        [ARTICLE CONTENT END]
        """);

    private string? _workingModel;

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
                logger.LogWarning(message: "Not a good source -> `{FeedItemUrl}`. Prompt is empty.", feedItem.Url);

                return true;
            }

            var responseResult = await GetGeminiResponseResultAsync(apiKey, prompt, feedItem.Url, ct);

            if (responseResult is null)
            {
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
                    switch (fallbackResult.Reason)
                    {
                        case GeminiFallbackReason.Unreadable:
                        case GeminiFallbackReason.NotProgramming:
                        case GeminiFallbackReason.InsufficientContent:
                        case GeminiFallbackReason.LowSignalNews:
                            await dailyNewsItemsService.AddNewsItemAsDeletedAsync(feedItem.Url, aiUser);

                            logger.LogWarning(
                                message: "`GeminiFallbackResult -> {FeedItemUrl}` -> {Reason} -> `{ResponseBody}`.",
                                feedItem.Url, fallbackResult.Reason, responseResult.ResponseBody ?? "");

                            return true;
                        default:
                            ResetModel();

                            return false;
                    }
                case GeminiSuccessResult successResult:

                    if (IsLanguageSupportFailure(successResult))
                    {
                        ResetModel();

                        return false;
                    }

                    var dailyNewsItemModel = new DailyNewsItemModel
                    {
                        Title = successResult.Title ?? feedItem.Title,
                        Url = feedItem.Url,
                        DescriptionText = successResult.Summary ?? "",
                        Tags = successResult.Tags ?? [DailyNewsItemsService.DefaultTag]
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

    private static bool IsLanguageSupportFailure(GeminiSuccessResult successResult)
        => !successResult.Title.ContainsFarsi() || !successResult.Summary.ContainsFarsi();

    private async Task<GeminiResponseResult<GeminiGenerateContentResponse?>?> GetGeminiResponseResultAsync(
        string apiKey,
        string prompt,
        string feedItemUrl,
        CancellationToken ct)
    {
        var models = _workingModel is null
            ? Models
            : [_workingModel, ..Models.Except([_workingModel], StringComparer.Ordinal)];

        foreach (var model in models)
        {
            var responseResult = await geminiClientService.RunGenerateContentPromptsAsync(new GeminiClientOptions
            {
                ApiVersion = GeminiApiVersions.V1Beta,
                ApiKey = apiKey,
                ModelId = model,
                Chats = [new GeminiChatRequest(prompt)]
            }, ct);

            if (responseResult.IsSuccessfulResponse is not (null or false))
            {
                _workingModel = model;

                return responseResult;
            }

            logger.LogWarning(message: "!IsSuccessfulResponse -> `{FeedItemUrl}` -> {ErrorMessage} ->`{ResponseBody}`.",
                feedItemUrl, responseResult.ErrorResponse?.Error?.Message ?? "", responseResult.ResponseBody ?? "");

            await emailsFactoryService.SendTextToAllAdminsAsync(responseResult.ResponseBody ?? "!IsSuccessfulResponse",
                emailSubject: "Gemini Client Service Error");
        }

        ResetModel();

        return null;
    }

    private void ResetModel() => _workingModel = null;

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
