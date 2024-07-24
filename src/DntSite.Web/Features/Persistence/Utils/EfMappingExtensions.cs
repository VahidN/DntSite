using AutoMapper.QueryableExtensions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace DntSite.Web.Features.Persistence.Utils;

public static class EfMappingExtensions
{
    public static Task<List<TDestination>> ProjectAsNoTrackingToListAsync<TDestination>(this IQueryable queryable,
        IConfigurationProvider configuration)
        where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable,
        IConfigurationProvider configuration)
        where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
}
