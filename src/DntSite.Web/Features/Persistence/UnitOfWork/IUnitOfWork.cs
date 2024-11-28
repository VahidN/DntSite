using Microsoft.EntityFrameworkCore.Query;

namespace DntSite.Web.Features.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    ///     Return the current limited DbSet with all of the applied `QueryFilters` to it.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public DbSet<TEntity> DbSet<TEntity>()
        where TEntity : class;

    /// <summary>
    ///     Return the current limited AsNoTracking DbSet with all of the applied `QueryFilters` to it.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public IQueryable<TEntity> DbSetAsNoTracking<TEntity>()
        where TEntity : class;

    /// <summary>
    ///     Applies `IgnoreQueryFilters()` to the current DbSet to return all of the records without any limitations.
    /// </summary>
    public IQueryable<TEntity> DbSetAll<TEntity>()
        where TEntity : class;

    public void AddRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;

    public EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class;

    public void MarkAsChanged<TEntity>(TEntity entity)
        where TEntity : class;

    public T? GetShadowPropertyValue<T>(object entity, string propertyName)
        where T : IConvertible;

    public object? GetShadowPropertyValue(object entity, string propertyName);

    public void Migrate(TimeSpan timeout);

    public void ExecuteSqlInterpolatedCommand(FormattableString query);

    public void ExecuteSqlRawCommand(string query, params object[] parameters);

    public IQueryable<TResult> SqlQuery<TResult>([NotParameterized] FormattableString query);

    public IQueryable<TResult> SqlQueryRaw<TResult>([NotParameterized] string sql, params object[] parameters);

    public int SaveChanges(bool acceptAllChangesOnSuccess);

    public int SaveChanges();

    public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new());

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());

    public Task ExecuteTransactionAsync(Func<Task> action);
}
