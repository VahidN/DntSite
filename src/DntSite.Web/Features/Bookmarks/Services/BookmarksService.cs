using DntSite.Web.Features.Bookmarks.Models;
using DntSite.Web.Features.Bookmarks.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using Gridify;

namespace DntSite.Web.Features.Bookmarks.Services;

public class BookmarksService(IUnitOfWork uow) : IBookmarksService
{
    private readonly DbSet<ParentBookmarkEntity> _bookmarks = uow.DbSet<ParentBookmarkEntity>();

    public Task<List<TBookmarkEntity>> GetPostBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId,
        int count = 100)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
        => _bookmarks.AsNoTracking()
            .OfType<TBookmarkEntity>()
            .Include(x => x.User)
            .Where(x => x.ParentId == fkId)
            .Take(count)
            .OrderBy(x => x.Id)
            .ToListAsync();

    public Task<List<User?>> GetPostBookmarksUsersListAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
        => _bookmarks.AsNoTracking()
            .OfType<TBookmarkEntity>()
            .Include(x => x.User)
            .Where(x => x.ParentId == fkId)
            .Select(x => x.User)
            .ToListAsync();

    public async Task<PagedResultModel<TBookmarkEntity>> GetUserBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(
        int? userId,
        int pageNumber,
        int recordsPerPage = 8,
        bool isAscending = false)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        var query = _bookmarks.OfType<TBookmarkEntity>()
            .Include(bookmarkEntity => bookmarkEntity.Parent)
            .ThenInclude(keyEntity => keyEntity.User)
            .Include(bookmarkEntity => bookmarkEntity.User)
            .Where(bookmarkEntity => bookmarkEntity.UserId == userId && !bookmarkEntity.Parent.IsDeleted &&
                                     !bookmarkEntity.IsDeleted)
            .AsNoTracking();

        query = !isAscending
            ? query.OrderByDescending(bookmarkEntity => bookmarkEntity.Id)
            : query.OrderBy(bookmarkEntity => bookmarkEntity.Id);

        return new PagedResultModel<TBookmarkEntity>
        {
            TotalItems = await query.CountAsync(),
            Data = await query.ApplyPaging(pageNumber, recordsPerPage).ToListAsync()
        };
    }

    public async Task<bool> SavePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId,
        BookmarkActionType actionType,
        int? fromUserId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>, new()
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        if (fromUserId is null)
        {
            return false;
        }

        var parentEntity = await uow.DbSet<TForeignKeyEntity>().FindAsync(fkId);

        if (parentEntity is null)
        {
            return false;
        }

        var entityBookmarksQuery = _bookmarks.OfType<TBookmarkEntity>();

        var userBookmark = await entityBookmarksQuery.OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.ParentId == fkId && x.UserId == fromUserId);

        switch (actionType)
        {
            case BookmarkActionType.Add:
                if (userBookmark is null)
                {
                    _bookmarks.Add(new TBookmarkEntity
                    {
                        UserId = fromUserId.Value,
                        ParentId = fkId
                    });
                }
                else
                {
                    userBookmark.IsDeleted = false;
                    _bookmarks.Update(userBookmark);
                }

                break;
            case BookmarkActionType.Cancel:
                if (userBookmark is not null)
                {
                    _bookmarks.Remove(userBookmark);
                }

                break;
        }

        await uow.SaveChangesAsync();

        parentEntity.EntityStat.NumberOfBookmarks =
            await entityBookmarksQuery.AsNoTracking().CountAsync(x => x.ParentId == fkId);

        await uow.SaveChangesAsync();

        return true;
    }
}
