using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Bookmarks.Services.Contracts;

public interface IBookmarksService : IScopedService
{
    Task<bool> SavePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId, int? fromUserId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>, new()
        where TForeignKeyEntity : BaseAuditedInteractiveEntity;

    Task<List<TBookmarkEntity>> GetPostBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId, int count = 100)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity;

    Task<List<User?>> GetPostBookmarksUsersListAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity;

    Task<bool> DeletePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(int bookmarkId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity;

    Task<PagedResultModel<TBookmarkEntity>> GetUserBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(int userId,
        int pageNumber,
        int recordsPerPage = 8,
        bool isAscending = false)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity;
}
