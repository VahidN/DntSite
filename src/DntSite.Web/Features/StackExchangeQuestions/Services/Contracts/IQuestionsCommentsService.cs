using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsCommentsService : IScopedService
{
    public Task<PagedResultModel<StackExchangeQuestionComment>> GetLastPagedStackExchangeQuestionCommentsOfUserAsync(
        string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<StackExchangeQuestionComment>>
        GetLastPagedStackExchangeQuestionCommentsAsNoTrackingAsync(int pageNumber,
            int recordsPerPage = 8,
            bool showDeletedItems = false,
            PagerSortBy pagerSortBy = PagerSortBy.Date,
            bool isAscending = false);

    public Task<List<StackExchangeQuestionComment>> GetRootCommentsOfQuestionAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    public ValueTask<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentAsync(int commentId);

    public Task<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentIncludeParentAsync(int commentId);

    public Task DeleteCommentAsync(int? modelFormCommentId);

    public Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    public StackExchangeQuestionComment AddStackExchangeQuestionComment(StackExchangeQuestionComment comment);

    public Task AddReplyAsync(int? modelFormCommentId,
        int modelFormPostId,
        string modelComment,
        int currentUserUserId,
        bool userIsRestricted);

    public Task<StackExchangeQuestionComment?> MarkQuestionCommentAsAnswerAsync(
        StackExchangeQuestionComment? questionComment,
        bool isAnswer);

    public Task NotifyQuestionCommentIsApprovedAsync(StackExchangeQuestionComment? comment);

    public Task IndexStackExchangeQuestionCommentsAsync();
}
