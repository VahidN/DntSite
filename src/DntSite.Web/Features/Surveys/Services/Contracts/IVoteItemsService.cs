using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Surveys.Services.Contracts;

public interface IVoteItemsService : IScopedService
{
    public SurveyItem AddVoteItem(SurveyItem data);

    public Task AddOrUpdateVoteItemsAsync(Survey surveyItem, VoteModel writeSurveyModel);

    public Task AddNewSurveyItemsAsync(VoteModel writeSurveyModel, Survey result);

    public void RemoveRange(IList<SurveyItem> items);

    public void Remove(SurveyItem item);

    public ValueTask<SurveyItem?> FindVoteItemAsync(int id);

    public Task<List<SurveyItem>> FindVoteItemsAsync(int surveyId);

    public Task<List<SurveyItem>> FindVoteItemsAsync(IList<int>? surveyItemIds);

    public Task<SurveyItem?> FindVoteItemAndUsersAsync(int id);

    public Task<List<User>> GetUserRatingsAsync(int itemId, int count = 100);
}
