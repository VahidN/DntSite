using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class UserRatingsService(IUnitOfWork uow) : IUserRatingsService
{
    private readonly DbSet<ParentReactionEntity> _reactions = uow.DbSet<ParentReactionEntity>();

    public Task<long?> TotalNumberOfRatingsValueAsync(DateTime fromDate, DateTime toDate, int forUserId)
        => _reactions.Where(x => x.ForUserId == forUserId &&
                                 x.Audit.CreatedAt >= fromDate && x.Audit.CreatedAt <= toDate)
            .SumAsync(x => (long?)x.Reaction);

    public async Task<bool> SaveRatingAsync<TReactionEntity, TForeignKeyEntity>(int fkId,
        ReactionType reactionType,
        int? fromUserId)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>, new()
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        if (fromUserId is null)
        {
            return false;
        }

        if (reactionType == ReactionType.ShowList)
        {
            return false;
        }

        var parentEntity = await uow.DbSet<TForeignKeyEntity>().FindAsync(fkId);

        if (parentEntity is null)
        {
            return false;
        }

        if (parentEntity.UserId is not null && fromUserId.Value == parentEntity.UserId.Value) // یک شخص به خودش رای ندهد
        {
            return false;
        }

        var entityReactionsQuery = _reactions.OfType<TReactionEntity>();

        var userReaction = await entityReactionsQuery.OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.ParentId == fkId && x.UserId == fromUserId);

        if (userReaction is not null)
        {
            if (userReaction.Reaction == reactionType)
            {
                return false;
            }

            _reactions.Remove(userReaction);
        }

        if (reactionType != ReactionType.Cancel)
        {
            _reactions.Add(new TReactionEntity
            {
                ForUserId = parentEntity.UserId,
                UserId = fromUserId.Value,
                ParentId = fkId,
                Reaction = reactionType
            });
        }

        await uow.SaveChangesAsync();

        parentEntity.Rating.TotalRaters = await entityReactionsQuery.CountAsync(x => x.ParentId == fkId);

        parentEntity.Rating.TotalRating = await entityReactionsQuery
            .Where(x => x.ParentId == fkId)
            .SumAsync(x => (int?)x.Reaction) ?? 0;

        parentEntity.Rating.AverageRating = (decimal)parentEntity.Rating.TotalRating / parentEntity.Rating.TotalRaters;
        await uow.SaveChangesAsync();

        return true;
    }

    public async Task<List<SectionFavUserRating>> GetBlogPostsFavItemsRatingsAsync(DateTime? date, int count = 100)
    {
        IList<SectionFavUserRating> favItems = date.HasValue
            ? await GetSectionFavItemsRatingsAsync<BlogPostReaction, BlogPost>(date.Value, count)
            : await GetSectionAllFavItemsRatingsAsync<BlogPostReaction, BlogPost>(count);

        return await GetBlogPostsFavItemsRatingsAsync(date, favItems);
    }

    public async Task<List<SectionFavUserRating>> GetNewsFavItemsRatingsAsync(DateTime? date, int count = 100)
    {
        IList<SectionFavUserRating> favItems = date.HasValue
            ? await GetSectionFavItemsRatingsAsync<DailyNewsItemReaction, DailyNewsItem>(date.Value, count)
            : await GetSectionAllFavItemsRatingsAsync<DailyNewsItemReaction, DailyNewsItem>(count);

        return await GetNewsFavItemsRatingsAsync(date, favItems);
    }

    public Task<List<SectionFavUserRating>> GetAllNewsFavItemsRatingsAsync(int count = 100)
        => GetNewsFavItemsRatingsAsync(date: null, count);

    public async Task<List<SectionFavUserRating>> GetNewsFavItemsRatingsFromToAsync(DateTime fromDate,
        DateTime toDate,
        int count = 100)
    {
        var favItems =
            await GetSectionFavItemsRatingsFromToAsync<DailyNewsItemReaction, DailyNewsItem>(fromDate, toDate, count);

        return await GetNewsFavItemsRatingsAsync(date: null, favItems);
    }

    public Task<List<DateUserRatings>> GetTopUserRatingsAsync(DateTime date, int count = 100)
        => _reactions.AsNoTracking()
            .Where(userRating => userRating.Audit.CreatedAt.Year == date.Year &&
                                 userRating.Audit.CreatedAt.Month == date.Month &&
                                 userRating.Audit.CreatedAt.Day == date.Day && userRating.ForUser != null &&
                                 userRating.ForUser.IsActive)
            .GroupBy(userRating => userRating.ForUser)
            .Select(grouping => new DateUserRatings
            {
                ForUser = grouping.Key,
                TotalRatingValue = grouping.Sum(userRating => (int)userRating.Reaction)
            })
            .OrderByDescending(favUserRating => favUserRating.TotalRatingValue)
            .ThenBy(dateUserRatings => dateUserRatings.ForUser!.FriendlyName)
            .Take(count)
            .ToListAsync();

    public Task<List<DateUserRatings>>
        GetTopUserRatingsFromDateToDateAsync(DateTime fromDate, DateTime toDate, int count = 100)
        => _reactions.AsNoTracking()
            .Where(userRating => userRating.Audit.CreatedAt >= fromDate && userRating.Audit.CreatedAt <= toDate &&
                                 userRating.ForUser != null && userRating.ForUser.IsActive)
            .GroupBy(userRating => userRating.ForUser)
            .Select(grouping => new DateUserRatings
            {
                ForUser = grouping.Key,
                TotalRatingValue = grouping.Sum(userRating => (int)userRating.Reaction)
            })
            .OrderByDescending(favUserRating => favUserRating.TotalRatingValue)
            .ThenBy(dateUserRatings => dateUserRatings.ForUser!.FriendlyName)
            .Take(count)
            .ToListAsync();

    public Task<List<TReactionEntity>> GetUserRatingsAsync<TReactionEntity, TForeignKeyEntity>(int fkId,
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity
        => _reactions.AsNoTracking()
            .OfType<TReactionEntity>()
            .Include(x => x.User)
            .Where(x => x.ParentId == fkId)
            .Take(count)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public Task<List<SectionFavUserRating>> GetSectionFavItemsRatingsAsync<TReactionEntity, TForeignKeyEntity>(
        DateTime date,
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity
        => _reactions.AsNoTracking()
            .OfType<TReactionEntity>()
            .Where(userRating => userRating.Audit.CreatedAt.Year == date.Year &&
                                 userRating.Audit.CreatedAt.Month == date.Month &&
                                 userRating.Audit.CreatedAt.Day == date.Day)
            .GroupBy(userRating => userRating.ParentId)
            .Select(r => new SectionFavUserRating
            {
                FkId = r.Key,
                TotalRatingValue = r.Sum(userRating => (int)userRating.Reaction)
            })
            .OrderByDescending(favUserRating => favUserRating.TotalRatingValue)
            .Take(count)
            .ToListAsync();

    public Task<List<SectionFavUserRating>> GetSectionAllFavItemsRatingsAsync<TReactionEntity, TForeignKeyEntity>(
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity
        => _reactions.AsNoTracking()
            .OfType<TReactionEntity>()
            .GroupBy(userRating => userRating.ParentId)
            .Select(r => new SectionFavUserRating
            {
                FkId = r.Key,
                TotalRatingValue = r.Sum(userRating => (int)userRating.Reaction)
            })
            .OrderByDescending(favUserRating => favUserRating.TotalRatingValue)
            .Take(count)
            .ToListAsync();

    public Task<List<SectionFavUserRating>> GetSectionFavItemsRatingsFromToAsync<TReactionEntity, TForeignKeyEntity>(
        DateTime fromDate,
        DateTime toDate,
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity
        => _reactions.AsNoTracking()
            .OfType<TReactionEntity>()
            .Where(userRating => userRating.Audit.CreatedAt <= fromDate && userRating.Audit.CreatedAt >= toDate)
            .GroupBy(userRating => userRating.ParentId)
            .Select(r => new SectionFavUserRating
            {
                FkId = r.Key,
                TotalRatingValue = r.Sum(userRating => (int)userRating.Reaction)
            })
            .OrderByDescending(favUserRating => favUserRating.TotalRatingValue)
            .Take(count)
            .ToListAsync();

    public async Task<ShowReactionsUsersListModel> GetReactionsUsersListAsync<TReactionEntity, TForeignKeyEntity>(
        int postId,
        ApplicationState? applicationState)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        var reactions = await _reactions.AsNoTracking()
            .OfType<TReactionEntity>()
            .Include(x => x.User)
            .Where(x => x.ParentId == postId)
            .ToListAsync();

        var thumbsUpUsers = reactions.Where(x => x.Reaction == ReactionType.ThumbsUp).Select(x => x.User).ToList();
        var thumbsDownUsers = reactions.Where(x => x.Reaction == ReactionType.ThumbsDown).Select(x => x.User).ToList();
        var currentUser = applicationState?.CurrentUser;
        var userId = currentUser?.UserId;

        return new ShowReactionsUsersListModel
        {
            ThumbsUpUsers = thumbsUpUsers,
            ThumbsDownUsers = thumbsDownUsers,
            ReactionsInfo = new ShowReactionsModel
            {
                AreReactionsDisabled = currentUser?.IsAuthenticated == false,
                ThumbsDownUsersCount = thumbsDownUsers.Count,
                ThumbsUpUsersCount = thumbsUpUsers.Count,
                IsCurrentUserReacted = thumbsDownUsers.Any(user => user?.UserId == userId) ||
                                       thumbsUpUsers.Any(user => user?.UserId == userId)
            }
        };
    }

    public ShowReactionsModel GetReactionsInfo<TReactionEntity, TForeignKeyEntity>(ApplicationState? applicationState,
        ICollection<TReactionEntity> reactions)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity
    {
        var currentUser = applicationState?.CurrentUser?.User;
        var isCurrentUserReacted = currentUser is not null && reactions.Any(x => x.UserId == currentUser.Id);

        return new ShowReactionsModel
        {
            AreReactionsDisabled = applicationState?.CurrentUser?.IsAuthenticated == false,
            ThumbsUpUsersCount = reactions.Count(x => x.Reaction == ReactionType.ThumbsUp),
            ThumbsDownUsersCount = reactions.Count(x => x.Reaction == ReactionType.ThumbsDown),
            IsCurrentUserReacted = isCurrentUserReacted
        };
    }

    public async Task<List<SectionFavUserRating>> GetBlogPostsFavItemsRatingsFromToAsync(DateTime fromDate,
        DateTime toDate,
        int count = 100)
    {
        var favItems = await GetSectionFavItemsRatingsFromToAsync<BlogPostReaction, BlogPost>(fromDate, toDate, count);

        return await GetBlogPostsFavItemsRatingsAsync(date: null, favItems);
    }

    public Task<List<SectionFavUserRating>> GetAllBlogPostsFavItemsRatingsAsync(int count = 100)
        => GetBlogPostsFavItemsRatingsAsync(date: null, count);

    private async Task<List<SectionFavUserRating>> GetBlogPostsFavItemsRatingsAsync(DateTime? date,
        IList<SectionFavUserRating> favItems)
    {
        var favItemsIds = favItems.Select(favUserRating => favUserRating.FkId).ToList();

        var relatedPosts = await uow.DbSet<BlogPost>()
            .AsNoTracking()
            .Include(blogPost => blogPost.User)
            .Where(blogPost => favItemsIds.Contains(blogPost.Id) && !blogPost.IsDeleted)
            .ToListAsync();

        return relatedPosts.Select(blogPost => new SectionFavUserRating
            {
                FkId = blogPost.Id,
                Title = blogPost.Title,
                TotalRatingValue = favItems.First(favUserRating => favUserRating.FkId == blogPost.Id).TotalRatingValue,
                DateTime = date,
                ForUser = blogPost.User
            })
            .OrderByDescending(favUserRating => favUserRating.TotalRatingValue)
            .ToList();
    }

    private async Task<List<SectionFavUserRating>> GetNewsFavItemsRatingsAsync(DateTime? date,
        IList<SectionFavUserRating> favItems)
    {
        var favItemsIds = favItems.Select(favUserRating => favUserRating.FkId).ToList();

        var relatedPosts = await uow.DbSet<DailyNewsItem>()
            .AsNoTracking()
            .Include(newsItem => newsItem.User)
            .Where(newsItem => favItemsIds.Contains(newsItem.Id) && !newsItem.IsDeleted)
            .ToListAsync();

        return relatedPosts.Select(newsItem => new SectionFavUserRating
            {
                FkId = newsItem.Id,
                Title = newsItem.Title,
                TotalRatingValue = favItems.First(favUserRating => favUserRating.FkId == newsItem.Id).TotalRatingValue,
                DateTime = date,
                ForUser = newsItem.User
            })
            .OrderByDescending(favUserRating => favUserRating.TotalRatingValue)
            .ToList();
    }
}
