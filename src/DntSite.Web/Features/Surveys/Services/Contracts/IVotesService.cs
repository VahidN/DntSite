using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVotesService : IScopedService
{
    Task<Survey?> GetLastActiveVoteAsync();

    ValueTask<Survey?> FindVoteAsync(int id);

    Task<Survey?> FindVoteIncludeResultsAsync(int id);

    Task<Survey?> GetSurveyAsync(int id, bool showDeletedItems = false);

    Survey AddVote(Survey data);

    Task<PagedResultModel<Survey>> GetVotesListAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Survey>> GetLastPagedSurveysAsync(DntQueryBuilderModel state, bool showDeletedItems = false);

    Task<PagedResultModel<Survey>> GetUserVotesListAsync(int pageNumber,
        int userId,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<Survey>> GetLastVotesByTagAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<BlogVoteModel> GetBlogVoteLastAndNextPostAsync(int id, bool showDeletedItems = false);

    Task<PagedResultModel<Survey>> GetLastVotesByUserAsync(string userName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    void UpdateOrders(Survey vote, string[] items);

    Task<bool> CanUserVoteAsync(Survey? vote, User? user);

    Task MarkAsDeletedAsync(Survey? surveyItem);

    Task NotifyDeleteChangesAsync(Survey? surveyItem, User? currentUserUser);

    Task ApplyVoteAsync(int surveyId, IList<int>? surveyItemIds, User? user);

    Task UpdateSurveyAsync(Survey? surveyItem, VoteModel writeSurveyModel, User? user);

    Task<Survey?> AddNewsSurveyAsync(VoteModel writeSurveyModel, User? user);

    Task NotifyAddOrUpdateChangesAsync(Survey? surveyItem, VoteModel writeSurveyModel, User? user);

    Task IndexSurveysAsync();
}
