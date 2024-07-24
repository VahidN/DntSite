namespace DntSite.Web.Features.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    ///     Return the current limited DbSet with all of the applied `QueryFilters` to it.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    DbSet<TEntity> DbSet<TEntity>()
        where TEntity : class;

    /// <summary>
    ///     Return the current limited AsNoTracking DbSet with all of the applied `QueryFilters` to it.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    IQueryable<TEntity> DbSetAsNoTracking<TEntity>()
        where TEntity : class;

    /// <summary>
    ///     Applies `IgnoreQueryFilters()` to the current DbSet to return all of the records without any limitations.
    /// </summary>
    IQueryable<TEntity> DbSetAll<TEntity>()
        where TEntity : class;

    void AddRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;

    void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class;

    void MarkAsChanged<TEntity>(TEntity entity)
        where TEntity : class;

    T? GetShadowPropertyValue<T>(object entity, string propertyName)
        where T : IConvertible;

    object? GetShadowPropertyValue(object entity, string propertyName);

    void Migrate(TimeSpan timeout);

    void ExecuteSqlInterpolatedCommand(FormattableString query);

    void ExecuteSqlRawCommand(string query, params object[] parameters);

    int SaveChanges(bool acceptAllChangesOnSuccess);

    int SaveChanges();

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new());

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}
