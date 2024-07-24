using AutoMapper;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsItemsService(
    IUnitOfWork uow,
    IMapper mapper,
    ITagsService tagsService,
    IStatService statService,
    IDailyNewsEmailsService dailyNewsEmailsService,
    IUrlNormalizationService urlNormalizationService,
    BaseHttpClient baseHttpClient,
    IRedirectUrlFinderService redirectUrlFinderService,
    ILogger<DailyNewsItemsService> logger,
    IUserRatingsService userRatingsService,
    IAppFoldersService appFoldersService,
    IPasswordHasherService passwordHasherService,
    IAppSettingsService appSettingsService,
    IProtectionProviderService protectionProviderService) : IDailyNewsItemsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<DailyNewsItem, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<DailyNewsItem> _dailyNewsItem = uow.DbSet<DailyNewsItem>();

    public Task<List<DownloadItem>> GetNeedScreenshotsItemsAsync(int count)
        => _dailyNewsItem.AsNoTracking()
            .Where(x => !x.IsDeleted && x.PageThumbnail == null)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .Select(x => new DownloadItem
            {
                Id = x.Id,
                Url = x.Url
            })
            .ToListAsync();

    public async Task UpdateStatAsync(int id, bool fromFeed)
    {
        var item = await FindDailyNewsItemAsync(id);

        if (item is null)
        {
            return;
        }

        item.EntityStat.NumberOfViews++;

        if (fromFeed)
        {
            item.EntityStat.NumberOfViewsFromFeed++;
        }

        var now = DateTime.UtcNow;

        if (item.LastHttpStatusCodeCheckDateTime is null || item.LastHttpStatusCodeCheckDateTime.Value.Day != now.Day)
        {
            item.LastHttpStatusCodeCheckDateTime = now;
            item.LastHttpStatusCode = await baseHttpClient.HttpClient.GetHttpStatusCodeAsync(item.Url);
        }

        await uow.SaveChangesAsync();
    }

    public DailyNewsItem AddDailyNewsItem(DailyNewsItem data) => _dailyNewsItem.Add(data).Entity;

    public Task<string?> GetRedirectUrlAsync(string siteUrl, int maxRedirects = 20)
        => redirectUrlFinderService.GetRedirectUrlAsync(siteUrl, maxRedirects);

    public Task<DailyNewsItem?> FindDailyNewsItemAsync(string urlHash)
        => _dailyNewsItem.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.UrlHash == urlHash);

    public ValueTask<DailyNewsItem?> FindDailyNewsItemAsync(int id) => _dailyNewsItem.FindAsync(id);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<DailyNewsItemReaction, DailyNewsItem>(fkId, reactionType, fromUserId);

    public async Task<bool> IsTheSameAuthorAsync(int postId, int userId)
    {
        var post = await FindDailyNewsItemAsync(postId);

        if (post is null)
        {
            return false;
        }

        if (userId <= 0)
        {
            return false;
        }

        return userId == post.UserId;
    }

    public Task<List<DailyNewsItem>> GetLastDailyNewsItemsIncludeUserAsync(int count, bool showDeletedItems = false)
        => _dailyNewsItem.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<List<DailyNewsItem>> GetTopDailyNewsItemsOfThisMonthAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        var shamsiDate = DateTime.UtcNow.ToPersianYearMonthDay();

        var fromDate =
            Invariant($"{shamsiDate.Year}/{shamsiDate.Month.ToString(format: "00", CultureInfo.InvariantCulture)}/01");

        var toDate =
            Invariant($"{shamsiDate.Year}/{shamsiDate.Month.ToString(format: "00", CultureInfo.InvariantCulture)}/31");

        return _dailyNewsItem.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems)
            .Where(x => x.Audit.CreatedAtPersian.Substring(0, 10).CompareTo(fromDate) >= 0 &&
                        x.Audit.CreatedAtPersian.Substring(0, 10).CompareTo(toDate) <= 0)
            .OrderByDescending(x => x.Rating.TotalRating)
            .ThenByDescending(x => x.Rating.TotalRaters)
            .ThenByDescending(x => x.EntityStat.NumberOfComments)
            .ThenByDescending(x => x.EntityStat.NumberOfViews)
            .ThenByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<List<DailyNewsItem>> GetLastDailyNewsItemsByPopularItemAsNoTrackingAsync(PopularItem item,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        var query = _dailyNewsItem.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems);

        switch (item)
        {
            case PopularItem.ByMonthTopPosts:
                return GetTopDailyNewsItemsOfThisMonthAsync(pageNumber, recordsPerPage, showDeletedItems);
            case PopularItem.ByComments:
                query = query.OrderByDescending(x => x.EntityStat.NumberOfComments);

                break;
            case PopularItem.ByRatings:
                query = query.OrderByDescending(x => x.Rating.TotalRaters);

                break;
            case PopularItem.ByViews:
                query = query.OrderByDescending(x => x.EntityStat.NumberOfViews);

                break;
            case PopularItem.Random:
                query = query.OrderBy(x => Guid.NewGuid());

                return query.Skip(skipRecords).Take(recordsPerPage).ToListAsync();
            case PopularItem.ByLastComments:
                query = from post in query
                    orderby post.Comments.Where(x => x.IsDeleted == showDeletedItems).Max(x => x.Id) descending
                    select post;

                return query.Skip(skipRecords).Take(recordsPerPage).ToListAsync();
            case PopularItem.ByAverageRating:
                query = query.OrderByDescending(x => x.Rating.AverageRating)
                    .ThenByDescending(x => x.Rating.TotalRating)
                    .ThenByDescending(x => x.Id);

                break;
            default:
                query = query.OrderByDescending(x => x.Id);

                break;
        }

        return query.Skip(skipRecords).Take(recordsPerPage).ToListAsync();
    }

    public async Task<List<DailyNewsItem>> GetLastDailyNewsItemsByMonthAsync(int pageNumber,
        bool showDeletedItems = false)
    {
        var lastRecord = await _dailyNewsItem.AsNoTracking().OrderByDescending(x => x.Id).FirstOrDefaultAsync();

        if (lastRecord is null)
        {
            return new List<DailyNewsItem>();
        }

        var year = int.Parse(lastRecord.Audit.CreatedAtPersian.AsSpan(start: 0, length: 4),
            CultureInfo.InvariantCulture);

        year = year - pageNumber;
        var fromDate = Invariant($"{year}/01/01");
        var toDate = Invariant($"{year}/12/30");

        return await _dailyNewsItem.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.IsDeleted == showDeletedItems)
            .Where(x => x.Audit.CreatedAtPersian.Substring(0, 10).CompareTo(fromDate) >= 0 &&
                        x.Audit.CreatedAtPersian.Substring(0, 10).CompareTo(toDate) <= 0)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public Task<int> GetAllDailyNewsItemsCountAsync(bool showDeletedItems = false)
        => _dailyNewsItem.AsNoTracking().CountAsync(x => x.IsDeleted == showDeletedItems);

    public Task<List<DailyNewsItem>> GetAllPublicNewsOfDateAsync(DateTime date)
        => _dailyNewsItem.AsNoTracking()
            .Include(x => x.User)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt.Year == date.Year && x.Audit.CreatedAt.Month == date.Month &&
                        x.Audit.CreatedAt.Day == date.Day)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public async Task<OperationResult<string>> DeleteImageAsync(string pid)
    {
        if (string.IsNullOrWhiteSpace(pid))
        {
            return OperationStat.Failed;
        }

        var id = protectionProviderService.Decrypt(pid)?.ToInt() ?? 0;
        var post = await FindDailyNewsItemAsync(id);

        if (post is null)
        {
            return OperationStat.Failed;
        }

        var name = Invariant($"news-{id}.jpg");
        var path = Path.Combine(appFoldersService.ThumbnailsServiceFolderPath, name);
        File.Delete(path);

        return ("", OperationStat.Succeeded, name);
    }

    public async Task UpdateAllNewsLastHttpStatusCodeAsync()
    {
        var itemsNeedUpdate = _dailyNewsItem.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).AsEnumerable();

        foreach (var item in itemsNeedUpdate)
        {
            try
            {
                var url = item.Url;
                item.LastHttpStatusCodeCheckDateTime = DateTime.UtcNow;
                item.LastHttpStatusCode = await baseHttpClient.HttpClient.GetHttpStatusCodeAsync(url);
                await uow.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(value: 3));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: "UpdateAllNewsPageThumbnails({Id}, {Url}): ", item.Id, item.Url);
            }
        }
    }

    public async Task<NewsDetailsModel> GetNewsLastAndNextIncludeAuthorTagsAsync(int id, bool showDeletedItems = false)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentNews =
                await _dailyNewsItem.AsNoTracking()
                    .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
                    .Include(x => x.User)
                    .Include(blogPost => blogPost.Reactions)
                    .Include(x => x.Tags)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextNews = await _dailyNewsItem.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id > id)
                .OrderBy(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(),
            PreviousNews = await _dailyNewsItem.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id < id)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync()
        };

    public Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _dailyNewsItem.Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagsAsync(
        DntQueryBuilderModel state,
        bool showDeletedItems = false)
    {
        var query = _dailyNewsItem.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(DailyNewsItem.Id),
            [.. GridifyMapings.GetDefaultMappings<DailyNewsItem>()]);
    }

    public Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _dailyNewsItem.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(x => x.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<DailyNewsItem>> GetDailyNewsItemsIncludeUserAndTagByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _dailyNewsItem.AsNoTracking()
            from t in b.Tags
            where t.Name == tagName
            select b;

        query = query.Include(x => x.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<DailyNewsItem?> GetDailyNewsItemAsync(int id, bool showDeletedItems = false)
        => _dailyNewsItem.Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task MarkAsDeletedAsync(DailyNewsItem? item)
    {
        if (item is null)
        {
            return;
        }

        item.IsDeleted = true;
        await uow.SaveChangesAsync();
    }

    public async Task NotifyAddOrUpdateChangesAsync(DailyNewsItem? newsItem,
        DailyNewsItemModel writeNewsModel,
        User? user)
    {
        ArgumentNullException.ThrowIfNull(writeNewsModel);

        await statService.RecalculateAllLinkTagsInUseCountsAsync(writeNewsModel.Tags);

        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(user?.Id ?? 0);

        if (newsItem is not null)
        {
            await dailyNewsEmailsService.DailyNewsItemsSendEmailAsync(newsItem,
                user?.FriendlyName ?? SharedConstants.GuestUserName);
        }
    }

    public async Task NotifyDeleteChangesAsync(DailyNewsItem? newsItem, User? user)
    {
        if (newsItem is null)
        {
            return;
        }

        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(newsItem.UserId ?? 0);

        await dailyNewsEmailsService.DailyNewsItemsSendEmailAsync(new DailyNewsItem
        {
            Title = newsItem.Title,
            Url = newsItem.Url,
            BriefDescription = Invariant($"حذف خبر شماره {newsItem.Id} توسط مدیر از سایت ")
        }, user?.FriendlyName ?? SharedConstants.GuestUserName);
    }

    public async Task<DailyNewsItem> AddNewsItemAsync(DailyNewsItemModel writeNewsModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeNewsModel);

        var listOfActualTags = await tagsService.SaveNewLinkItemTagsAsync(writeNewsModel.Tags);

        var newsItem = mapper.Map<DailyNewsItemModel, DailyNewsItem>(writeNewsModel);
        newsItem.Url = await GetRedirectUrlAsync(writeNewsModel.Url) ?? writeNewsModel.Url;
        newsItem.Tags = listOfActualTags;
        newsItem.UserId = user?.Id;
        var result = AddDailyNewsItem(newsItem);
        await uow.SaveChangesAsync();

        return result;
    }

    public async Task UpdateNewsItemAsync(DailyNewsItem? newsItem, DailyNewsItemModel writeNewsModel)
    {
        ArgumentNullException.ThrowIfNull(writeNewsModel);

        if (newsItem is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveNewLinkItemTagsAsync(writeNewsModel.Tags);

        mapper.Map(writeNewsModel, newsItem);
        newsItem.Url = await GetRedirectUrlAsync(writeNewsModel.Url) ?? writeNewsModel.Url;
        newsItem.Tags = listOfActualTags;

        await uow.SaveChangesAsync();
    }

    public async Task<OperationResult> CheckUrlHashAsync(string url, int? id, bool isAdmin)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return ("لطفا آدرسی را وارد نمائید.", OperationStat.Failed);
        }

        var cantAccessError =
            "سرور برنامه امکان دسترسی به آدرس اشتراکی را جهت تعیین اعتبار وجودی آن ندارد. بهتر است ابتدا وجود این آدرس را در یک مرورگر بررسی کنید.";

        if (!url.IsValidUrl())
        {
            return (cantAccessError, OperationStat.Failed);
        }

        var siteUrl = (await appSettingsService.GetAppSettingsAsync())?.SiteRootUri;

        if (!string.IsNullOrWhiteSpace(siteUrl) && !isAdmin && url.IsLocalReferrer(siteUrl))
        {
            return ("لطفا به سایت جاری لینک ندهید.", OperationStat.Failed);
        }

        if (await appSettingsService.IsBannedDomainAndSubDomainAsync(url) ||
            await appSettingsService.IsBannedSiteAsync(url))
        {
            return ("امکان سرویس دهی به دومین مدنظر شما در این سایت وجود ندارد.", OperationStat.Failed);
        }

        var urlHash = passwordHasherService.GetSha1Hash(urlNormalizationService.NormalizeUrl(url.Trim()));
        var data = await FindDailyNewsItemAsync(urlHash);

        if (id.HasValue && data is not null)
        {
            // Edit mode
            return data.Id == id
                ? OperationStat.Succeeded
                : ("امکان ویرایش این مطلب وجود ندارد.", OperationStat.Failed);
        }

        if (data is not null)
        {
            return (
                Invariant($"این آدرس پیشتر در اشتراک شماره «{data.Id}» تحت عنوان «{data.Title}» در سایت ثبت شده‌است."),
                OperationStat.Failed);
        }

        return OperationStat.Succeeded;
    }
}
