using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

public interface IQuestionsService : IScopedService
{
    ValueTask<StackExchangeQuestion?> FindStackExchangeQuestionAsync(int id);

    Task<List<StackExchangeQuestion>> GetAllPublicStackExchangeQuestionsOfDateAsync(DateTime date);

    Task<StackExchangeQuestionsListModel> GetCountsAsync();

    Task<StackExchangeQuestion?> GetLastActiveStackExchangeQuestionAsync();

    Task<PagedResultModel<StackExchangeQuestion>> GetStackExchangeQuestionsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false);

    Task<PagedResultModel<StackExchangeQuestion>> GetLastPagedStackExchangeQuestionsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false);

    Task<PagedResultModel<StackExchangeQuestion>> GetStackExchangeQuestionsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<PagedResultModel<StackExchangeQuestion>> GetLastPagedStackExchangeQuestionByUsernameAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false);

    Task<QuestionDetailsModel> GetQuestionDetailsAsync(int id, bool showDeletedItems = false);

    Task MarkAsAnsweredAsync(int id);

    Task UpdateStatAsync(StackExchangeQuestion? currentItem, bool isFromFeed);

    Task<StackExchangeQuestion?> GetStackExchangeQuestionAsync(int id, bool showDeletedItems = false);

    Task MarkAsDeletedAsync(StackExchangeQuestion? question);

    Task NotifyDeleteChangesAsync(StackExchangeQuestion? question, User? currentUserUser);

    Task UpdateQuestionsItemAsync(StackExchangeQuestion? question, QuestionModel? writeQuestionModel, User? user);

    Task<StackExchangeQuestion?> AddStackExchangeQuestionAsync(QuestionModel? writeQuestionModel, User? user);

    StackExchangeQuestion AddStackExchangeQuestion(StackExchangeQuestion data);

    Task NotifyAddOrUpdateChangesAsync(StackExchangeQuestion? question, QuestionModel? writeQuestionModel, User? user);

    Task IndexStackExchangeQuestionsAsync();
}
