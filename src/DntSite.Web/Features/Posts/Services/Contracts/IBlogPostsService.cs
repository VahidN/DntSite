using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Posts.Services.Contracts;

public interface IBlogPostsService : IScopedService
{
    public Task<BlogPost?> GetFirstBlogPostAsync();

    public Task PerformPossibleDeleteAsync(int? deleteId);

    public Task<BlogPost?> PerformEditAsync(int? editId,
        WriteArticleModel? writeArticleModel,
        ApplicationState? applicationState);

    public Task UpdateStatAsync(int id, bool fromFeed);

    public Task<bool> IsTheSameAuthorAsync(int postId, int userId);

    public Task FixOldBlogImageLinksAsync(string baseUrl);

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsIncludeAuthorsTagsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsIncludeAuthorsTagsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    public Task<List<BlogPost>> GetLastBlogPostsAsync(int start, int count, bool showDeletedItems = false);

    public Task<List<BlogPost>> GetLastBlogPostsIncludeAuthorTagsAsync(int count, bool showDeletedItems = false);

    public Task<BlogPost?> GetBlogPostIncludeAuthorTagsAsync(int id, bool showDeletedItems = false);

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsByAuthorIncludeAuthorTagsAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsByAuthorAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost,
        IList<BlogPostTag> listOfActualTags,
        bool isEditForm = false);

    public Task<BlogPost?> FindBlogPostAsync(string? oldUrl, bool showDeletedItems = false);

    public Task<BlogPost?> FindBlogPostAsync(int id, bool showDeletedItems = false);

    public Task<BlogPost?> FindBlogPostIncludeTagsAsync(int id, bool showDeletedItems = false);

    public Task<List<BlogPost>> GetLastBlogPostsByTermIncludeAuthorsTagsAsync(string term,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<List<BlogPost>> GetLastBlogPostsByPopularItemAsNoTrackingAsync(PopularItem item,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    public Task<List<BlogPost>> GetLastBlogPostsByMonthAsync(int pageNumber, bool showDeletedItems = false);

    public Task<BlogPostModel> GetBlogPostLastAndNextPostIncludeAuthorTagsAsync(int id, bool showDeletedItems = false);

    public Task<int> GetAllBlogPostsCountAsync(bool showDeletedItems = false);

    public Task<List<BlogPost>> GetTopBlogPostsOfThisMonthAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    public Task<List<BlogPost>> GetAllPublicPostsOfDateAsync(DateTime date);

    public Task IndexBlogPostsAsync();
}
