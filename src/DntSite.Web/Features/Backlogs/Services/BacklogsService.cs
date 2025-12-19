using AutoMapper;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Backlogs.ModelsMappings;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.ModelsMappings;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Backlogs.Services;

public class BacklogsService(
    IUnitOfWork uow,
    IMapper mapper,
    ITagsService tagsService,
    IStatService statService,
    IBlogPostsService blogPostsService,
    IEmailsFactoryService emailsFactoryService,
    IBacklogEmailsService emailsService,
    IUserRatingsService userRatingsService,
    IFullTextSearchService fullTextSearchService,
    ILogger<BacklogsService> logger) : IBacklogsService
{
    private static readonly Dictionary<PagerSortBy, Expression<Func<Backlog, object?>>> CustomOrders = new()
    {
        [PagerSortBy.Date] = x => x.Id,
        [PagerSortBy.FriendlyName] = x => x.User!.FriendlyName,
        [PagerSortBy.Title] = x => x.Title,
        [PagerSortBy.RepliesNumbers] = x => x.EntityStat.NumberOfComments,
        [PagerSortBy.ViewsNumber] = x => x.EntityStat.NumberOfViews,
        [PagerSortBy.TotalRating] = x => x.Rating.TotalRating
    };

    private readonly DbSet<Backlog> _backlogs = uow.DbSet<Backlog>();

    public ValueTask<Backlog?> FindBacklogAsync(int id) => _backlogs.FindAsync(id);

    public Task<Backlog?> GetFullBacklogAsync(int id, bool showDeletedItems = false)
        => _backlogs.Include(x => x.User)
            .Include(x => x.DoneByUser)
            .Include(x => x.Reactions)
            .Include(x => x.Tags)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.IsDeleted == showDeletedItems && x.Id == id);

    public Backlog AddBacklog(Backlog data) => _backlogs.Add(data).Entity;

    public Task<PagedResultModel<Backlog>> GetBacklogsAsync(int pageNumber,
        int? userId = null,
        int recordsPerPage = 15,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false,
        bool isNewItems = false,
        bool isDone = false,
        bool isInProgress = false)
    {
        var query = _backlogs.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.DoneByUser)
            .Include(x => x.Reactions)
            .Include(x => x.Tags)
            .Where(x => x.IsDeleted == showDeletedItems);

        if (userId is not null)
        {
            query = query.Where(x => x.UserId == userId.Value);
        }

        if (isNewItems)
        {
            query = query.Where(x => !x.DoneByUserId.HasValue && !x.ConvertedBlogPostId.HasValue);
        }

        if (isDone)
        {
            query = query.Where(x => x.ConvertedBlogPostId.HasValue);
        }

        if (isInProgress)
        {
            query = query.Where(x => x.DoneByUserId.HasValue && !x.ConvertedBlogPostId.HasValue);
        }

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task<BacklogsListModel> GetCountsAsync()
        => new()
        {
            AllItemsCount = await _backlogs.AsNoTracking().CountAsync(x => !x.IsDeleted),
            DoneItemsCount =
                await _backlogs.AsNoTracking().CountAsync(x => x.ConvertedBlogPostId.HasValue && !x.IsDeleted),
            InProgressItemsCount =
                await _backlogs.AsNoTracking()
                    .CountAsync(x => x.DoneByUserId.HasValue && !x.ConvertedBlogPostId.HasValue && !x.IsDeleted),
            NewItemsCount = await _backlogs.AsNoTracking().CountAsync(x => !x.DoneByUserId.HasValue && !x.IsDeleted)
        };

    public Task<bool> SaveRatingAsync(int fkId, ReactionType reactionType, int? fromUserId)
        => userRatingsService.SaveRatingAsync<BacklogReaction, Backlog>(fkId, reactionType, fromUserId);

    public async Task<BacklogDetailsModel> BacklogDetailsAsync(int id, bool showDeletedItems = false)

        // این شماره‌ها پشت سر هم نیستند
        => new()
        {
            CurrentItem =
                await _backlogs.AsNoTracking()
                    .Where(x => x.IsDeleted == showDeletedItems && x.Id == id)
                    .OrderBy(x => x.Id)
                    .Include(x => x.User)
                    .Include(x => x.DoneByUser)
                    .Include(x => x.Reactions)
                    .Include(x => x.Tags)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(),
            NextItem = await _backlogs.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id > id)
                .OrderBy(x => x.Id)
                .Include(x => x.User)
                .Include(x => x.DoneByUser)
                .Include(x => x.Reactions)
                .Include(x => x.Tags)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(),
            PreviousItem = await _backlogs.AsNoTracking()
                .Where(x => x.IsDeleted == showDeletedItems && x.Id < id)
                .OrderByDescending(x => x.Id)
                .Include(x => x.User)
                .Include(x => x.DoneByUser)
                .Include(x => x.Reactions)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync()
        };

    public Task<bool> HasUserAnotherHalfFinishedAssignedBacklogAsync(int userId)
        => _backlogs.AsNoTracking().AnyAsync(x => x.DoneByUserId == userId && !x.DoneDate.HasValue);

    public async Task CancelOldOnesAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var inprogressItems = await _backlogs.Include(x => x.DoneByUser)
            .Where(x => x.DoneByUserId.HasValue && !x.ConvertedBlogPostId.HasValue && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var backlog in inprogressItems)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (backlog.StartDate is null || backlog.DaysEstimate is null)
            {
                continue;
            }

            if ((now - backlog.StartDate.Value).Days > backlog.DaysEstimate.Value)
            {
                await LogAndEmailAsync(backlog);

                backlog.DoneByUserId = null;
                backlog.StartDate = null;
                backlog.DoneDate = null;
                backlog.DaysEstimate = null;
                backlog.ConvertedBlogPostId = null;

                await uow.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public Task<List<Backlog>> GetAllPublicBacklogsOfDateAsync(DateTime date)
        => _backlogs.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.DoneByUser)
            .Include(x => x.Reactions)
            .Include(x => x.Tags)
            .Where(x => !x.IsDeleted && x.Audit.CreatedAt.Year == date.Year && x.Audit.CreatedAt.Month == date.Month &&
                        x.Audit.CreatedAt.Day == date.Day)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public Task<Backlog?> GetLastActiveBacklogAsync()
        => _backlogs.AsNoTracking()
            .Include(backlog => backlog.User)
            .Include(x => x.DoneByUser)
            .Include(x => x.Reactions)
            .Include(x => x.Tags)
            .OrderBy(_ => Guid.NewGuid())
            .FirstOrDefaultAsync(backlog
                => !backlog.IsDeleted && !backlog.DoneByUserId.HasValue && !backlog.ConvertedBlogPostId.HasValue);

    public Task<PagedResultModel<Backlog>> GetBacklogsByTagNameAsync(string tagName,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = from b in _backlogs.AsNoTracking() from t in b.Tags where t.Name == tagName select b;

        query = query.Include(x => x.User)
            .Include(x => x.DoneByUser)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public Task<PagedResultModel<Backlog>> GetLastPagedBacklogsAsync(DntQueryBuilderModel state,
        bool showDeletedItems = false)
    {
        var query = _backlogs.Where(blogPost => blogPost.IsDeleted == showDeletedItems)
            .Include(blogPost => blogPost.User)
            .Include(x => x.DoneByUser)
            .Include(blogPost => blogPost.Tags)
            .Include(blogPost => blogPost.Reactions)
            .AsNoTracking();

        return query.ApplyQueryableDntGridFilterAsync(state, nameof(Backlog.Id), [
            .. GridifyMapings.GetDefaultMappings<Backlog>(), new GridifyMap<Backlog>
            {
                From = BacklogsMappingsProfiles.BacklogTags,
                To = entity => entity.Tags.Select(tag => tag.Name)
            }
        ]);
    }

    public Task<PagedResultModel<Backlog>> GetLastPagedBacklogsAsync(string name,
        int pageNumber,
        int recordsPerPage = 8,
        bool showDeletedItems = false,
        PagerSortBy pagerSortBy = PagerSortBy.Date,
        bool isAscending = false)
    {
        var query = _backlogs.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.DoneByUser)
            .Include(x => x.Tags)
            .Include(x => x.Reactions)
            .Where(x => x.IsDeleted == showDeletedItems && x.User!.FriendlyName == name);

        return query.ApplyQueryablePagingAsync(pageNumber, recordsPerPage, pagerSortBy, isAscending, CustomOrders);
    }

    public async Task MarkAsDeletedAsync(Backlog? backlog)
    {
        if (backlog is null)
        {
            return;
        }

        backlog.IsDeleted = true;
        await uow.SaveChangesAsync();

        logger.LogWarning(message: "Deleted a Backlog record with Id={Id} and Title={Text}", backlog.Id, backlog.Title);

        fullTextSearchService.DeleteLuceneDocument(backlog
            .MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)
            .DocumentTypeIdHash);
    }

    public async Task NotifyDeleteChangesAsync(Backlog? backlog, BacklogModel? writeBacklogModel)
    {
        if (backlog is null || writeBacklogModel is null)
        {
            return;
        }

        await UpdateStatAsync(backlog);

        await emailsFactoryService.SendTextToAllAdminsAsync(string.Create(CultureInfo.InvariantCulture,
            $"پیشنهاد شماره {backlog.Id} حذف شد."));
    }

    public async Task UpdateBacklogAsync(Backlog? backlog, BacklogModel? writeBacklogModel)
    {
        ArgumentNullException.ThrowIfNull(writeBacklogModel);

        if (backlog is null)
        {
            return;
        }

        var listOfActualTags = await tagsService.SaveNewBacklogTagsAsync(writeBacklogModel.Tags);

        mapper.Map(writeBacklogModel, backlog);
        backlog.Tags = listOfActualTags;

        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(
            backlog.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false));
    }

    public async Task<Backlog?> AddBacklogAsync(BacklogModel? writeBacklogModel, User? user)
    {
        ArgumentNullException.ThrowIfNull(writeBacklogModel);

        var listOfActualTags = await tagsService.SaveNewBacklogTagsAsync(writeBacklogModel.Tags);

        var item = mapper.Map<BacklogModel, Backlog>(writeBacklogModel);
        item.Tags = listOfActualTags;
        item.UserId = user?.Id;
        item.IsDeleted = false;
        var result = AddBacklog(item);
        await uow.SaveChangesAsync();

        fullTextSearchService.AddOrUpdateLuceneDocument(result.MapToWhatsNewItemModel(siteRootUri: "",
            showBriefDescription: false));

        return result;
    }

    public async Task NotifyAddOrUpdateChangesAsync(Backlog? backlog, BacklogModel? writeBacklogModel)
    {
        if (backlog is null || writeBacklogModel is null)
        {
            return;
        }

        await emailsService.NewBacklogSendEmailToAdminsAsync(backlog);
        await UpdateStatAsync(backlog);
    }

    public async Task UpdateStatAsync(int backlogId, bool isFromFeed)
    {
        var item = await FindBacklogAsync(backlogId);

        if (item is null)
        {
            return;
        }

        item.EntityStat.NumberOfViews++;

        if (isFromFeed)
        {
            item.EntityStat.NumberOfViewsFromFeed++;
        }

        await uow.SaveChangesAsync();
    }

    public async Task<OperationResult> TakeBacklogAsync(ManageBacklogModel? data,
        CurrentUserModel? user,
        string? siteRootUri)
    {
        if (data is null || user?.UserId is null)
        {
            return ("اطلاعات وارد شده معتبر نیستند", OperationStat.Failed);
        }

        if (data.DaysEstimate is not > 0)
        {
            return ("لطفا تخمین تکمیل (تعداد روز) متناظر با این پیشنهاد را وارد کنید", OperationStat.Failed);
        }

        var backlog = await FindBacklogAsync(data.Id);

        if (backlog is null)
        {
            return ("پیشنهاد درخواستی یافت نشد", OperationStat.Failed);
        }

        if (backlog.IsDeleted)
        {
            return ("پیشنهاد درخواستی حذف شده‌است", OperationStat.Failed);
        }

        if (backlog.DoneByUserId is not null)
        {
            return ("پیشنهاد درخواستی پیشتر رزرو شده‌است", OperationStat.Failed);
        }

        if (await HasUserAnotherHalfFinishedAssignedBacklogAsync(user.UserId.Value))
        {
            return ("پیش از انتخاب پیشنهاد دیگری نیاز است مورد قبلی انتخابی را تکمیل کنید", OperationStat.Failed);
        }

        backlog.DoneByUserId = user.UserId.Value;
        backlog.StartDate = DateTime.UtcNow;
        backlog.DoneDate = null;
        backlog.DaysEstimate = data.DaysEstimate;
        backlog.ConvertedBlogPostId = null;

        await uow.SaveChangesAsync();

        var url = string.Create(CultureInfo.InvariantCulture, $"{siteRootUri}backlogs/details/{backlog.Id}");
        await emailsFactoryService.SendTextToAllAdminsAsync($"انتخاب پیشنهاد <a href='{url}'>{backlog.Title}</a>");

        return "با موفقیت انجام شد";
    }

    public async Task<OperationResult> CancelBacklogAsync(ManageBacklogModel? data,
        CurrentUserModel? user,
        string? siteRootUri)
    {
        if (data is null || user is null)
        {
            return ("اطلاعات وارد شده معتبر نیستند", OperationStat.Failed);
        }

        var backlog = await FindBacklogAsync(data.Id);

        if (backlog is null)
        {
            return ("پیشنهاد دریافتی یافت نشد", OperationStat.Failed);
        }

        if (backlog.IsDeleted)
        {
            return ("پیشنهاد دریافتی حذف شده‌است", OperationStat.Failed);
        }

        if (backlog.DoneByUserId is null)
        {
            return ("پیشنهاد دریافتی هنوز انتخاب نشده‌است", OperationStat.Failed);
        }

        if (!user.IsAdmin && backlog.DoneByUserId != user.UserId)
        {
            return ("پیشنهاد دریافتی متعلق به شما نیست", OperationStat.Failed);
        }

        backlog.DoneByUserId = null;
        backlog.StartDate = null;
        backlog.DoneDate = null;
        backlog.DaysEstimate = null;
        backlog.ConvertedBlogPostId = null;

        await uow.SaveChangesAsync();

        var url = string.Create(CultureInfo.InvariantCulture, $"{siteRootUri}backlogs/details/{backlog.Id}");
        await emailsFactoryService.SendTextToAllAdminsAsync($"لغو پیشنهاد <a href='{url}'>{backlog.Title}</a>");

        return "با موفقیت انجام شد";
    }

    public async Task<OperationResult> DoneBacklogAsync(ManageBacklogModel? data,
        CurrentUserModel? user,
        string? siteRootUri)
    {
        if (data is null || user is null)
        {
            return ("اطلاعات وارد شده معتبر نیستند", OperationStat.Failed);
        }

        var backlog = await FindBacklogAsync(data.Id);

        if (backlog is null)
        {
            return ("پیشنهاد دریافتی یافت نشد", OperationStat.Failed);
        }

        if (backlog.IsDeleted)
        {
            return ("پیشنهاد دریافتی حذف شده‌است", OperationStat.Failed);
        }

        if (backlog.DoneByUserId is null)
        {
            return ("پیشنهاد دریافتی هنوز توسط شخصی انتخاب نشده‌است", OperationStat.Failed);
        }

        if (!user.IsAdmin && backlog.DoneByUserId != user.UserId)
        {
            return ("پیشنهاد دریافتی توسط شما انتخاب نشده‌است", OperationStat.Failed);
        }

        if (data.ConvertedBlogPostId is not > 0)
        {
            return ("لطفا شماره مقاله متناظر با این پیشنهاد را وارد کنید", OperationStat.Failed);
        }

        var post = await blogPostsService.FindBlogPostAsync(data.ConvertedBlogPostId.Value);

        if (post?.UserId is null || post.UserId.Value != backlog.DoneByUserId)
        {
            return ("شماره مقاله متناظر وارد شده، یافت نشد یا نویسنده آن شما نیستید", OperationStat.Failed);
        }

        backlog.DoneDate ??= DateTime.UtcNow;
        backlog.ConvertedBlogPostId = data.ConvertedBlogPostId.Value;

        await uow.SaveChangesAsync();

        var url = string.Create(CultureInfo.InvariantCulture, $"{siteRootUri}backlogs/details/{backlog.Id}");
        await emailsFactoryService.SendTextToAllAdminsAsync($"پایان پیشنهاد <a href='{url}'>{backlog.Title}</a>");

        return "با موفقیت انجام شد";
    }

    public async Task IndexBackLogsAsync()
    {
        var items = await _backlogs.AsNoTracking()
            .Include(x => x.Tags)
            .Include(x => x.User)
            .Include(x => x.DoneByUser)
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        await fullTextSearchService.IndexTableAsync(items.Select(item
            => item.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)));
    }

    private async Task UpdateStatAsync(Backlog backlog)
    {
        await statService.RecalculateThisUserNumberOfPostsAndCommentsAndLinksAsync(backlog.UserId ?? 0);
        await statService.RecalculateTagsInUseCountsAsync<BacklogTag, Backlog>();
    }

    private Task LogAndEmailAsync(Backlog backlog)
    {
        var message = $"لغو اشتراک پیشنهاد {backlog.Title}, {backlog.DoneByUser?.FriendlyName}";

        return emailsFactoryService.SendEmailToAllAdminsNormalAsync(messageId: "CancelBackLog",
            inReplyTo: "CancelBackLog", references: "CancelBackLog", message, emailSubject: "لغو اشتراک پیشنهاد");
    }
}
