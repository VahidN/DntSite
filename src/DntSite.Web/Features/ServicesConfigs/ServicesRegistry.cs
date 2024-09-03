using System.Security.Claims;
using System.Security.Principal;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.DbSeeder.Services.Contracts;

namespace DntSite.Web.Features.ServicesConfigs;

public static class ServicesRegistry
{
    public static void AddCustomizedServices(this IServiceCollection services,
        IHostBuilder host,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        host.AlwaysValidateScopes();

        services.AddOptions(configuration);
        services.AddHttpContextAccessor();
        services.AddIPrincipal();
        services.ScanAllServices();

        var siteSettings = configuration.GetSiteSettings();
        services.AddConfiguredDbContext(siteSettings, environment);
        services.AddCustomizedDataProtection(siteSettings);
        services.AddDNTCommonWeb();
        services.AddAutoMapper();
        services.AddSchedulers();
        services.AddCustomizedControllers();
        services.AddCustomizedAuthentication(siteSettings, environment);
    }

    private static void AlwaysValidateScopes(this IHostBuilder host)
        => host.UseDefaultServiceProvider(options =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });

    private static void AddIPrincipal(this IServiceCollection services)
        => services.AddScoped<IPrincipal>(provider
            => provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User ??
               ClaimsPrincipal.Current ?? new ClaimsPrincipal());

    private static void AddAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(cfg => { }, typeof(AutoMapperConfig).Assembly);

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<StartupSettingsModel>().Bind(configuration);
        services.Configure<AntiXssConfig>(options => configuration.GetSection(key: "AntiXssConfig").Bind(options));
    }

    private static void ScanAllServices(this IServiceCollection services)
    {
        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Started ScanAllServices"));

        // Using the `Scrutor` to add all of the application's services at once.
        services.Scan(scan => scan.FromAssembliesOf(typeof(IDataSeedersRunner))
            .AddClasses(classes => classes.AssignableTo<ISingletonService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
            .AddClasses(classes => classes.AssignableTo<BackgroundService>())
            .As<IHostedService>()
            .WithSingletonLifetime()
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<ITransientService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .AddClasses(classes => classes.Where(type =>
            {
                var allInterfaces = type.GetInterfaces();

                return allInterfaces.Contains(typeof(IMiddleware)) && allInterfaces.Contains(typeof(ISingletonService));
            }))
            .AsSelf()
            .WithSingletonLifetime());

        WriteLine(Invariant($"{DateTime.UtcNow:HH:mm:ss.fff} Finished ScanAllServices"));
    }

    private static StartupSettingsModel GetSiteSettings(this IConfiguration configuration)
        => configuration.Get<StartupSettingsModel>() ??
           throw new InvalidOperationException(message: "SiteSettings is null.");
}
