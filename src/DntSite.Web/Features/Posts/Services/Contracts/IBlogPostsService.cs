using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostsService : IScopedService
{
    Task<BlogPost?> GetFirstBlogPostAsync();

    Task PerformPossibleDeleteAsync(int? deleteId);

    Task<BlogPost?> PerformEditAsync(int? editId,
        WriteArticleModel? writeArticleModel,
        ApplicationState? applicationState);

    Task UpdateStatAsync(int id, bool fromFeed);

    Task<bool> IsTheSameAuthorAsync(int postId, int userId);

    Task FixOldBlogImageLinksAsync(string baseUrl);

    Task<PagedResultModel<BlogPost>> GetLastBlogPostsIncludeAuthorsTagsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<BlogPost>> GetLastBlogPostsIncludeAuthorsTagsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    Task<List<BlogPost>> GetLastBlogPostsAsync(int start, int count, bool showDeletedItems = false);

    Task<List<BlogPost>> GetLastBlogPostsIncludeAuthorTagsAsync(int count, bool showDeletedItems = false);

    Task<BlogPost?> GetBlogPostIncludeAuthorTagsAsync(int id, bool showDeletedItems = false);

    Task<PagedResultModel<BlogPost>> GetLastBlogPostsByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<BlogPost>> GetLastBlogPostsByAuthorIncludeAuthorTagsAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<BlogPost>> GetLastBlogPostsByAuthorAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost, IList<BlogPostTag> listOfActualTags, bool isEditForm = false);

    Task<BlogPost?> FindBlogPostAsync(string oldUrl, bool showDeletedItems = false);

    Task<BlogPost?> FindBlogPostAsync(int id, bool showDeletedItems = false);

    Task<BlogPost?> FindBlogPostIncludeTagsAsync(int id, bool showDeletedItems = false);

    Task<List<BlogPost>> GetLastBlogPostsByTermIncludeAuthorsTagsAsync(string term,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<List<BlogPost>> GetLastBlogPostsByPopularItemAsNoTrackingAsync(PopularItem item,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    Task<List<BlogPost>> GetLastBlogPostsByMonthAsync(int pageNumber, bool showDeletedItems = false);

    Task<BlogPostModel> GetBlogPostLastAndNextPostIncludeAuthorTagsAsync(int id, bool showDeletedItems = false);

    Task<int> GetAllBlogPostsCountAsync(bool showDeletedItems = false);

    Task<List<BlogPost>> GetTopBlogPostsOfThisMonthAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    Task<List<BlogPost>> GetAllPublicPostsOfDateAsync(DateTime date);
}
