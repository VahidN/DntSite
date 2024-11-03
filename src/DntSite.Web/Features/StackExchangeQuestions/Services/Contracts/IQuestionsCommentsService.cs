using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsCommentsService : IScopedService
{
    Task<PagedResultModel<StackExchangeQuestionComment>> GetLastPagedStackExchangeQuestionCommentsOfUserAsync(
        string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<StackExchangeQuestionComment>> GetLastPagedStackExchangeQuestionCommentsAsNoTrackingAsync(
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<List<StackExchangeQuestionComment>> GetRootCommentsOfQuestionAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    ValueTask<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentAsync(int commentId);

    Task<StackExchangeQuestionComment?> FindStackExchangeQuestionCommentIncludeParentAsync(int commentId);

    Task DeleteCommentAsync(int? modelFormCommentId);

    Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    StackExchangeQuestionComment AddStackExchangeQuestionComment(StackExchangeQuestionComment comment);

    Task AddReplyAsync(int? modelFormCommentId,
        int modelFormPostId,
        string modelComment,
        int currentUserUserId,
        bool userIsRestricted);

    Task<StackExchangeQuestionComment?> MarkQuestionCommentAsAnswerAsync(StackExchangeQuestionComment? questionComment,
        bool isAnswer);

    Task NotifyQuestionCommentIsApprovedAsync(StackExchangeQuestionComment? comment);

    Task IndexStackExchangeQuestionCommentsAsync();
}
