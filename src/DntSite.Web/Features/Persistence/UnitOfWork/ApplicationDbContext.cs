using DntSite.Web.Features.AppConfigs.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.Utils;
using Microsoft.EntityFrameworkCore.Query;

namespace DntSite.Web.Features.Persistence.UnitOfWork;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public IQueryable<TEntity> DbSetAll<TEntity>()
        where TEntity : class
        => Set<TEntity>().IgnoreQueryFilters();

    public void AddRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
        => Set<TEntity>().AddRange(entities);

    public void ExecuteSqlInterpolatedCommand(FormattableString query) => Database.ExecuteSqlInterpolated(query);

    public void ExecuteSqlRawCommand(string query, params object[] parameters)
        => Database.ExecuteSqlRaw(query, parameters);

    public IQueryable<TResult> SqlQuery<TResult>([NotParameterized] FormattableString query)
        => Database.SqlQuery<TResult>(query);

    public IQueryable<TResult> SqlQueryRaw<TResult>([NotParameterized] string sql, params object[] parameters)
        => Database.SqlQueryRaw<TResult>(sql, parameters);

    public T? GetShadowPropertyValue<T>(object entity, string propertyName)
        where T : IConvertible
    {
        var value = Entry(entity).Property(propertyName).CurrentValue;

        return value is not null ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture) : default;
    }

    public object? GetShadowPropertyValue(object entity, string propertyName)
        => Entry(entity).Property(propertyName).CurrentValue;

    public void Migrate(TimeSpan timeout)
    {
        Database.SetCommandTimeout(timeout);
        Database.Migrate();
    }

    public void MarkAsChanged<TEntity>(TEntity entity)
        where TEntity : class
        => Update(entity);

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
        => Set<TEntity>().RemoveRange(entities);

    public DbSet<TEntity> DbSet<TEntity>()
        where TEntity : class
        => Set<TEntity>();

    public IQueryable<TEntity> DbSetAsNoTracking<TEntity>()
        where TEntity : class
        => Set<TEntity>().AsNoTracking();

    public async Task ExecuteTransactionAsync(Func<Task> action)
    {
        var strategy = Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        WriteLine(
            string.Create(CultureInfo.InvariantCulture, $"{DateTime.UtcNow:HH:mm:ss.fff} Started OnModelCreating"));

        // it should be placed here, otherwise it will rewrite the following settings!
        base.OnModelCreating(modelBuilder);

        // Custom application mappings
        Type[] tphBaseTypes =
        [
            typeof(ParentBookmarkEntity), typeof(ParentVisitorEntity), typeof(ParentReactionEntity),
            typeof(ParentUserFileEntity)
        ];

        modelBuilder.RegisterAllDerivedEntities<BaseEntity>(tphBaseTypes);
        modelBuilder.MakeAllDerivedTableNamesPluralized<BaseEntity>(tphBaseTypes);
        modelBuilder.ApplyBaseEntityConfigurationToAllDerivedEntities();
        modelBuilder.ConfigureTph(tphBaseTypes);
        modelBuilder.SetCaseInsensitiveSearchesForSqLite();

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Started ApplyConfigurationsFromAssembly"));

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDataProtectionKeyConfig).Assembly);

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Finished ApplyConfigurationsFromAssembly"));

        modelBuilder.SetDecimalPrecision();

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Finished OnModelCreating"));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeAsUtcValueConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeAsUtcValueConverter>();
    }
}
