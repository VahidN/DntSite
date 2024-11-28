using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVotesService : IScopedService
{
    public Task<Survey?> GetLastActiveVoteAsync();

    public ValueTask<Survey?> FindVoteAsync(int id);

    public Task<Survey?> FindVoteIncludeResultsAsync(int id);

    public Task<Survey?> GetSurveyAsync(int id, bool showDeletedItems = false);

    public Survey AddVote(Survey data);

    public Task<PagedResultModel<Survey>> GetVotesListAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Survey>> GetLastPagedSurveysAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    public Task<PagedResultModel<Survey>> GetUserVotesListAsync(int pageNumber,
        int userId,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<Survey>> GetLastVotesByTagAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<BlogVoteModel> GetBlogVoteLastAndNextPostAsync(int id, bool showDeletedItems = false);

    public Task<PagedResultModel<Survey>> GetLastVotesByUserAsync(string userName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public void UpdateOrders(Survey vote, string[] items);

    public Task<bool> CanUserVoteAsync(Survey? vote, User? user);

    public Task MarkAsDeletedAsync(Survey? surveyItem);

    public Task NotifyDeleteChangesAsync(Survey? surveyItem, User? currentUserUser);

    public Task ApplyVoteAsync(int surveyId, IList<int>? surveyItemIds, User? user);

    public Task UpdateSurveyAsync(Survey? surveyItem, VoteModel writeSurveyModel, User? user);

    public Task<Survey?> AddNewsSurveyAsync(VoteModel writeSurveyModel, User? user);

    public Task NotifyAddOrUpdateChangesAsync(Survey? surveyItem, VoteModel writeSurveyModel, User? user);

    public Task IndexSurveysAsync();
}
