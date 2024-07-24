using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Persistence.Interceptors;
using EFCoreSecondLevelCacheInterceptor;

namespace DntSite.Web.Features.Persistence.UnitOfWork;

public static class SqLiteServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredSqLiteDbContext(this IServiceCollection services)
    {
        services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder)
            => optionsBuilder.UseConfiguredSqLite(serviceProvider));

        services.AddScoped<IUnitOfWork>(serviceProvider =>
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            SetCascadeOnSaveChanges(context);

            return context;
        });

        return services;
    }

    private static void SetCascadeOnSaveChanges(DbContext context)
    {
        // To fix https://github.com/dotnet/efcore/issues/19786
        context.ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
        context.ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
    }

    public static void UseConfiguredSqLite(this DbContextOptionsBuilder optionsBuilder,
        IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        var appFoldersService = serviceProvider.GetRequiredService<IAppFoldersService>();
        var connectionString = appFoldersService.DefaultConnectionString;

        optionsBuilder.UseSqlite(connectionString, sqliteOptionsBuilder =>
        {
            sqliteOptionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
            sqliteOptionsBuilder.MigrationsAssembly(typeof(SqLiteServiceCollectionExtensions).Assembly.FullName);
        });

        optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor(),
            serviceProvider.GetRequiredService<AuditableEntitiesInterceptor>(),
            serviceProvider.GetRequiredService<EfExceptionsInterceptor>(),
            serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());

        optionsBuilder.ConfigureWarnings(warnings =>
        {
            warnings.Log((CoreEventId.LazyLoadOnDisposedContextWarning, LogLevel.Warning),
                (CoreEventId.DetachedLazyLoadingWarning, LogLevel.Warning),
                (CoreEventId.ManyServiceProvidersCreatedWarning, LogLevel.Warning),
                (CoreEventId.SensitiveDataLoggingEnabledWarning, LogLevel.Debug),
                (RelationalEventId.MultipleCollectionIncludeWarning, LogLevel.Debug));
        });

        optionsBuilder.EnableSensitiveDataLogging().EnableDetailedErrors();
    }
}
