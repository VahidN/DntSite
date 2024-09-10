using AutoMapper;
using DntSite.Web.Features.Common.Models;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Models;
using DntSite.Web.Features.StackExchangeQuestions.ModelsMappings;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.StackExchangeQuestions.Services;

public class QuestionsService(
    IUnitOfWork uow,
    ITagsService tagsService,
    IStatService statService,
    IQuestionsEmailsService questionsEmailsService,
    IMapper mapper,
    IFullTextSearchService fullTextSearchService) : IQuestionsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<StackExchangeQuestion, object?>>> CustomOrders =
        new()
        {
            [PagerSortBy.Date] = x => x.Id,
            [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
            [PagerSortBy.Title] = x => x.Title,
            [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
            [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
            [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
        };

    private readonly DbSet<StackExchangeQuestion> _stackExchangeQuestions = uow.DbSet<StackExchangeQuestion>();

    public ValueTask<StackExchangeQuestion?> FindStackExchangeQuestionAsync(int id)
        => _stackExchangeQuestions.FindAsync(id);

    public Task<List<StackExchangeQuestion>> GetAllPublicStackExchangeQuestionsOfDateAsync(DateTime date)
        => _stackExchangeQuestions.AsNoTracking()
            .Include(x => x.User)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt.Year == date.Year && x.Audit.CreatedAt.Month == date.Month &&
                        x.Audit.CreatedAt.Day == date.Day)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public async Task<StackExchangeQuestionsListModel> GetCountsAsync()
        => new()
        {
            AllItemsCount = await _stackExchangeQuestions.AsNoTracking().CountAsync(x => !x.IsDeleted),
            DoneItemsCount = await _stackExchangeQuestions.AsNoTracking().CountAsync(x => x.IsAnswered && !x.IsDeleted),
            NewItemsCount = await _stackExchangeQuestions.CountAsync(x => !x.IsAnswered && !x.IsDeleted)
        };

    public Task<StackExchangeQuestion?> GetLastActiveStackExchangeQuestionAsync()
        => _stackExchangeQuestions.AsNoTracking()
            .Include(question => question.User)
            .OrderBy(question => Guid.NewGuid())
            .FirstOrDefaultAsync(question => !question.IsAnswered && !question.IsDeleted);

    public async Task MarkAsAnsweredAsync(int id)
    {
        var question = await FindStackExchangeQuestionAsync(id);

        if (question is null)
        {
            return;
        }

        question.IsAnswered = true;
        await uow.SaveChangesAsync();
    }

    public async Task<QuestionDetailsModel> GetQuestionDetailsAsync(int id, bool showDeletedItems = false)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentItem =
                await _stackExchangeQuestions.Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
                    .Include(x => x.User)
                    .Include(blogPost => blogPost.Reactions)
                    .Include(x => x.Tags)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem = await _stackExchangeQuestions.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id > id)
                .OrderBy(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(),
            PreviousItem = await _stackExchangeQuestions.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id < id)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(blogPost => blogPost.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync()
        };

    public Task<PagedResultModel<StackExchangeQuestion>> GetStackExchangeQuestionsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false)
    {
        var query = _stackExchangeQuestions.AsNoTracking()
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Where(x => x.IsDeleted == showDeletedItems);

        if (userId is not null)
        {
            query = query.Where(x => x.UserId == userId.Value);
        }

        if (isNewItems)
        {
            query = query.Where(x => !x.IsAnswered);
        }

        if (isDone)
        {
            query = query.Where(x => x.IsAnswered);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<StackExchangeQuestion>> GetLastPagedStackExchangeQuestionsAsync(
        DntQueryBuilderModel state,
        bool showDeletedItems = false)
    {
        var query = _stackExchangeQuestions.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(StackExchangeQuestion.Id), [
            .. GridifyMapings.GetDefaultMappings<StackExchangeQuestion>(), new GridifyMap<StackExchangeQuestion>
            {
                From = QuestionMappingsProfiles.StackExchangeQuestionTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public Task<PagedResultModel<StackExchangeQuestion>> GetStackExchangeQuestionsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _stackExchangeQuestions.AsNoTracking()
            from t in b.Tags
            where t.Name == tagName
            select b;

        query = query.Include(x => x.User)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<StackExchangeQuestion>> GetLastPagedStackExchangeQuestionByUsernameAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _stackExchangeQuestions.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Tags)
            .Include(x => x.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task UpdateStatAsync(StackExchangeQuestion? currentItem, bool isFromFeed)
    {
        if (currentItem is null)
        {
            return;
        }

        currentItem.EntityStat.NumberOfViews++;

        if (isFromFeed)
        {
            currentItem.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public Task<StackExchangeQuestion?> GetStackExchangeQuestionAsync(int id, bool showDeletedItems = false)
        => _stackExchangeQuestions.Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task MarkAsDeletedAsync(StackExchangeQuestion? question)
    {
        if (question is null)
        {
            return;
        }

        question.IsDeleted = true;
        await uow.SaveChangesAsync();

        fullTextSearchService.DeleteLuceneDocument(question.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash);
    }

    public async Task NotifyDeleteChangesAsync(StackExchangeQuestion? question, User? currentUserUser)
    {
        if (question is null)
        {
            return;
        }

        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(question.UserId ?? 0);

        await questionsEmailsService.StackExchangeQuestionSendEmailAsync(new StackExchangeQuestion
        {
            Id = question.Id,
            Title = question.Title,
            Description = Invariant($"حذف پرسش شماره {question.Id} توسط مدیر از سایت ")
        }, currentUserUser?.FriendlyName ?? SharedConstants.GuestUserName);
    }

    public async Task UpdateQuestionsItemAsync(StackExchangeQuestion? question,
        QuestionModel writeQuestionModel,
        User? user)
    {
        ArgumentNullException.ThrowIfNull(writeQuestionModel);

        if (question is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveNewQuestionTagsAsync(writeQuestionModel.Tags);

        mapper.Map(writeQuestionModel, question);
        question.Tags = listOfActualTags;

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(question.MapToWhatsNewItemModel(siteRootUri: ""));
    }

    public async Task<StackExchangeQuestion?> AddStackExchangeQuestionAsync(QuestionModel writeQuestionModel,
        User? user)
    {
        ArgumentNullException.ThrowIfNull(writeQuestionModel);

        var listOfActualTags = await tagsService.SaveNewQuestionTagsAsync(writeQuestionModel.Tags);

        var question = mapper.Map<QuestionModel, StackExchangeQuestion>(writeQuestionModel);
        question.Tags = listOfActualTags;
        question.UserId = user?.Id;
        var result = AddStackExchangeQuestion(question);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: ""));

        return result;
    }

    public StackExchangeQuestion AddStackExchangeQuestion(StackExchangeQuestion data)
        => _stackExchangeQuestions.Add(data).Entity;

    public async Task NotifyAddOrUpdateChangesAsync(StackExchangeQuestion? question,
        QuestionModel writeQuestionModel,
        User? user)
    {
        ArgumentNullException.ThrowIfNull(writeQuestionModel);

        await statService.RecalculateAllQuestionTagsInUseCountsAsync(writeQuestionModel.Tags);

        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(user?.Id ?? 0);

        if (question is not null)
        {
            await questionsEmailsService.StackExchangeQuestionSendEmailAsync(question,
                user?.FriendlyName ?? SharedConstants.GuestUserName);
        }
    }

    public Task IndexStackExchangeQuestionsAsync()
    {
        var items = _stackExchangeQuestions.AsNoTracking()
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Where(x => !x.IsDeleted)
            .AsEnumerable();

        return fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "")));
    }
}
