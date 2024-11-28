using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsService : IScopedService
{
    public ValueTask<StackExchangeQuestion?> FindStackExchangeQuestionAsync(int id);

    public Task<List<StackExchangeQuestion>> GetAllPublicStackExchangeQuestionsOfDateAsync(DateTime date);

    public Task<StackExchangeQuestionsListModel> GetCountsAsync();

    public Task<StackExchangeQuestion?> GetLastActiveStackExchangeQuestionAsync();

    public Task<PagedResultModel<StackExchangeQuestion>> GetStackExchangeQuestionsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false);

    public Task<PagedResultModel<StackExchangeQuestion>> GetLastPagedStackExchangeQuestionsAsync(
        DntQueryBuilderModel state,
        bool showDeletedItems = false);

    public Task<PagedResultModel<StackExchangeQuestion>> GetStackExchangeQuestionsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<PagedResultModel<StackExchangeQuestion>> GetLastPagedStackExchangeQuestionByUsernameAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    public Task<QuestionDetailsModel> GetQuestionDetailsAsync(int id, bool showDeletedItems = false);

    public Task MarkAsAnsweredAsync(int id);

    public Task UpdateStatAsync(StackExchangeQuestion? currentItem, bool isFromFeed);

    public Task<StackExchangeQuestion?> GetStackExchangeQuestionAsync(int id, bool showDeletedItems = false);

    public Task MarkAsDeletedAsync(StackExchangeQuestion? question);

    public Task NotifyDeleteChangesAsync(StackExchangeQuestion? question, User? currentUserUser);

    public Task UpdateQuestionsItemAsync(StackExchangeQuestion? question, QuestionModel writeQuestionModel, User? user);

    public Task<StackExchangeQuestion?> AddStackExchangeQuestionAsync(QuestionModel writeQuestionModel, User? user);

    public StackExchangeQuestion AddStackExchangeQuestion(StackExchangeQuestion data);

    public Task NotifyAddOrUpdateChangesAsync(StackExchangeQuestion? question,
        QuestionModel writeQuestionModel,
        User? user);

    public Task IndexStackExchangeQuestionsAsync();
}
