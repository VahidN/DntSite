using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.ServicesConfigs;

public static class DataProtectionConfig
{
    public static IServiceCollection AddCustomizedDataProtection(this IServiceCollection services,
        StartupSettingsModel startupSettings)
    {
        ArgumentNullException.ThrowIfNull(startupSettings);

        // Persist keys to DB
        services.AddSingleton<IXmlRepository, DataProtectionKeyService>();

        services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(serviceProvider =>
        {
            return new ConfigureOptions<KeyManagementOptions>(options =>
            {
                serviceProvider.RunScopedService<IXmlRepository>(xmlRepository
                    => options.XmlRepository = xmlRepository);
            });
        });

        services.AddDataProtection()
            .SetDefaultKeyLifetime(startupSettings.DataProtectionOptions.DataProtectionKeyLifetime)
            .SetApplicationName(startupSettings.DataProtectionOptions.ApplicationName);

        return services;
    }
}
