using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.News.Services.Contracts;

public interface IDailyNewsItemsService : IScopedService
{
    DailyNewsItem AddDailyNewsItem(DailyNewsItem data);

    Task<bool> IsTheSameAuthorAsync(int postId, int userId);

    Task<DailyNewsItem?> FindDailyNewsItemAsync(string urlHash);

    ValueTask<DailyNewsItem?> FindDailyNewsItemAsync(int id);

    Task<string?> GetRedirectUrlAsync(string siteUrl, int maxRedirects = 20);

    Task<List<DailyNewsItem>> GetLastDailyNewsItemsIncludeUserAsync(int count, bool showDeletedItems = false);

    Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    Task UpdateStatAsync(int id, bool fromFeed);

    Task<PagedResultModel<DailyNewsItem>> GetLastPagedDailyNewsItemsIncludeUserAndTagAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<PagedResultModel<DailyNewsItem>> GetDailyNewsItemsIncludeUserAndTagByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<NewsDetailsModel> GetNewsLastAndNextIncludeAuthorTagsAsync(int id, bool showDeletedItems = false);

    Task<List<DailyNewsItem>> GetTopDailyNewsItemsOfThisMonthAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    Task<List<DailyNewsItem>> GetLastDailyNewsItemsByPopularItemAsNoTrackingAsync(PopularItem item,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    Task<List<DailyNewsItem>> GetLastDailyNewsItemsByMonthAsync(int pageNumber, bool showDeletedItems = false);

    Task<int> GetAllDailyNewsItemsCountAsync(bool showDeletedItems = false);

    Task<List<DailyNewsItem>> GetAllPublicNewsOfDateAsync(DateTime date);

    Task UpdateAllNewsLastHttpStatusCodeAsync();

    Task<DailyNewsItem?> GetDailyNewsItemAsync(int id, bool showDeletedItems = false);

    Task MarkAsDeletedAsync(DailyNewsItem? item);

    Task UpdateNewsItemAsync(DailyNewsItem? newsItem, DailyNewsItemModel writeNewsModel);

    Task<DailyNewsItem> AddNewsItemAsync(DailyNewsItemModel writeNewsModel, User? user);

    Task NotifyAddOrUpdateChangesAsync(DailyNewsItem? newsItem, DailyNewsItemModel writeNewsModel, User? user);

    Task NotifyDeleteChangesAsync(DailyNewsItem? newsItem, User? user);

    Task<OperationResult> CheckUrlHashAsync(string url, int? id, bool isAdmin);

    Task IndexDailyNewsItemsAsync();
}
