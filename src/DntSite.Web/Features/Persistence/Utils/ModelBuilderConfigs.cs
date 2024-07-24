namespace DntSite.Web.Features.Persistence.Utils;

public static class ModelBuilderConfigs
{
    public static void SetDecimalPrecision(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Started SetDecimalPrecision"));

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType(value: "decimal(18, 6)");
        }

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Finished SetDecimalPrecision"));
    }

    public static void SetCaseInsensitiveSearchesForSqLite(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Started SetCaseInsensitiveSearchesForSqLite"));
        modelBuilder.UseCollation(collation: "NOCASE");

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(string)))
        {
            property.SetCollation(collation: "NOCASE");
        }

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Finished SetCaseInsensitiveSearchesForSqLite"));
    }
}
