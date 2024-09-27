using AutoMapper.QueryableExtensions;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using Gridify;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace DntSite.Web.Features.Common.Utils.Pagings;

public static class GridifyUtils
{
    /// <summary>
    ///     Supports parameterized and special type EF-Core queries
    /// </summary>
    static GridifyUtils()
    {
        GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();

        // If true, string comparison operations are case insensitive by default.
        // https://alirezanet.github.io/Gridify/guide/gridifyMapper#caseinsensitivefiltering
        GridifyGlobalConfiguration.CaseInsensitiveFiltering = true;
    }

    public static async Task<PagedResultModel<TResult>> ApplyQueryableDntGridFilterAsync<TResult, TEntity>(
        this IQueryable<TEntity> query,
        DntQueryBuilderModel? state,
        string defaultSortField,
        IConfigurationProvider mapperConfiguration,
        IList<GridifyMap<TEntity>>? customMappings = null)
        where TEntity : class
        where TResult : class
    {
        state ??= new DntQueryBuilderModel();

        var mappings = new GridifyMapper<TEntity>().GenerateMappings();

        if (customMappings is not null)
        {
            foreach (var map in customMappings)
            {
                mappings.AddMap(map.From, map.To, map.Convertor, map.OverrideIfExists);
            }
        }

        var gridifyQuery = new GridifyQuery
        {
            Filter = state.GridifyFilter,
            Page = state.Page,
            PageSize = state.PageSize,
            OrderBy = GetSortFilter(state, defaultSortField)
        };

        var filteredQuery = query.AsNoTracking().ApplyFiltering(gridifyQuery, mappings);
        var totalItems = await filteredQuery.CountAsync();
        filteredQuery = filteredQuery.ApplyOrdering(gridifyQuery, mappings).ApplyPaging(gridifyQuery);

        return new PagedResultModel<TResult>
        {
            TotalItems = totalItems,
            Data = await filteredQuery.ProjectTo<TResult>(mapperConfiguration).ToListAsync()
        };
    }

    public static async Task<PagedResultModel<TEntity>> ApplyQueryableDntGridFilterAsync<TEntity>(
        this IQueryable<TEntity> query,
        DntQueryBuilderModel? state,
        string defaultSortField,
        IList<GridifyMap<TEntity>>? customMappings = null)
        where TEntity : class
    {
        state ??= new DntQueryBuilderModel();

        var mappings = new GridifyMapper<TEntity>().GenerateMappings();

        if (customMappings is not null)
        {
            foreach (var map in customMappings)
            {
                mappings.AddMap(map.From, map.To, map.Convertor, map.OverrideIfExists);
            }
        }

        var gridifyQuery = new GridifyQuery
        {
            Filter = state.GridifyFilter,
            Page = state.Page,
            PageSize = state.PageSize,
            OrderBy = GetSortFilter(state, defaultSortField)
        };

        var filteredQuery = query.AsNoTracking().ApplyFiltering(gridifyQuery, mappings);
        var totalItems = await filteredQuery.CountAsync();
        filteredQuery = filteredQuery.ApplyOrdering(gridifyQuery, mappings).ApplyPaging(gridifyQuery);

        return new PagedResultModel<TEntity>
        {
            TotalItems = totalItems,
            Data = await filteredQuery.ToListAsync()
        };
    }

    public static Task<PagedResultModel<TEntity>> ApplyEnumerableDntGridFilterAsync<TEntity>(
        this IEnumerable<TEntity> sequence,
        DntQueryBuilderModel? state,
        string defaultSortField,
        IList<GridifyMap<TEntity>>? customMappings = null)
        where TEntity : class
    {
        var query = sequence.AsQueryable();

        return ApplyQueryableDntGridFilterAsync(query, state, defaultSortField, customMappings);
    }

    public static string NormalizeGridifyFilter(this string? filter)
        => string.IsNullOrWhiteSpace(filter) || string.Equals(filter, b: "*", StringComparison.Ordinal) ? "" : filter;

    private static string GetSortFilter(DntQueryBuilderModel state, string defaultSortField)
    {
        if (string.IsNullOrWhiteSpace(state.SortBy) && string.IsNullOrWhiteSpace(defaultSortField))
        {
            return string.Empty;
        }

        var direction = state.IsAscending ? "asc" : "desc";

        return string.IsNullOrWhiteSpace(state.SortBy)
            ? $"{defaultSortField} {direction}"
            : $"{state.SortBy} {direction}";
    }
}
