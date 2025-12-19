using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVoteItemsService : IScopedService
{
    SurveyItem AddVoteItem(SurveyItem data);

    Task AddOrUpdateVoteItemsAsync(Survey surveyItem, VoteModel writeSurveyModel);

    Task AddNewSurveyItemsAsync(VoteModel writeSurveyModel, Survey result);

    void RemoveRange(IList<SurveyItem> items);

    void Remove(SurveyItem item);

    ValueTask<SurveyItem?> FindVoteItemAsync(int id);

    Task<List<SurveyItem>> FindVoteItemsAsync(int surveyId);

    Task<List<SurveyItem>> FindVoteItemsAsync(IList<int>? surveyItemIds);

    Task<SurveyItem?> FindVoteItemAndUsersAsync(int id);

    Task<List<User>> GetUserRatingsAsync(int itemId, int count = 100);
}
