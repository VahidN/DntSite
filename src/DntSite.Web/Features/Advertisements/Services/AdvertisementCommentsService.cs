using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Advertisements.Services;

public class AdvertisementCommentsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IAntiXssService antiXssService,
    IAdvertisementsEmailsService emailsService,
    IStatService statService) : IAdvertisementCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<AdvertisementComment, object?>>> CustomOrders =
        new()
        {
            [PagerSortBy.Date] = x => x.Id,
            [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
            [PagerSortBy.Title] = x => x.Parent.Title,
            [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
            [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
            [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
        };

    private readonly DbSet<AdvertisementComment> _advertisementComments = uow.DbSet<AdvertisementComment>();

    public ValueTask<AdvertisementComment?> FindAdvertisementCommentAsync(int id)
        => _advertisementComments.FindAsync(id);

    public async Task<List<AdvertisementComment>> GetRootCommentsOfAdvertisementAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false)
    {
        var advertisementComments = await _advertisementComments.AsNoTracking()
            .Include(x => x.Reactions)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Where(x => x.ParentId == postId && x.IsDeleted == showDeletedItems)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return advertisementComments.ToSelfReferencingTree();
    }

    public Task<List<AdvertisementComment>> GetLastPagedAdvertisementCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;
        var now = DateTime.UtcNow;

        return _advertisementComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted && (!x.Parent.DueDate.HasValue || x.Parent.DueDate.Value >= now))
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<List<AdvertisementComment>> GetLastPagedDeletedAdvertisementCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8)
    {
        var skipRecords = pageNumber * recordsPerPage;
        var now = DateTime.UtcNow;

        return _advertisementComments.AsNoTracking()
            .Where(x => x.IsDeleted)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted && (!x.Parent.DueDate.HasValue || x.Parent.DueDate.Value >= now))
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<AdvertisementCommentReaction, AdvertisementComment>(fkId, reactionType,
            fromUserId);

    public async Task MarkAllOfAdvertisementCommentsAsDeletedAsync(int id)
    {
        var list = await _advertisementComments.Where(x => x.ParentId == id).ToListAsync();

        if (list.Count == 0)
        {
            return;
        }

        foreach (var item in list)
        {
            item.IsDeleted = true;
        }
    }

    public AdvertisementComment AddAdvertisementComment(AdvertisementComment comment)
        => _advertisementComments.Add(comment).Entity;

    public Task<PagedResultModel<AdvertisementComment>> GetPagedLastAdvertisementCommentsIncludeBlogPostAndUserAsync(
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var now = DateTime.UtcNow;

        var query = _advertisementComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted && (!x.Parent.DueDate.HasValue || x.Parent.DueDate.Value >= now));

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<AdvertisementComment>> GetLastPagedAdvertisementsCommentsAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var now = DateTime.UtcNow;

        var query = _advertisementComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted && (!x.Parent.DueDate.HasValue || x.Parent.DueDate.Value >= now));

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task DeleteCommentAsync(int? modelFormCommentId)
    {
        if (modelFormCommentId is null)
        {
            return;
        }

        var comment = await FindAdvertisementCommentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.IsDeleted = true;
        await uow.SaveChangesAsync();

        await UpdateStatAsync(comment);
    }

    public async Task EditReplyAsync(int? modelFormCommentId, string modelComment)
    {
        if (modelFormCommentId is null)
        {
            return;
        }

        var comment = await FindAdvertisementCommentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(modelComment);
        await uow.SaveChangesAsync();

        await emailsService.AdvertisementCommentSendEmailToAdminsAsync(comment);
    }

    public async Task AddReplyAsync(int? modelFormCommentId,
        int modelFormPostId,
        string modelComment,
        int currentUserUserId)
    {
        if (string.IsNullOrWhiteSpace(modelComment))
        {
            return;
        }

        var comment = new AdvertisementComment
        {
            ParentId = modelFormPostId,
            ReplyId = modelFormCommentId,
            Body = antiXssService.GetSanitizedHtml(modelComment),
            UserId = currentUserUserId
        };

        var result = AddAdvertisementComment(comment);
        await uow.SaveChangesAsync();

        await SendEmailsAsync(result);
        await UpdateStatAsync(result);
    }

    private async Task UpdateStatAsync(AdvertisementComment comment)
    {
        await statService.RecalculateThisAdvertisementCommentsCountsAsync(comment.ParentId);
        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(comment.UserId ?? 0);
    }

    private async Task SendEmailsAsync(AdvertisementComment result)
    {
        await emailsService.AdvertisementCommentSendEmailToAdminsAsync(result);
        await emailsService.AdvertisementCommentSendEmailToWritersAsync(result);
        await emailsService.AdvertisementCommentSendEmailToPersonAsync(result);
    }
}
