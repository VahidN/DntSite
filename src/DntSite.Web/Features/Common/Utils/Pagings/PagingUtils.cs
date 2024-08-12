using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Common.Utils.Pagings;

public static class PagingUtils
{
    public static async Task<PagedResultModel<TEntity>> ApplyQueryablePagingAsync<TEntity>(
        this IOrderedQueryable<TEntity> query,
        int pageNumber,
        int recordsPerPage)
        where TEntity : BaseEntity
        => new()
        {
            TotalItems = await query.CountAsync(),
            Data = await query.ApplyPaging(pageNumber, recordsPerPage).ToListAsync()
        };

    public static async Task<PagedResultModel<TEntity>> ApplyQueryablePagingAsync<TEntity>(
        this IQueryable<TEntity> query,
        int pageNumber,
        int recordsPerPage,
        PagerSortBy sortBy,
        bool isAscending,
        IDictionary<PagerSortBy, Expression<Func<TEntity, object?>>> customOrders)
        where TEntity : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(customOrders);

        return new PagedResultModel<TEntity>
        {
            TotalItems = await query.CountAsync(),
            Data = await query.ApplyOrdering(sortBy, isAscending, customOrders)
                .ApplyPaging(pageNumber, recordsPerPage)
                .ToListAsync()
        };
    }

    private static IQueryable<TEntity> ApplyOrdering<TEntity>(this IQueryable<TEntity> query,
        PagerSortBy sortBy,
        bool isAscending,
        IDictionary<PagerSortBy, Expression<Func<TEntity, object?>>> customOrders)
        where TEntity : BaseEntity
    {
        if (customOrders.TryGetValue(sortBy, out var orderBy))
        {
            query = !isAscending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }
        else
        {
            query = !isAscending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query,
        int pageNumber,
        int recordsPerPage)
        where TEntity : BaseEntity
    {
        var skipRecords = pageNumber * recordsPerPage;
        query = query.Skip(skipRecords).Take(recordsPerPage);

        return query;
    }
}
