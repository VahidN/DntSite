using DntSite.Web.Features.AppConfigs.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Persistence.Utils;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Started OnModelCreating"));

        // it should be placed here, otherwise it will rewrite the following settings!
        base.OnModelCreating(modelBuilder);

        // Custom application mappings
        Type[] tphBaseTypes =
        [
            typeof(ParentBookmarkEntity), typeof(ParentVisitorEntity), typeof(ParentReactionEntity),
            typeof(ParentUserFileEntity)
        ];

        modelBuilder.RegisterAllDerivedEntities<BaseAuditedEntity>(tphBaseTypes);
        modelBuilder.MakeAllDerivedTableNamesPluralized<BaseAuditedEntity>(tphBaseTypes);
        modelBuilder.ApplyBaseEntityConfigurationToAllDerivedEntities();
        modelBuilder.ConfigureTph(tphBaseTypes);
        modelBuilder.SetCaseInsensitiveSearchesForSqLite();

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Started ApplyConfigurationsFromAssembly"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDataProtectionKeyConfig).Assembly);
        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Finished ApplyConfigurationsFromAssembly"));

        modelBuilder.SetDecimalPrecision();

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Finished OnModelCreating"));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeAsUtcValueConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeAsUtcValueConverter>();
    }
}
