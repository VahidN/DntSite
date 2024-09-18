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

    public async Task<bool> SavePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(int fkId, int? fromUserId)
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

        if (userBookmark is not null)
        {
            return false;
        }

        _bookmarks.Add(new TBookmarkEntity
        {
            UserId = fromUserId.Value,
            ParentId = fkId
        });

        await uow.SaveChangesAsync();

        parentEntity.EntityStat.NumberOfBookmarks =
            await entityBookmarksQuery.AsNoTracking().CountAsync(x => x.ParentId == fkId);

        await uow.SaveChangesAsync();

        return true;
    }

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

    public async Task<bool> DeletePostBookmarkAsync<TBookmarkEntity, TForeignKeyEntity>(int bookmarkId)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        var bookmark = await _bookmarks.FindAsync(bookmarkId);

        if (bookmark is null)
        {
            return false;
        }

        var entityBookmarksQuery = _bookmarks.OfType<TBookmarkEntity>();

        var parentEntity = await entityBookmarksQuery.Select(x => x.Parent)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Id == bookmarkId);

        _bookmarks.Remove(bookmark);
        await uow.SaveChangesAsync();

        if (parentEntity is not null)
        {
            parentEntity.EntityStat.NumberOfBookmarks =
                await entityBookmarksQuery.AsNoTracking().CountAsync(x => x.ParentId == parentEntity.Id);

            await uow.SaveChangesAsync();
        }

        return true;
    }

    public async Task<PagedResultModel<TBookmarkEntity>> GetUserBookmarksAsync<TBookmarkEntity, TForeignKeyEntity>(
        int userId,
        int pageNumber,
        int recordsPerPage = 8,
        bool isAscending = false)
        where TBookmarkEntity : BaseBookmarkEntity<TForeignKeyEntity>
        where TForeignKeyEntity : BaseAuditedInteractiveEntity
    {
        var query = _bookmarks.OfType<TBookmarkEntity>()
            .Include(x => x.Parent)
            .Where(x => x.UserId == userId)
            .AsNoTracking();

        query = !isAscending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);

        return new PagedResultModel<TBookmarkEntity>
        {
            TotalItems = await query.CountAsync(),
            Data = await query.ApplyPaging(pageNumber, recordsPerPage).ToListAsync()
        };
    }
}
