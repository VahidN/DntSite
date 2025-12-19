using Humanizer;

namespace DntSite.Web.Features.Persistence.Utils;

public static class DbSetsConfigs
{
    public static void RegisterAllDerivedEntities<TEntity>(this ModelBuilder builder, params Type[] exceptTheseTypes)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(builder);

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Started RegisterAllEntities"));

        var baseType = typeof(TEntity);

        foreach (var entityType in baseType.Assembly.GetTypes())
        {
            if (entityType.IsAbstract || !baseType.IsAssignableFrom(entityType) ||
                ShouldSkipClrType(exceptTheseTypes, entityType))
            {
                continue;
            }

            WriteLine(entityType);
            builder.Entity(entityType);
        }

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Finished RegisterAllEntities"));
    }

    private static bool ShouldSkipClrType(IEnumerable<Type>? exceptTheseTypes, Type entity)
        => exceptTheseTypes?.Any(exceptThisType => exceptThisType.IsAssignableFrom(entity)) == true;

    public static void MakeAllDerivedTableNamesPluralized<TEntity>(this ModelBuilder builder,
        params Type[] exceptTheseTypes)
    {
        ArgumentNullException.ThrowIfNull(builder);

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Started MakeAllDerivedTableNamesPluralized"));

        var baseType = typeof(TEntity);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var displayName = entityType.DisplayName();
            var clrType = entityType.ClrType;

            if (entityType.IsKeyless || !baseType.IsAssignableFrom(clrType) ||
                ShouldSkipClrType(exceptTheseTypes, clrType) ||
                Attribute.IsDefined(clrType, typeof(TableAttribute), inherit: true) ||
                Attribute.IsDefined(clrType, typeof(ComplexTypeAttribute), inherit: true) ||
                displayName.Contains(value: ' ', StringComparison.Ordinal))
            {
                continue;
            }

            var pluralizedTableName = displayName.Pluralize();
            WriteLine($"{entityType} -> {pluralizedTableName}");
            entityType.SetTableName(pluralizedTableName);
        }

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Finished MakeAllDerivedTableNamesPluralized"));
    }

    public static void ConfigureTph(this ModelBuilder modelBuilder, params Type[] tphBaseTypes)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        WriteLine(string.Create(CultureInfo.InvariantCulture, $"{DateTime.UtcNow:HH:mm:ss.fff} Started ConfigureTph"));

        if (tphBaseTypes is null)
        {
            return;
        }

        foreach (var type in tphBaseTypes)
        {
            var pluralizedTableName = type.Name
                .Replace(oldValue: "entity", newValue: "", StringComparison.OrdinalIgnoreCase)
                .Pluralize();

            modelBuilder.Entity(type).UseTphMappingStrategy().ToTable(pluralizedTableName);
        }

        WriteLine(string.Create(CultureInfo.InvariantCulture, $"{DateTime.UtcNow:HH:mm:ss.fff} Finished ConfigureTph"));
    }
}
