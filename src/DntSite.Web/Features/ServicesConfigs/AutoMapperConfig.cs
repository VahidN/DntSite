using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace DntSite.Web.Features.ServicesConfigs;

public static class AutoMapperConfig
{
    /// <summary>
    ///     Validates the Auto-Mapper's config
    /// </summary>
    public static IHost CompileAutoMapperConfig(this IHost host)
    {
        ArgumentNullException.ThrowIfNull(host);

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Started CompileAutoMapperConfig"));

        host.Services.RunScopedService<IConfigurationProvider>(configurationProvider =>
        {
            configurationProvider.AssertConfigurationIsValid();
            configurationProvider.CompileMappings();
        });

        WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"{DateTime.UtcNow:HH:mm:ss.fff} Finished CompileAutoMapperConfig"));

        return host;
    }
}
