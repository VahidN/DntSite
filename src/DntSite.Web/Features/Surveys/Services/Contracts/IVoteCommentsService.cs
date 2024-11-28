using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVoteCommentsService : IScopedService
{
    public Task<List<SurveyComment>> GetRootCommentsOfVoteAsync(int postId,
        int count = 1000,
        bool showDeletedItems = false);

    public ValueTask<SurveyComment?> FindVoteCommentAsync(int commentId);

    public Task<SurveyComment?> FindVoteCommentIncludeParentAsync(int commentId);

    public ValueTask<Survey?> FindVoteCommentParentAsync(int parentId);

    public SurveyComment AddVoteComment(SurveyComment comment);

    public Task<List<SurveyComment>> GetLastPagedVoteCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false);

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    public Task<List<SurveyComment>> GetLastVoteCommentsIncludeBlogPostAndUserAsync(int count,
        bool showDeletedItems = false);

    public Task<List<SurveyComment>> GetLastVoteCommentsIncludeBlogPostAndUserAsync(int voteId,
        int count,
        bool showDeletedItems = false);

    public Task<PagedResultModel<SurveyComment>> GetPagedLastVoteCommentsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<List<SurveyComment>> GetPagedLastVoteCommentsIncludeBlogPostAndUserAsync(int voteId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    public Task<PagedResultModel<SurveyComment>> GetLastPagedVotesCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task DeleteCommentAsync(int? modelFormCommentId);

    public Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    public Task AddReplyAsync(int? modelFormCommentId,
        int modelFormPostId,
        string modelComment,
        int currentUserUserId,
        bool userIsRestricted);

    public Task IndexSurveyCommentsAsync();
}
