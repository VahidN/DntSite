using DntSite.Web.Features.DbSeeder.Services.Contracts;

namespace DntSite.Web.Features.DbSeeder.Services;

public static class DataSeedersRunnerExtensions
{
    /// <summary>
    ///     Creates and seeds the database.
    /// </summary>
    public static IHost InitializeDb(this IHost host)
    {
        ArgumentNullException.ThrowIfNull(host);

        WriteLine(string.Create(CultureInfo.InvariantCulture, $"{DateTime.UtcNow:HH:mm:ss.fff} Started InitializeDb"));
        host.Services.RunScopedService<IDataSeedersRunner>(runner => runner.RunAllDataSeeders());
        WriteLine(string.Create(CultureInfo.InvariantCulture, $"{DateTime.UtcNow:HH:mm:ss.fff} Finished InitializeDb"));

        return host;
    }
}
