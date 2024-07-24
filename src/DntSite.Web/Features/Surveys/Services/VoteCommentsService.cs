using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Services;

public class VoteCommentsService(
    IUnitOfWork uow,
    IUserRatingsService userRatingsService,
    IStatService statService,
    IAntiXssService antiXssService,
    IVotesEmailsService votesEmailsService) : IVoteCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<SurveyComment, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Parent.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<SurveyComment> _voteComments = uow.DbSet<SurveyComment>();

    public async Task<List<SurveyComment>> GetRootCommentsOfVoteAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false)
    {
        var surveyComments = await _voteComments.AsNoTracking()
            .Include(x => x.Reactions)
            .Include(x => x.User)
            .Where(x => x.ParentId == postId && x.IsDeleted == showDeletedItems && !x.Parent.IsDeleted)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return surveyComments.ToSelfReferencingTree();
    }

    public Task<List<SurveyComment>> GetLastPagedVoteCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        return _voteComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<SurveyCommentReaction, SurveyComment>(fkId, reactionType, fromUserId);

    public Task<List<SurveyComment>>
        GetLastVoteCommentsIncludeBlogPostAndUserAsync(int count, bool showDeletedItems = false)
        => _voteComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<List<SurveyComment>>
        GetLastVoteCommentsIncludeBlogPostAndUserAsync(int voteId, int count, bool showDeletedItems = false)
        => _voteComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => x.ParentId == voteId && !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Take(count)
            .ToListAsync();

    public Task<PagedResultModel<SurveyComment>> GetPagedLastVoteCommentsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _voteComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<List<SurveyComment>> GetPagedLastVoteCommentsIncludeBlogPostAndUserAsync(int voteId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false)
    {
        var skipRecords = pageNumber * recordsPerPage;

        return _voteComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => x.ParentId == voteId && !x.Parent.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Skip(skipRecords)
            .Take(recordsPerPage)
            .ToListAsync();
    }

    public Task<PagedResultModel<SurveyComment>> GetLastPagedVotesCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _voteComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public ValueTask<SurveyComment?> FindVoteCommentAsync(int commentId) => _voteComments.FindAsync(commentId);

    public Task<SurveyComment?> FindVoteCommentIncludeParentAsync(int commentId)
        => _voteComments.Include(x => x.Parent).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == commentId);

    public SurveyComment AddVoteComment(SurveyComment comment) => _voteComments.Add(comment).Entity;

    public async Task DeleteCommentAsync(int? modelFormCommentId)
    {
        if (modelFormCommentId is null)
        {
            return;
        }

        var comment = await FindVoteCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.IsDeleted = true;
        await uow.SaveChangesAsync();

        await statService.RecalculateThisVoteCommentsCountsAsync(comment.ParentId);
    }

    public async Task EditReplyAsync(int? modelFormCommentId, string modelComment)
    {
        if (modelFormCommentId is null)
        {
            return;
        }

        var comment = await FindVoteCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(modelComment);
        await uow.SaveChangesAsync();

        await votesEmailsService.VoteCommentSendEmailToAdminsAsync(comment);
    }

    public async Task AddReplyAsync(int? modelFormCommentId,
        int modelFormPostId,
        string modelComment,
        int currentUserUserId,
        bool userIsRestricted)
    {
        if (string.IsNullOrWhiteSpace(modelComment))
        {
            return;
        }

        var comment = new SurveyComment
        {
            ParentId = modelFormPostId,
            ReplyId = modelFormCommentId,
            Body = antiXssService.GetSanitizedHtml(modelComment),
            UserId = currentUserUserId
        };

        var result = AddVoteComment(comment);
        await uow.SaveChangesAsync();

        await SendEmailsAsync(result);
        await UpdateStatAsync(modelFormPostId, currentUserUserId);
    }

    public ValueTask<Survey?> FindVoteCommentParentAsync(int parentId) => uow.DbSet<Survey>().FindAsync(parentId);

    private async Task SendEmailsAsync(SurveyComment result)
    {
        result.Parent = await FindVoteCommentParentAsync(result.ParentId) ??
                        throw new InvalidOperationException(message: "SurveyComment's parent is null.");

        await votesEmailsService.VoteCommentSendEmailToAdminsAsync(result);
        await votesEmailsService.VoteCommentSendEmailToWritersAsync(result);
        await votesEmailsService.VoteCommentSendEmailToPersonAsync(result);
    }

    private async Task UpdateStatAsync(int blogPostId, int userId)
    {
        await statService.RecalculateThisVoteCommentsCountsAsync(blogPostId);
        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(userId);
    }
}
