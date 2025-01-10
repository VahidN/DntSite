using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsItemsService : IScopedService
{
    public DailyNewsItem AddDailyNewsItem(DailyNewsItem data);

    public Task<bool> IsTheSameAuthorAsync(int postId, int userId);

    public Task<DailyNewsItem?> FindDailyNewsItemAsync(string urlHash);

    public ValueTask<DailyNewsItem?> FindDailyNewsItemAsync(int id);

    public Task<string?> GetRedirectUrlAsync(string siteUrl, int maxRedirects = 20);

    public Task<List<DailyNewsItem>> GetLastDailyNewsItemsIncludeUserAsync(int count, bool showDeletedItems = false);

    public Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagsAsync(
        DntQueryBuilderModel state,
        bool showDeletedItems = false);

    public Task UpdateStatAsync(int id, bool fromFeed);

    public Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<PagedResultModel<DailyNewsItem>> GetDailyNewsItemsIncludeUserAndTagByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<NewsDetailsModel> GetNewsLastAndNextIncludeAuthorTagsAsync(int id, bool showDeletedItems = false);

    public Task<List<DailyNewsItem>> GetTopDailyNewsItemsOfThisMonthAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    public Task<List<DailyNewsItem>> GetLastDailyNewsItemsByPopularItemAsNoTrackingAsync(PopularItem item,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    public Task<List<DailyNewsItem>> GetLastDailyNewsItemsByMonthAsync(int pageNumber, bool showDeletedItems = false);

    public Task<int> GetAllDailyNewsItemsCountAsync(bool showDeletedItems = false);

    public Task<List<DailyNewsItem>> GetAllPublicNewsOfDateAsync(DateTime date);

    public Task UpdateAllNewsLastHttpStatusCodeAsync(UpdateNewsStatusAction updateNewsStatusAction,
        CancellationToken cancellationToken);

    public Task<DailyNewsItem?> GetDailyNewsItemAsync(int id, bool showDeletedItems = false);

    public Task MarkAsDeletedAsync(DailyNewsItem? item);

    public Task UpdateNewsItemAsync(DailyNewsItem? newsItem, DailyNewsItemModel writeNewsModel);

    public Task<DailyNewsItem> AddNewsItemAsync(DailyNewsItemModel writeNewsModel, User? user);

    public Task NotifyAddOrUpdateChangesAsync(DailyNewsItem? newsItem, DailyNewsItemModel writeNewsModel, User? user);

    public Task NotifyDeleteChangesAsync(DailyNewsItem? newsItem, User? user);

    public Task<OperationResult> CheckUrlHashAsync(string url, int? id, bool isAdmin);

    public Task IndexDailyNewsItemsAsync();
}
