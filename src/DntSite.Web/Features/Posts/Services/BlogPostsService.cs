using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Models;
using DntSite.Web.Features.Posts.ModelsMappings;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Posts.Services;

public class BlogPostsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IAppFoldersService appFoldersService,
    ITagsService tagsService,
    IBlogPostsEmailsService emailsService,
    IStatService statService,
    IMapper mapper,
    IBlogCommentsService blogCommentsService,
    IHtmlHelperService htmlHelperService,
    IFullTextSearchService fullTextSearchService) : IBlogPostsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<BlogPost, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<BlogPost> _blogPosts = uow.DbSet<BlogPost>();

    public Task<BlogPost?> GetFirstBlogPostAsync()
        => _blogPosts.AsNoTracking().OrderBy(x => x.Id).FirstOrDefaultAsync();

    public Task<BlogPost?> FindBlogPostAsync(string? oldUrl, bool showDeletedItems = false)
        => string.IsNullOrWhiteSpace(oldUrl)
            ? Task.FromResult<BlogPost?>(result: null)
            : _blogPosts.OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.OldUrl == oldUrl && x.IsDeleted == showDeletedItems);

    public async Task<BlogPost?> FindBlogPostAsync(int id, bool showDeletedItems = false)
    {
        var data = await _blogPosts.FindAsync(id);

        if (data is null)
        {
            return null;
        }

        return data.IsDeleted == showDeletedItems ? data : null;
    }

    public Task<BlogPost?> FindBlogPostIncludeTagsAsync(int id, bool showDeletedItems = false)
        => _blogPosts.Include(x => x.Tags)
            .Include(x => x.User)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == showDeletedItems);

    public async Task UpdateStatAsync(int id, bool fromFeed)
    {
        var data = await _blogPosts.FindAsync(id);

        if (data is null)
        {
            return;
        }

        data.EntityStat.NumberOfViews++;

        if (fromFeed)
        {
            data.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public Task<List<BlogPost>> GetLastBlogPostsAsync(int start, int count, bool showDeletedItems = false)
        => _blogPosts.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .OrderByDescending(x => x.Id)
            .Skip(start)
            .Take(count)
            .ToListAsync();

    public Task<int> GetAllBlogPostsCountAsync(bool showDeletedItems = false)
        => _blogPosts.AsNoTracking().CountAsync(x => x.IsDeleted == showDeletedItems);

    public Task<List<BlogPost>> GetLastBlogPostsIncludeAuthorTagsAsync(int count, bool showDeletedItems = false)
        => _blogPosts.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<BlogPostReaction, BlogPost>(fkId, reactionType, fromUserId);

    public async Task<bool> IsTheSameAuthorAsync(int postId, int userId)
    {
        var post = await FindBlogPostAsync(postId);

        return post is not null && userId > 0 && post.UserId is not null && userId == post.UserId.Value;
    }

    public Task<BlogPost?> GetBlogPostIncludeAuthorTagsAsync(int id, bool showDeletedItems = false)
        => _blogPosts.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<BlogPostModel> GetBlogPostLastAndNextPostIncludeAuthorTagsAsync(int id,
        bool showDeletedItems = false)
    {
        // این شماره‌ها پشت سر هم نیستند
        var currentItem = await _blogPosts.Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .Include(x => x.Tags)
            .Include(x => x.Backlogs)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

        await UpdateReadingTimeOfOldPostsAsync(currentItem);

        return new BlogPostModel
        {
            CurrentItem = currentItem,
            NextItem = await _blogPosts.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id > id)
                .OrderBy(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .Include(x => x.Backlogs)
                .FirstOrDefaultAsync(),
            PreviousItem = await _blogPosts.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id < id)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .Include(x => x.Backlogs)
                .FirstOrDefaultAsync()
        };
    }

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsByTagIncludeAuthorAsync(string tag,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _blogPosts.AsNoTracking()
            from t in b.Tags
            where t.Name == tag
            select b;

        query = query.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<List<BlogPost>> GetLastBlogPostsByTermIncludeAuthorsTagsAsync(string term,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        return _blogPosts.AsNoTracking()
            .Where(x => (x.Body.Contains(term) || x.Title.Contains(term)) && x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.Tags)
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsByAuthorIncludeAuthorTagsAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _blogPosts.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Where(x => x.User!.FriendlyName == authorName && x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsByAuthorAsync(string authorName,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _blogPosts.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(x => x.Reactions)
            .Where(x => x.User!.FriendlyName == authorName && x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<List<BlogPost>> GetLastBlogPostsByPopularItemAsNoTrackingAsync(PopularItem item,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        var query = _blogPosts.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems);

        switch (item)
        {
            case PopularItem.ByMonthTopPosts:
                return GetTopBlogPostsOfThisMonthAsync(pageNumber, recordsPerPage, showDeletedItems);
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

    public async Task<List<BlogPost>> GetLastBlogPostsByMonthAsync(int pageNumber, bool showDeletedItems = false)
    {
        var lastRecord = await _blogPosts.AsNoTracking().OrderByDescending(x => x.Id).FirstOrDefaultAsync();

        if (lastRecord is null)
        {
            return new List<BlogPost>();
        }

        var year = int.Parse(lastRecord.Audit.CreatedAtPersian.AsSpan(start: 0, length: 4),
            CultureInfo.InvariantCulture);

        year = year - pageNumber;
        var fromDate = Invariant($"{year}/01/01");
        var toDate = Invariant($"{year}/12/30");

        return await _blogPosts.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.IsDeleted == showDeletedItems)
            .Where(x => x.Audit.CreatedAtPersian.Substring(0, 10).CompareTo(fromDate) >= 0 &&
                        x.Audit.CreatedAtPersian.Substring(0, 10).CompareTo(toDate) <= 0)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public Task<List<BlogPost>> GetTopBlogPostsOfThisMonthAsync(int pageNumber = 0,
        int recordsPerPage = 15,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        var shamsiDate = DateTime.UtcNow.ToPersianYearMonthDay();

        var fromDate =
            Invariant($"{shamsiDate.Year}/{shamsiDate.Month.ToString(format: "00", CultureInfo.InvariantCulture)}/01");

        var toDate =
            Invariant($"{shamsiDate.Year}/{shamsiDate.Month.ToString(format: "00", CultureInfo.InvariantCulture)}/31");

        return _blogPosts.AsNoTracking()
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

    public async Task FixOldBlogImageLinksAsync(string baseUrl)
    {
        var pathGetInvalidPathChars = Path.GetInvalidPathChars();
        var list = _blogPosts.AsEnumerable();

        var blogDir = appFoldersService.ArticleImagesFolderPath;

        foreach (var post in list)
        {
            var html = post.Body;
            var postImages = htmlHelperService.ExtractImagesLinks(html);

            foreach (var image in postImages)
            {
                var fileName = image.Md5Hash();

                var fileExt = ".jpg";

                if (image.IndexOfAny(pathGetInvalidPathChars) == -1)
                {
                    fileExt = Path.GetExtension(image);
                }

                fileExt = ModifyImagesExt(fileExt);
                var finalName = fileName + fileExt;
                var path = Path.Combine(blogDir, finalName);

                if (File.Exists(path))
                {
                    finalName = $"{baseUrl}{ApiUrlsRoutingConstants.File.HttpAny.Image}?name={finalName}";
                    html = html.Replace(image, finalName, StringComparison.OrdinalIgnoreCase);
                }
            }

            if (!string.Equals(post.Body, html, StringComparison.Ordinal))
            {
                post.Body = html;
                await uow.SaveChangesAsync();
            }
        }
    }

    public Task<List<BlogPost>> GetAllPublicPostsOfDateAsync(DateTime date)
        => _blogPosts.AsNoTracking()
            .Include(x => x.User)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt.Year == date.Year && x.Audit.CreatedAt.Month == date.Month &&
                        x.Audit.CreatedAt.Day == date.Day &&
                        (!x.NumberOfRequiredPoints.HasValue || x.NumberOfRequiredPoints.Value == 0))
            .OrderBy(x => x.Id)
            .ToListAsync();

    /// <summary>
    ///     Note: You need to call _tagsService.SaveNewTags first to add the missing tags to the db.
    /// </summary>
    public async Task<BlogPost> SaveBlogPostAsync(BlogPost blogPost,
        IList<BlogPostTag> listOfActualTags,
        bool isEditForm = false)
    {
        ArgumentNullException.ThrowIfNull(blogPost);
        ArgumentNullException.ThrowIfNull(listOfActualTags);

        blogPost.ReadingTimeMinutes = blogPost.Body.MinReadTime();

        if (blogPost.Tags is not null && blogPost.Tags.Count != 0)
        {
            blogPost.Tags.Clear();
        }

        blogPost.Tags = new List<BlogPostTag>();

        foreach (var item in listOfActualTags)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                continue;
            }

            item.Name = item.Name.GetCleanedTag()!;
            blogPost.Tags.Add(item);
        }

        if (!isEditForm)
        {
            _blogPosts.Add(blogPost);
        }

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(blogPost.MapToPostWhatsNewItemModel(siteRootUri: ""));

        return blogPost;
    }

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsIncludeAuthorsTagsAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _blogPosts.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<BlogPost>> GetLastBlogPostsIncludeAuthorsTagsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false)
    {
        var query = _blogPosts.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(BlogPost.Id), [
            .. GridifyMapings.GetDefaultMappings<BlogPost>(), new GridifyMap<BlogPost>
            {
                From = PostsMappingsProfiles.BlogPostTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public async Task PerformPossibleDeleteAsync(int? deleteId)
    {
        if (!deleteId.HasValue)
        {
            return;
        }

        var post = await FindBlogPostAsync(deleteId.Value);

        if (post is null)
        {
            return;
        }

        post.IsDeleted = true;
        await blogCommentsService.MarkAllOfPostCommentsAsDeletedAsync(deleteId.Value);
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(post.MapToPostWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash);

        var listOfActualTags = await tagsService.GetThisPostTagsListAsync(deleteId.Value);
        await statService.RecalculateBlogPostTagsInUseCountsAsync(listOfActualTags);

        if (post.UserId is not null)
        {
            await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(post.UserId.Value);
        }

        await emailsService.WriteArticleSendEmailAsync(new BlogPost
        {
            Title = post.Title,
            Body = Invariant($"حذف مطلب شماره {deleteId.Value} توسط مدیر از سایت "),
            Id = deleteId.Value
        });
    }

    public async Task<BlogPost?> PerformEditAsync(int? editId,
        WriteArticleModel? writeArticleModel,
        ApplicationState? applicationState)
    {
        if (!editId.HasValue || writeArticleModel is null)
        {
            return null;
        }

        var post = await FindBlogPostIncludeTagsAsync(editId.Value);

        if (post is null || !applicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt))
        {
            return null;
        }

        var availableDbTags = await tagsService.SaveNewArticleTagsAsync(writeArticleModel.ArticleTags);
        mapper.Map(writeArticleModel, post);
        await SaveBlogPostAsync(post, availableDbTags, isEditForm: true);

        await statService.RecalculateBlogPostTagsInUseCountsAsync(availableDbTags);

        if (post.UserId is not null)
        {
            await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(post.UserId.Value);
        }

        await emailsService.WriteArticleSendEmailAsync(post);
        fullTextSearchService.AddOrUpdateLuceneDocument(post.MapToPostWhatsNewItemModel(siteRootUri: ""));

        return post;
    }

    public Task IndexBlogPostsAsync()
    {
        var items = _blogPosts.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.User)
            .OrderByDescending(x => x.Id)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToPostWhatsNewItemModel(siteRootUri: "")));
    }

    private static string ModifyImagesExt(string ext)
    {
        //for data like .jpg?w=450&amp;h=291 ...

        string[] extensions = [".jpg", ".gif", ".png", ".bmp"];

        foreach (var ex in extensions)
        {
            if (ext.Contains(ex, StringComparison.OrdinalIgnoreCase))
            {
                return ex;
            }
        }

        return ".jpg";
    }

    private Task UpdateReadingTimeOfOldPostsAsync(BlogPost? currentItem)
    {
        if (currentItem is not { ReadingTimeMinutes: 0 })
        {
            return Task.CompletedTask;
        }

        var minReadTime = currentItem.Body.MinReadTime();

        if (minReadTime == currentItem.ReadingTimeMinutes)
        {
            return Task.CompletedTask;
        }

        currentItem.ReadingTimeMinutes = minReadTime;

        return uow.SaveChangesAsync();
    }
}
