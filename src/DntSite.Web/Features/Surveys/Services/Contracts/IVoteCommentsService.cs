using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVoteCommentsService : IScopedService
{
    Task<List<SurveyComment>> GetRootCommentsOfVoteAsync(int postId, int count = 1000, bool showDeletedItems = false);

    ValueTask<SurveyComment?> FindVoteCommentAsync(int commentId);

    Task<SurveyComment?> FindVoteCommentIncludeParentAsync(int commentId);

    ValueTask<Survey?> FindVoteCommentParentAsync(int parentId);

    SurveyComment AddVoteComment(SurveyComment comment);

    Task<List<SurveyComment>> GetLastPagedVoteCommentsAsNoTrackingAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false);

    Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId);

    Task<List<SurveyComment>> GetLastVoteCommentsIncludeBlogPostAndUserAsync(int count, bool showDeletedItems = false);

    Task<List<SurveyComment>> GetLastVoteCommentsIncludeBlogPostAndUserAsync(int voteId,
        int count,
        bool showDeletedItems = false);

    Task<PagedResultModel<SurveyComment>> GetPagedLastVoteCommentsAsync(int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<List<SurveyComment>> GetPagedLastVoteCommentsIncludeBlogPostAndUserAsync(int voteId,
        int pageNumber,
        int recordsPerPage = 15,
        bool showDeletedItems = false);

    Task<PagedResultModel<SurveyComment>> GetLastPagedVotesCommentsAsNoTrackingAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task DeleteCommentAsync(int? modelFormCommentId);

    Task EditReplyAsync(int? modelFormCommentId, string modelComment);

    Task AddReplyAsync(int? modelFormCommentId,
        int modelFormPostId,
        string modelComment,
        int currentUserUserId,
        bool userIsRestricted);
}
