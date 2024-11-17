using Polly;

namespace DntSite.Web.Features.Persistence.Utils;

public static class MigrationHelpers
{
    public static void MigrateDbContext<TContext>(this IServiceProvider serviceProvider,
        Action<IServiceProvider>? postMigrationAction = null)
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var scopedServiceProvider = scope.ServiceProvider;
        var logger = scopedServiceProvider.GetRequiredService<ILogger<TContext>>();

        logger.LogInformation("Migrating the DB associated with the context {Name}", typeof(TContext).Name);

        var retry = Policy.Handle<Exception>()
            .WaitAndRetry([
                TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15)
            ]);

        retry.Execute(() =>
        {
            using var context = scopedServiceProvider.GetRequiredService<TContext>();
            context.Database.Migrate();
            postMigrationAction?.Invoke(scopedServiceProvider);
        });

        logger.LogInformation("Migrated the DB associated with the context {Name}", typeof(TContext).Name);
    }
}
