using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;

public static class BaseAuditedEntityConfigurations
{
    private static readonly MethodInfo ConfigureBaseEntityMethodInfo =
        new Action<ModelBuilder>(ConfigureBaseEntity<BaseAuditedEntity>).Method.GetGenericMethodDefinition();

    private static readonly Type BaseEntityType = typeof(BaseAuditedEntity);

    public static void ApplyBaseEntityConfigurationToAllDerivedEntities(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        WriteLine(Invariant(
            $"{DateTime.UtcNow:HH:mm:ss.fff} Started ApplyBaseEntityConfigurationToAllDerivedEntities"));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(type => BaseEntityType.IsAssignableFrom(type.ClrType))
                     .ToList())
        {
            ConfigureBaseEntityMethodInfo.MakeGenericMethod(entityType.ClrType).Invoke(obj: null, [modelBuilder]);
        }

        WriteLine(Invariant(
            $"{DateTime.UtcNow:HH:mm:ss.fff} Finished ApplyBaseEntityConfigurationToAllDerivedEntities"));
    }

    private static void ConfigureBaseEntity<TEntity>(ModelBuilder modelBuilder)
        where TEntity : BaseAuditedEntity
        => modelBuilder.Entity<TEntity>(builder =>
        {
            builder.OwnsMany(baseEntity => baseEntity.AuditActions, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsMany(auditField => auditField.AffectedColumns);
            });
        });
}
