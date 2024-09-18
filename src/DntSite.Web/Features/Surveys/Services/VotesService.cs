using AutoMapper;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.Models;
using DntSite.Web.Features.Surveys.ModelsMappings;
using DntSite.Web.Features.Surveys.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Surveys.Services;

public class VotesService(
    IUnitOfWork uow,
    IVotesEmailsService votesEmailsService,
    IStatService statService,
    ITagsService tagsService,
    IVoteItemsService voteItemsService,
    IMapper mapper,
    IEmailsFactoryService emailsFactoryService,
    IFullTextSearchService fullTextSearchService) : IVotesService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<Survey, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<Survey> _votes = uow.DbSet<Survey>();

    public Survey AddVote(Survey data) => _votes.Add(data).Entity;

    public ValueTask<Survey?> FindVoteAsync(int id) => _votes.FindAsync(id);

    public Task<Survey?> FindVoteIncludeResultsAsync(int id)
        => _votes.Where(x => x.Id == id)
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .Include(x => x.SurveyItems)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task<BlogVoteModel> GetBlogVoteLastAndNextPostAsync(int id, bool showDeletedItems = false)
        =>

            // این شماره‌ها پشت سر هم نیستند
            new()
            {
                CurrentItem =
                    await _votes.AsNoTracking()
                        .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
                        .Include(x => x.Tags)
                        .Include(x => x.User)
                        .Include(x => x.SurveyItems)
                        .Include(x => x.Reactions)
                        .OrderBy(x => x.Id)
                        .FirstOrDefaultAsync(),
                NextItem = await _votes.AsNoTracking()
                    .Where(x => x.IsDeleted == showDeletedItems && x.Id > id)
                    .OrderBy(x => x.Id)
                    .Include(x => x.Tags)
                    .Include(x => x.User)
                    .Include(x => x.SurveyItems)
                    .Include(x => x.Reactions)
                    .FirstOrDefaultAsync(),
                PreviousItem = await _votes.AsNoTracking()
                    .Where(x => x.IsDeleted == showDeletedItems && x.Id < id)
                    .OrderByDescending(x => x.Id)
                    .Include(x => x.Tags)
                    .Include(x => x.User)
                    .Include(x => x.SurveyItems)
                    .Include(x => x.Reactions)
                    .FirstOrDefaultAsync()
            };

    public async Task<bool> CanUserVoteAsync(Survey? vote, User? user)
    {
        if (vote is null || user is not { IsActive: true })
        {
            return false;
        }

        return !await _votes.Include(x => x.SurveyItems)
            .Where(x => x.Id == vote.Id)
            .SelectMany(x => x.SurveyItems)
            .Include(x => x.Users)
            .AnyAsync(x => x.Users.Any(u => u.Id == user.Id));
    }

    public Task<Survey?> GetLastActiveVoteAsync()
        => _votes.AsNoTracking()
            .Include(vote => vote.User)
            .OrderByDescending(vote => vote.Id)
            .FirstOrDefaultAsync(vote => !vote.IsDeleted);

    public void UpdateOrders(Survey vote, string[] items)
    {
        ArgumentNullException.ThrowIfNull(vote);
        ArgumentNullException.ThrowIfNull(items);

        var itemIds = new List<int>();

        foreach (var item in items)
        {
            var value = item.Replace(oldValue: "item-row-", string.Empty, StringComparison.OrdinalIgnoreCase);
            itemIds.Add(int.Parse(value, CultureInfo.InvariantCulture));
        }

        var voteItems = vote.SurveyItems.OrderBy(x => x.Order).ToList();
        var order = 0;

        foreach (var itemId in itemIds)
        {
            order++;
            var voteItem = voteItems.Find(x => x.Id == itemId);

            if (voteItem is null)
            {
                continue;
            }

            voteItem.Order = order;
        }
    }

    public Task<PagedResultModel<Survey>> GetLastVotesByUserAsync(string userName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _votes.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(x => x.SurveyItems)
            .Include(x => x.Reactions)
            .Where(x => x.User!.FriendlyName == userName && x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Survey>> GetVotesListAsync(int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _votes.Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.Tags)
            .Include(x => x.SurveyItems)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Survey>> GetLastPagedSurveysAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false)
    {
        var query = _votes.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Include(x => x.SurveyItems)
            .AsNoTracking();

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(Survey.Id), [
            .. GridifyMapings.GetDefaultMappings<Survey>(), new GridifyMap<Survey>
            {
                From = SurveysProfiles.SurveyTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public Task<PagedResultModel<Survey>> GetUserVotesListAsync(int pageNumber,
        int userId,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _votes.Where(x => x.IsDeleted == showDeletedItems && x.UserId == userId)
            .Include(x => x.Tags)
            .Include(x => x.SurveyItems)
            .Include(x => x.User)
            .Include(x => x.Reactions)
            .AsNoTracking();

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Survey>> GetLastVotesByTagAsync(string tag,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _votes.AsNoTracking()
            from t in b.Tags
            where t.Name == tag
            select b;

        query = query.Where(x => x.IsDeleted == showDeletedItems)
            .Include(x => x.User)
            .Include(x => x.SurveyItems)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<Survey?> GetSurveyAsync(int id, bool showDeletedItems = false)
        => _votes.Include(x => x.Tags)
            .Include(x => x.User)
            .Include(blogPost => blogPost.Reactions)
            .Include(blogPost => blogPost.SurveyItems)
            .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task MarkAsDeletedAsync(Survey? surveyItem)
    {
        if (surveyItem is null)
        {
            return;
        }

        surveyItem.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(
            surveyItem.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash);
    }

    public async Task NotifyDeleteChangesAsync(Survey? surveyItem, User? currentUserUser)
    {
        if (surveyItem is null)
        {
            return;
        }

        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(currentUserUser?.UserId ?? 0);

        await emailsFactoryService.SendTextToAllAdminsAsync(
            Invariant($"حذف نظرسنجی {surveyItem.Title} توسط مدیر از سایت "));
    }

    public async Task ApplyVoteAsync(int surveyId, IList<int>? surveyItemIds, User? user)
    {
        if (surveyItemIds is null || surveyItemIds.Count == 0)
        {
            return;
        }

        var vote = await FindVoteAsync(surveyId);

        if (vote is null)
        {
            return;
        }

        if (vote.DueDate.HasValue && vote.DueDate.Value <= DateTime.UtcNow)
        {
            return;
        }

        if (user is null || !user.IsActive)
        {
            return;
        }

        if (!await CanUserVoteAsync(vote, user))
        {
            return;
        }

        vote.TotalRaters += surveyItemIds.Count;

        var voteItems = await voteItemsService.FindVoteItemsAsync(surveyItemIds);

        foreach (var currentVoteItem in voteItems)
        {
            if (currentVoteItem.Users.Any(x => x.Id == user.Id))
            {
                return;
            }

            currentVoteItem.Users.Add(user);
            currentVoteItem.TotalSurveys++;

            await uow.SaveChangesAsync();
        }
    }

    public async Task UpdateSurveyAsync(Survey? surveyItem, VoteModel writeSurveyModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeSurveyModel);

        if (surveyItem is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveVoteTagsAsync(writeSurveyModel.Tags);

        mapper.Map(writeSurveyModel, surveyItem);
        surveyItem.Tags = listOfActualTags;
        await uow.SaveChangesAsync();

        await voteItemsService.AddOrUpdateVoteItemsAsync(surveyItem, writeSurveyModel);

        fullTextSearchService.AddOrUpdateLuceneDocument(surveyItem.MapToWhatsNewItemModel(siteRootUri: ""));
    }

    public async Task<Survey?> AddNewsSurveyAsync(VoteModel writeSurveyModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeSurveyModel);

        var result = await AddNewSurveyAndTagsAsync(writeSurveyModel, user);
        await voteItemsService.AddNewSurveyItemsAsync(writeSurveyModel, result);

        return result;
    }

    public async Task NotifyAddOrUpdateChangesAsync(Survey? surveyItem, VoteModel writeSurveyModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeSurveyModel);

        if (surveyItem is null || user is null)
        {
            return;
        }

        await votesEmailsService.VoteSendEmailAsync(surveyItem);
        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(surveyItem.UserId ?? 0);
        await statService.RecalculateTagsInUseCountsAsync<SurveyTag, Survey>();
    }

    public async Task IndexSurveysAsync()
    {
        var items = await _votes.Where(x => !x.IsDeleted)
            .Include(x => x.Tags)
            .Include(x => x.SurveyItems)
            .Include(x => x.User)
            .AsNoTracking()
            .ToListAsync();

        await fullTextSearchService.IndexTableAsync(items.Select(item => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }

    private async Task<Survey> AddNewSurveyAndTagsAsync(VoteModel writeSurveyModel, User? user)
    {
        var listOfActualTags = await tagsService.SaveVoteTagsAsync(writeSurveyModel.Tags);

        var survey = mapper.Map<VoteModel, Survey>(writeSurveyModel);
        survey.Tags = listOfActualTags;
        survey.UserId = user?.Id;
        var result = AddVote(survey);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        return result;
    }
}
