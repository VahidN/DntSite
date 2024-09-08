using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Persistence.Utils;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Services;

public class QuestionsCommentsService(
    IUnitOfWork uow,
    IStatService statService,
    IAppAntiXssService antiXssService,
    IQuestionsEmailsService questionsEmailsService,
    IFullTextSearchService fullTextSearchService) : IQuestionsCommentsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<StackExchangeQuestionComment, object?>>>
        CustomOrders = new()
        {
            [PagerSortBy.Date] = x => x.Id,
            [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
            [PagerSortBy.Title] = x => x.Parent.Title,
            [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
            [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
            [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
        };

    private readonly DbSet<StackExchangeQuestionComment> _questionComments = uow.DbSet<StackExchangeQuestionComment>();

    public Task<PagedResultModel<StackExchangeQuestionComment>> GetLastPagedStackExchangeQuestionCommentsOfUserAsync(
        string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _questionComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<StackExchangeQuestionComment>>
        GetLastPagedStackExchangeQuestionCommentsAsNoTrackingAsync(int pageNumber,
            int recordsPerPage = 8,
            bool showDeletedItems = false,
            PagerSortBy pagerSortBy = PagerSortBy.Date,
            bool isAscending = false)
    {
        var query = _questionComments.AsNoTracking()
            .Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Where(x => !x.Parent.IsDeleted);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task<List<StackExchangeQuestionComment>> GetRootCommentsOfQuestionAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false)
    {
        var comments = await _questionComments.AsNoTracking()
            .Include(x => x.Parent)
            .Include(x => x.Reactions)
            .Include(x => x.User)
            .Where(x => x.ParentId == postId && x.IsDeleted == showDeletedItems && !x.Parent.IsDeleted)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync();

        return comments.ToSelfReferencingTree();
    }

    public ValueTask<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentAsync(int commentId)
        => _questionComments.FindAsync(commentId);

    public Task<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentIncludeParentAsync(int commentId)
        => _questionComments.Include(x => x.Parent).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == commentId);

    public async Task DeleteCommentAsync(int? modelFormCommentId)
    {
        if (modelFormCommentId is null)
        {
            return;
        }

        var comment = await FindStackExchangeQuestionCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash);

        await statService.RecalculateThisStackExchangeQuestionCommentsCountsAsync(comment.ParentId);
    }

    public async Task EditReplyAsync(int? modelFormCommentId, string modelComment)
    {
        if (modelFormCommentId is null)
        {
            return;
        }

        var comment = await FindStackExchangeQuestionCommentIncludeParentAsync(modelFormCommentId.Value);

        if (comment is null)
        {
            return;
        }

        comment.Body = antiXssService.GetSanitizedHtml(modelComment);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(comment.MapToWhatsNewItemModel(siteRootUri: ""));

        await questionsEmailsService.PostQuestionCommentReplySendEmailToAdminsAsync(comment);
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

        var comment = new StackExchangeQuestionComment
        {
            ParentId = modelFormPostId,
            ReplyId = modelFormCommentId,
            Body = antiXssService.GetSanitizedHtml(modelComment),
            UserId = currentUserUserId
        };

        var result = AddStackExchangeQuestionComment(comment);
        await uow.SaveChangesAsync();

        await SetParentAsync(result, modelFormPostId);
        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        await SendEmailsAsync(result);
        await UpdateStatAsync(modelFormPostId, currentUserUserId);
    }

    public StackExchangeQuestionComment AddStackExchangeQuestionComment(StackExchangeQuestionComment comment)
        => _questionComments.Add(comment).Entity;

    public async Task MarkQuestionCommentAsAnswerAsync(StackExchangeQuestionComment? questionComment, bool isAnswer)
    {
        if (questionComment is null)
        {
            return;
        }

        var comment = await FindStackExchangeQuestionCommentIncludeParentAsync(questionComment.Id);

        if (comment is null)
        {
            return;
        }

        comment.IsAnswer = isAnswer;
        comment.Parent.IsAnswered = isAnswer;

        await uow.SaveChangesAsync();
    }

    public Task IndexStackExchangeQuestionCommentsAsync()
    {
        var items = _questionComments.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Parent)
            .Include(x => x.User)
            .Where(x => !x.Parent.IsDeleted)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }

    private async Task SetParentAsync(StackExchangeQuestionComment result, int modelFormPostId)
        => result.Parent = await uow.DbSet<StackExchangeQuestion>().FindAsync(modelFormPostId) ??
                           new StackExchangeQuestion
                           {
                               Id = modelFormPostId,
                               Title = "",
                               Description = ""
                           };

    private async Task SendEmailsAsync(StackExchangeQuestionComment result)
    {
        await questionsEmailsService.PostQuestionCommentReplySendEmailToAdminsAsync(result);
        await questionsEmailsService.PostQuestionCommentsReplySendEmailToWritersAsync(result);
        await questionsEmailsService.PostQuestionCommentsReplySendEmailToPersonAsync(result);
    }

    private async Task UpdateStatAsync(int blogPostId, int userId)
    {
        await statService.RecalculateThisStackExchangeQuestionCommentsCountsAsync(blogPostId);
        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(userId);
    }
}
