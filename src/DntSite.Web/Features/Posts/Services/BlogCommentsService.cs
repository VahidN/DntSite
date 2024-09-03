using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.ModelsMappings;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Posts.Services;

public class BlogCommentsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IStatService statService,
    IBlogCommentsEmailsService blogCommentsEmailsService,
    IAntiXssService antiXssService,
    IFullTextSearchService fullTextSearchService) : IBlogCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<BlogPostComment, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Parent.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<BlogPostComment> _blogComments = uow.DbSet<BlogPostComment>();
    private readonly DbSet<BlogPost> _blogPosts = uow.DbSet<BlogPost>();

    public ValueTask<BlogPostComment?> FindBlogCommentAsync(int id) => _blogComments.FindAsync(id);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<BlogPostCommentReaction, BlogPostComment>(fkId, reactionType, fromUserId);

    public async Task<string> FindBlogCommentPostTitleAsync(int commentId)
    {
        var comment = await _blogComments.FindAsync(commentId);

        if (comment is null)
        {
            return string.Empty;
        }

        var post = await _blogPosts.FindAsync(comment.ParentId);

        if (post is null)
        {
            return string.Empty;
        }

        return post.Title;
    }

    public ValueTask<BlogPost?> FindCommentPostAsync(int blogPostId) => _blogPosts.FindAsync(blogPostId);

    public Task<List<BlogPostComment>> GetLastBlogCommentsOnlyAsync(int count, bool showDeletedItems = false)
        => _blogComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<List<BlogPostComment>>
        GetLastBlogCommentsIncludeBlogPostAndUserAsync(int count, bool showDeletedItems = false)
        => _blogComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public async Task<List<BlogPostComment>> GetRootCommentsOfPostAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false)
    {
        var blogPostComments = await _blogComments.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => x.ParentId == postId && x.IsDeleted == showDeletedItems)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return blogPostComments.ToSelfReferencingTree();
    }

    public async Task MarkAllOfPostCommentsAsDeletedAsync(int postId)
    {
        var list = await _blogComments.Where(x => x.ParentId == postId).ToListAsync();

        if (list.Count == 0)
        {
            return;
        }

        foreach (var item in list)
        {
            item.IsDeleted = true;
        }
    }

    public BlogPostComment AddBlogComment(BlogPostComment data) => _blogComments.Add(data).Entity;

    public Task<PagedResultModel<BlogPostComment>> GetLastPagedBlogCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _blogComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<BlogPostComment>> GetLastPagedBlogCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _blogComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<BlogPostComment>> GetMyPostsLastCommentsAsNoTrackingAsync(int userId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from post in _blogPosts
            where post.UserId == userId && post.IsDeleted == showDeletedItems
            from comment in post.Comments
            where comment.IsDeleted == showDeletedItems
            select comment;

        query = query.Include(x => x.User).AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task DeleteCommentAsync(int? commentId)
    {
        if (commentId is null)
        {
            return;
        }

        var comment = await FindBlogCommentAsync(commentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash);

        await statService.RecalculateThisBlogPostCommentsCountsAsync(comment.ParentId);
    }

    public async Task EditReplyAsync(int? commentId, string message)
    {
        if (commentId is null)
        {
            return;
        }

        var comment = await FindBlogCommentAsync(commentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(message);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: ""));

        await blogCommentsEmailsService.PostReplySendEmailToAdminsAsync(comment);
    }

    public async Task AddReplyAsync(int? replyId, int blogPostId, string message, int userId)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        var comment = new BlogPostComment
        {
            ParentId = blogPostId,
            ReplyId = replyId,
            Body = antiXssService.GetSanitizedHtml(message),
            UserId = userId
        };

        var result = AddBlogComment(comment);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        await SendEmailsAsync(result);
        await UpdateStatAsync(blogPostId, userId);
    }

    public Task IndexBlogPostCommentsAsync()
    {
        var items = _blogComments.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }

    private async Task SendEmailsAsync(BlogPostComment result)
    {
        await blogCommentsEmailsService.PostReplySendEmailToAdminsAsync(result);
        await blogCommentsEmailsService.PostReplySendEmailToWritersAsync(result);
        await blogCommentsEmailsService.PostReplySendEmailToPersonAsync(result);
    }

    private async Task UpdateStatAsync(int blogPostId, int userId)
    {
        await statService.RecalculateThisBlogPostCommentsCountsAsync(blogPostId);
        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(userId);
    }
}
