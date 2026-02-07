using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsItemAIBacklogService(
    IUnitOfWork uow,
    IDailyNewsItemsService dailyNewsItemsService,
    IRssReaderService rssReaderService,
    IUsersInfoService usersInfoService,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    IUrlNormalizationService urlNormalizationService,
    IPasswordHasherService passwordHasherService,
    ILogger<DailyNewsItemAIBacklogService> logger) : IDailyNewsItemAIBacklogService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<DailyNewsItemAIBacklog, object?>>> CustomOrders =
        new()
        {
            [PagerSortBy.Date] = x => x.Id,
            [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName
        };

    private readonly DbSet<DailyNewsItemAIBacklog> _dailyNewsItemAIBacklogs = uow.DbSet<DailyNewsItemAIBacklog>();

    public Task<PagedResultModel<DailyNewsItemAIBacklog>> GetLastPagedDailyNewsItemAIBacklogsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        bool showProcessedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _dailyNewsItemAIBacklogs.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.IsDeleted == showDeletedItems && x.IsProcessed == showProcessedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public ValueTask<DailyNewsItemAIBacklog?> FindDailyNewsItemAIBacklogAsync(int? id)
        => id.HasValue
            ? _dailyNewsItemAIBacklogs.FindAsync(id.Value)
            : ValueTask.FromResult<DailyNewsItemAIBacklog?>(result: null);

    public async Task MarkAsDeletedOrApprovedAsync(IList<int>? allIds,
        IList<int>? selectedDeleteIds,
        IList<int>? selectedApproveIds)
    {
        if (allIds is null)
        {
            return;
        }

        selectedDeleteIds ??= [];

        if (selectedDeleteIds.Count > 0)
        {
            await _dailyNewsItemAIBacklogs.Where(x => selectedDeleteIds.Contains(x.Id))
                .ExecuteUpdateAsync(builder
                    => builder.SetProperty(aiBacklog => aiBacklog.IsDeleted, valueExpression: true));
        }

        var notDeletedIds = allIds.Except(selectedDeleteIds).ToList();

        if (notDeletedIds.Count > 0)
        {
            await _dailyNewsItemAIBacklogs.Where(x => notDeletedIds.Contains(x.Id))
                .ExecuteUpdateAsync(builder
                    => builder.SetProperty(aiBacklog => aiBacklog.IsDeleted, valueExpression: false));
        }

        selectedApproveIds ??= [];

        if (selectedApproveIds.Count > 0)
        {
            await _dailyNewsItemAIBacklogs.Where(x => selectedApproveIds.Contains(x.Id))
                .ExecuteUpdateAsync(builder
                    => builder.SetProperty(aiBacklog => aiBacklog.IsApproved, valueExpression: true));
        }

        var notApprovedIds = allIds.Except(selectedApproveIds).ToList();

        if (notApprovedIds.Count > 0)
        {
            await _dailyNewsItemAIBacklogs.Where(x => notApprovedIds.Contains(x.Id))
                .ExecuteUpdateAsync(builder
                    => builder.SetProperty(aiBacklog => aiBacklog.IsApproved, valueExpression: false));
        }
    }

    public async Task MarkAsDeletedAsync(int id)
    {
        var item = await FindDailyNewsItemAIBacklogAsync(id);

        if (item is null)
        {
            return;
        }

        item.IsDeleted = true;
        item.IsApproved = false;
        await uow.SaveChangesAsync();
    }

    public async Task MarkAsApprovedAsync(int id)
    {
        var item = await FindDailyNewsItemAIBacklogAsync(id);

        if (item is null)
        {
            return;
        }

        item.IsApproved = true;
        item.IsDeleted = false;
        await uow.SaveChangesAsync();
    }

    public async Task UpdateFetchRetiresAsync(int id)
    {
        const int MaxRetries = 3;
        var item = await FindDailyNewsItemAIBacklogAsync(id);

        if (item is null)
        {
            return;
        }

        item.FetchRetries++;
        item.IsProcessed = item.FetchRetries > MaxRetries;
        await uow.SaveChangesAsync();
    }

    public async Task MarkAsProcessedAsync(int id, int? dailyNewsItemId)
    {
        var item = await FindDailyNewsItemAIBacklogAsync(id);

        if (item is null)
        {
            return;
        }

        item.IsProcessed = true;
        item.DailyNewsItemId = dailyNewsItemId;
        await uow.SaveChangesAsync();
    }

    public DailyNewsItemAIBacklog AddDailyNewsItemAIBacklog(DailyNewsItemAIBacklog data)
        => _dailyNewsItemAIBacklogs.Add(data).Entity;

    public async Task AddDailyNewsItemAIBacklogsAsync(string? urls, User? user)
    {
        if (urls.IsEmpty())
        {
            return;
        }

        var feedItems = await GetNewUrlsToAddAsync(urls);

        if (feedItems.Count == 0)
        {
            return;
        }

        foreach (var feedItem in feedItems)
        {
            AddDailyNewsItemAIBacklog(new DailyNewsItemAIBacklog
            {
                Url = feedItem.Url.Trim(),
                UrlHash = passwordHasherService.GetSha1Hash(urlNormalizationService.NormalizeUrl(feedItem.Url.Trim())),
                Title = feedItem.Title.GetNormalizedAIText(processCodes: false),
                IsApproved = true,
                IsProcessed = false,
                UserId = user?.Id
            });
        }

        await uow.SaveChangesAsync();
    }

    public Task<List<DailyNewsItemAIBacklog>>
        GetApprovedNotProcessedDailyNewsItemAIBacklogsAsync(CancellationToken ct = default)
        => _dailyNewsItemAIBacklogs.AsNoTracking()
            .Include(x => x.User)
            .Where(x => !x.IsDeleted && !x.IsProcessed && x.IsApproved)
            .OrderBy(x => x.Id)
            .ToListAsync(ct);

    public Task<List<DailyNewsItemAIBacklog>>
        GetNotProcessedDailyNewsItemAIBacklogsAsync(CancellationToken ct = default)
        => _dailyNewsItemAIBacklogs.AsNoTracking()
            .Include(x => x.User)
            .Where(x => !x.IsDeleted && !x.IsProcessed)
            .OrderBy(x => x.Id)
            .ThenBy(x => x.IsApproved)
            .ToListAsync(ct);

    public Task<List<int>> GetNotProcessedDailyNewsItemAIBacklogIdsAsync(CancellationToken ct = default)
        => _dailyNewsItemAIBacklogs.AsNoTracking()
            .Where(x => !x.IsDeleted && !x.IsProcessed)
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .ToListAsync(ct);

    public async Task AddFeedItemsAsDailyNewsItemAIBacklogsAsync(CancellationToken ct = default)
    {
        var feeds = (await cachedAppSettingsProvider.GetAppSettingsAsync()).GeminiNewsFeeds.NewsFeeds;

        if (feeds.Count == 0)
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
                await AddFeedItemsAsDailyNewsItemAIBacklogsAsync(feedUrl, aiUser, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Demystify(), message: "Error processing `{FeedUrl}`.", feedUrl);
            }
        }
    }

    private async Task<List<FeedItem>> GetNewUrlsToAddAsync(string urls)
    {
        var feedItems = urls.ConvertMultiLineTextToList()
            .Where(line => !line.IsEmpty())
            .Select(line =>
            {
                if (!line.Contains(NewsRoutingConstants.UrlTitleSeparator, StringComparison.Ordinal))
                {
                    return new FeedItem
                    {
                        Url = line,
                        Title = line
                    };
                }

                var lineParts = line.Split(NewsRoutingConstants.UrlTitleSeparator,
                    StringSplitOptions.RemoveEmptyEntries);

                return new FeedItem
                {
                    Url = lineParts[0],
                    Title = lineParts.Length == 2 ? lineParts[1] : lineParts[0]
                };
            })
            .Where(item => item.Url.IsValidUrl())
            .ToList();

        return await GetDistinctNewFeedItemsAsync(feedItems);
    }

    private async Task AddFeedItemsAsDailyNewsItemAIBacklogsAsync(string? feedUrl,
        User? aiUser,
        CancellationToken ct = default)
    {
        if (feedUrl.IsEmpty())
        {
            return;
        }

        var feedItems = await GetNewFeedItemsAsync(feedUrl, ct);

        if (feedItems.Count == 0)
        {
            return;
        }

        foreach (var feedItem in feedItems)
        {
            AddDailyNewsItemAIBacklog(new DailyNewsItemAIBacklog
            {
                Url = feedItem.Url.Trim(),
                UrlHash = passwordHasherService.GetSha1Hash(urlNormalizationService.NormalizeUrl(feedItem.Url.Trim())),
                Title = feedItem.Title.GetNormalizedAIText(processCodes: false),
                IsApproved = false,
                IsProcessed = false,
                UserId = aiUser?.Id
            });
        }

        await uow.SaveChangesAsync(ct);
    }

    private async Task<List<FeedItem>> GetNewFeedItemsAsync(string feedUrl, CancellationToken ct)
    {
        var rssItems = (await rssReaderService.ReadRssAsync(feedUrl, ct)).RssItems;

        if (rssItems is null)
        {
            logger.LogWarning(message: "`{FeedUrl}` feed is empty.", feedUrl);

            return [];
        }

        return await GetDistinctNewFeedItemsAsync([.. rssItems], ct);
    }

    private async Task<List<FeedItem>> GetDistinctNewFeedItemsAsync(IList<FeedItem>? rssItems,
        CancellationToken ct = default)
    {
        rssItems ??= [];

        var newLinks =
            await dailyNewsItemsService.GetNotProcessedLinksAsync(rssItems.Select(feedItem => feedItem.Url).Distinct(),
                ct);

        newLinks = await GetNewBacklogLinksAsync(newLinks, ct);

        return [..rssItems.Where(item => newLinks.Contains(item.Url, StringComparer.OrdinalIgnoreCase))];
    }

    private async Task<List<string>> GetNewBacklogLinksAsync(IList<string> urls, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(urls);

        var itemsDictionary = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var url in urls.Distinct())
        {
            if (!url.IsValidUrl())
            {
                continue;
            }

            var key = passwordHasherService.GetSha1Hash(urlNormalizationService.NormalizeUrl(url.Trim()));
            itemsDictionary[key] = url;
        }

        var processedItems = await _dailyNewsItemAIBacklogs
            .Where(newsItem => itemsDictionary.Keys.Contains(newsItem.UrlHash))
            .Select(newsItem => newsItem.UrlHash)
            .ToListAsync(ct);

        return
        [
            .. itemsDictionary.Where(valuePair => !processedItems.Contains(valuePair.Key))
                .Select(valuePair => valuePair.Value)
        ];
    }
}
