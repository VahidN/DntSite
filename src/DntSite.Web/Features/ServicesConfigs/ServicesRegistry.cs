using System.Security.Claims;
using System.Security.Principal;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.DbSeeder.Services.Contracts;
using Microsoft.AspNetCore.HttpOverrides;

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
        services.AddForwardedHeadersOptions();

        services.AddHttpContextAccessor();
        services.AddIPrincipal();
        services.AutoInjectAllServices();

        var siteSettings = configuration.GetSiteSettings();
        services.AddConfiguredDbContext(siteSettings, environment);
        services.AddCustomizedDataProtection(siteSettings);
        services.AddDNTCommonWeb();
        services.AddAutoMapper();
        services.AddSchedulers();
        services.RunHostedServicesConcurrently();
        services.AddCustomizedControllers();
        services.AddCustomizedAuthentication(siteSettings, environment);
    }

    private static void AddAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(_ => { }, typeof(AutoMapperConfig).Assembly);

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<StartupSettingsModel>().Bind(configuration);
        services.Configure<AntiXssConfig>(options => configuration.GetSection(key: "AntiXssConfig").Bind(options));
        services.Configure<AntiDosConfig>(options => configuration.GetSection(key: "AntiDosConfig").Bind(options));
    }

    private static StartupSettingsModel GetSiteSettings(this IConfiguration configuration)
        => configuration.Get<StartupSettingsModel>() ??
           throw new InvalidOperationException(message: "SiteSettings is null.");
}
