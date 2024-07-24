using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.Surveys.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Surveys.Services;

public class VoteItemsService(IUnitOfWork uow) : IVoteItemsService
{
    private readonly DbSet<SurveyItem> _voteItems = uow.DbSet<SurveyItem>();

    public SurveyItem AddVoteItem(SurveyItem data) => _voteItems.Add(data).Entity;

    public ValueTask<SurveyItem?> FindVoteItemAsync(int id) => _voteItems.FindAsync(id);

    public Task<SurveyItem?> FindVoteItemAndUsersAsync(int id)
        => _voteItems.Include(x => x.Users).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<User>> GetUserRatingsAsync(int itemId, int count = 100)
        => _voteItems.Where(x => x.Id == itemId).SelectMany(x => x.Users).Take(count).OrderBy(x => x.Id).ToListAsync();

    public Task<List<SurveyItem>> FindVoteItemsAsync(int surveyId)
        => _voteItems.Where(x => x.SurveyId == surveyId).ToListAsync();

    public Task<List<SurveyItem>> FindVoteItemsAsync(IList<int>? surveyItemIds)
    {
        if (surveyItemIds is null || surveyItemIds.Count == 0)
        {
            return Task.FromResult<List<SurveyItem>>([]);
        }

        return _voteItems.Include(x => x.Users).Where(x => surveyItemIds.Contains(x.Id)).ToListAsync();
    }

    public void RemoveRange(IList<SurveyItem> items) => _voteItems.RemoveRange(items);

    public void Remove(SurveyItem item) => _voteItems.Remove(item);

    public async Task AddNewSurveyItemsAsync(VoteModel writeSurveyModel, Survey result)
    {
        ArgumentNullException.ThrowIfNull(writeSurveyModel);
        ArgumentNullException.ThrowIfNull(result);

        var items = writeSurveyModel.VoteItems.ConvertMultiLineTextToList();

        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                continue;
            }

            AddVoteItem(new SurveyItem
            {
                Title = item.Trim(),
                SurveyId = result.Id
            });
        }

        await uow.SaveChangesAsync();
    }

    public async Task AddOrUpdateVoteItemsAsync(Survey surveyItem, VoteModel writeSurveyModel)
    {
        ArgumentNullException.ThrowIfNull(surveyItem);
        ArgumentNullException.ThrowIfNull(writeSurveyModel);

        var newVoteItemTitles = writeSurveyModel.VoteItems.ConvertMultiLineTextToList();
        var currentVoteItems = await FindVoteItemsAsync(surveyItem.Id);

        for (var index = 0; index < newVoteItemTitles.Count; index++)
        {
            if (index < currentVoteItems.Count)
            {
                currentVoteItems[index].Title = newVoteItemTitles[index].Trim();
            }
        }

        var itemsToRemoveCount = currentVoteItems.Count - newVoteItemTitles.Count;

        if (itemsToRemoveCount > 0)
        {
            RemoveRange(currentVoteItems.TakeLast(itemsToRemoveCount).ToList());
        }

        var newItemsCount = newVoteItemTitles.Count - currentVoteItems.Count;

        if (newItemsCount > 0)
        {
            foreach (var newItemTitle in newVoteItemTitles.TakeLast(newItemsCount).ToList())
            {
                AddVoteItem(new SurveyItem
                {
                    Title = newItemTitle.Trim(),
                    SurveyId = surveyItem.Id
                });
            }
        }

        await uow.SaveChangesAsync();
    }
}
