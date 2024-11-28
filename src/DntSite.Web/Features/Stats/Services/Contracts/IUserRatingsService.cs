using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface IUserRatingsService : IScopedService
{
    public Task<List<TReactionEntity>> GetUserRatingsAsync<TReactionEntity, TForeignKeyEntity>(int fkId,
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity;

    public Task<ShowReactionsUsersListModel> GetReactionsUsersListAsync<TReactionEntity, TForeignKeyEntity>(int postId,
        ApplicationState? applicationState)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity;

    public Task<long?> TotalNumberOfRatingsValueAsync(DateTime fromDate, DateTime toDate, int forUserId);

    public Task<List<SectionFavUserRating>> GetSectionFavItemsRatingsAsync<TReactionEntity, TForeignKeyEntity>(
        DateTime date,
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity;

    public Task<List<SectionFavUserRating>> GetBlogPostsFavItemsRatingsAsync(DateTime? date, int count = 100);

    public Task<List<SectionFavUserRating>> GetSectionAllFavItemsRatingsAsync<TReactionEntity, TForeignKeyEntity>(
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity;

    public Task<List<SectionFavUserRating>> GetSectionFavItemsRatingsFromToAsync<TReactionEntity, TForeignKeyEntity>(
        DateTime fromDate,
        DateTime toDate,
        int count = 100)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity;

    public Task<List<SectionFavUserRating>> GetNewsFavItemsRatingsAsync(DateTime? date, int count = 100);

    public Task<List<SectionFavUserRating>> GetAllNewsFavItemsRatingsAsync(int count = 100);

    public Task<List<SectionFavUserRating>> GetNewsFavItemsRatingsFromToAsync(DateTime fromDate,
        DateTime toDate,
        int count = 100);

    public Task<List<DateUserRatings>> GetTopUserRatingsAsync(DateTime date, int count = 100);

    public Task<List<DateUserRatings>> GetTopUserRatingsFromDateToDateAsync(DateTime fromDate,
        DateTime toDate,
        int count = 100);

    public Task<bool> SaveRatingAsync<TReactionEntity, TForeignKeyEntity>(int fkId,
        ReactionType reactionType,
        int? fromUserId)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>, new()
        where TForeignKeyEntity : BaseAuditedInteractiveEntity;

    public ShowReactionsModel GetReactionsInfo<TReactionEntity, TForeignKeyEntity>(ApplicationState? applicationState,
        ICollection<TReactionEntity> reactions)
        where TReactionEntity : BaseReactionEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedEntity;
}
