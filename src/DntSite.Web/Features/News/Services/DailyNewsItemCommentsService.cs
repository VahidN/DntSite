using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.ModelsMappings;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.News.Services;

public class DailyNewsItemCommentsService(
    IUnitOfWork uow,
    IAppAntiXssService antiXssService,
    IStatService statService,
    IDailyNewsEmailsService dailyNewsEmailsService,
    IUserRatingsService userRatingsService,
    IFullTextSearchService fullTextSearchService) : IDailyNewsItemCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<DailyNewsItemComment, object?>>> CustomOrders =
        new()
        {
            [PagerSortBy.Date] = x => x.Id,
            [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
            [PagerSortBy.Title] = x => x.Parent.Title,
            [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
            [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
            [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
        };

    private readonly DbSet<DailyNewsItemComment> _dailyNewsItemComments = uow.DbSet<DailyNewsItemComment>();

    public async Task<List<DailyNewsItemComment>> GetRootCommentsOfNewsAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false)
    {
        var dailyNewsItemComments = await _dailyNewsItemComments.AsNoTracking()
            .Include(x => x.Reactions)
            .Include(x => x.User)
            .Where(x => x.ParentId == postId && x.IsDeleted == showDeletedItems && !x.Parent.IsDeleted)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return dailyNewsItemComments.ToSelfReferencingTree();
    }

    public ValueTask<DailyNewsItemComment?> FindBlogNewsCommentAsync(int commentId)
        => _dailyNewsItemComments.FindAsync(commentId);

    public Task<DailyNewsItemComment?> FindBlogNewsCommentIncludeParentAsync(int commentId)
        => _dailyNewsItemComments.Include(x => x.Parent).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == commentId);

    public DailyNewsItemComment AddBlogNewsComment(DailyNewsItemComment comment)
        => _dailyNewsItemComments.Add(comment).Entity;

    public Task<PagedResultModel<DailyNewsItemComment>> GetLastPagedBlogNewsCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _dailyNewsItemComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<DailyNewsItemCommentReaction, DailyNewsItemComment>(fkId, reactionType,
            fromUserId);

    public Task<List<DailyNewsItemComment>>
        GetLastBlogNewsCommentsIncludeBlogPostAndUserAsync(int count, bool showDeletedItems = false)
        => _dailyNewsItemComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<PagedResultModel<DailyNewsItemComment>> GetLastPagedDailyNewsItemCommentsOfUserAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _dailyNewsItemComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task DeleteCommentAsync(int? commentId)
    {
        if (commentId is null)
        {
            return;
        }

        var comment = await FindBlogNewsCommentIncludeParentAsync(commentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash);

        await statService.RecalculateThisNewsPostCommentsCountsAsync(comment.ParentId);
    }

    public async Task EditReplyAsync(int? commentId, string message)
    {
        if (commentId is null)
        {
            return;
        }

        var comment = await FindBlogNewsCommentIncludeParentAsync(commentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(message);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: ""));

        await dailyNewsEmailsService.PostNewsReplySendEmailToAdminsAsync(comment);
    }

    public async Task AddReplyAsync(int? replyId, int blogPostId, string message, int userId)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        var comment = new DailyNewsItemComment
        {
            ParentId = blogPostId,
            ReplyId = replyId,
            Body = antiXssService.GetSanitizedHtml(message),
            UserId = userId
        };

        var result = AddBlogNewsComment(comment);
        await uow.SaveChangesAsync();

        await SetParentAsync(result, blogPostId);
        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        await SendEmailsAsync(result);
        await UpdateStatAsync(blogPostId, userId);
    }

    public Task IndexDailyNewsItemCommentsAsync()
    {
        var items = _dailyNewsItemComments.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }

    private async Task SetParentAsync(DailyNewsItemComment result, int modelFormPostId)
        => result.Parent = await uow.DbSet<DailyNewsItem>().FindAsync(modelFormPostId) ?? new DailyNewsItem
        {
            Id = modelFormPostId,
            Title = "",
            Url = ""
        };

    private async Task SendEmailsAsync(DailyNewsItemComment result)
    {
        await dailyNewsEmailsService.PostNewsReplySendEmailToAdminsAsync(result);
        await dailyNewsEmailsService.PostNewsReplySendEmailToWritersAsync(result);
        await dailyNewsEmailsService.PostNewsReplySendEmailToPersonAsync(result);
    }

    private async Task UpdateStatAsync(int blogPostId, int userId)
    {
        await statService.RecalculateThisNewsPostCommentsCountsAsync(blogPostId);
        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(userId);
    }
}
