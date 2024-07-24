using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.Persistence.Interceptors;
using DntSite.Web.Features.Persistence.UnitOfWork;
using EFCoreSecondLevelCacheInterceptor;

namespace DntSite.Web.Features.ServicesConfigs;

public static class DbContextConfig
{
    private static readonly string[] NamesToIgnoreForUpdates = ["EntityStat_", "Rating_"];
    private static readonly string[] NamesToIgnoreForAllCommands = [nameof(AppLogItem)];

    public static IServiceCollection AddConfiguredDbContext(this IServiceCollection services,
        StartupSettingsModel startupSettings,
        IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(startupSettings);

        services.AddEfCoreInterceptors(environment);
        services.AddConfiguredSqLiteDbContext();

        return services;
    }

    public static void AddEfCoreInterceptors(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddSingleton<EfExceptionsInterceptor>();
        services.AddSingleton<AuditableEntitiesInterceptor>();
        services.AddEfSecondLevelCacheInterceptor(environment);
    }

    private static void AddEfSecondLevelCacheInterceptor(this IServiceCollection services,
        IWebHostEnvironment environment)
        => services.AddEFSecondLevelCache(options => options.UseMemoryCacheProvider()
            .ConfigureLogging(environment.IsDevelopment(), args =>
            {
                switch (args.EventId)
                {
                    case CacheableLogEventId.CacheHit:
                        break;
                    case CacheableLogEventId.QueryResultCached:
                        break;
                    case CacheableLogEventId.QueryResultInvalidated:
                        args.ServiceProvider.GetRequiredService<ILoggerFactory>()
                            .CreateLogger(nameof(EFCoreSecondLevelCacheInterceptor))
                            .LogWarning(message: "{EventId} -> {Message} -> {CommandText}", args.EventId, args.Message,
                                args.CommandText);

                        break;
                    case CacheableLogEventId.CachingSkipped:
                        break;
                    case CacheableLogEventId.InvalidationSkipped:
                        break;
                    case CacheableLogEventId.CachingSystemStarted:
                        break;
                    case CacheableLogEventId.CachingError:
                        break;
                    case CacheableLogEventId.QueryResultSuppressed:
                        break;
                    case CacheableLogEventId.CacheDependenciesCalculated:
                        break;
                    case CacheableLogEventId.CachePolicyCalculated:
                        break;
                }
            })
            .UseCacheKeyPrefix(prefix: "EF_")
            .CacheAllQueriesExceptContainingTypes(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(value: 5),
                typeof(AppLogItem))
            .SkipCachingCommands(commandText
                => commandText.Contains(value: "NEWID()", StringComparison.InvariantCultureIgnoreCase))
            .SkipCacheInvalidationCommands(commandText
                => ShouldIgnoreUpdateCommands(commandText) || ShouldIgnoreForAllCommands(commandText))
            .UseDbCallsIfCachingProviderIsDown(TimeSpan.FromMinutes(value: 1)));

    private static bool ShouldIgnoreForAllCommands(string commandText)
        => NamesToIgnoreForAllCommands.Any(item
            => commandText.Contains(item, StringComparison.InvariantCultureIgnoreCase));

    private static bool ShouldIgnoreUpdateCommands(string commandText)
        => commandText.Contains(value: "update ", StringComparison.InvariantCultureIgnoreCase) &&
           NamesToIgnoreForUpdates.Any(item => commandText.Contains(item, StringComparison.InvariantCultureIgnoreCase));
}
