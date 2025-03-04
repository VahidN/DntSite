using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.ServicesConfigs;
using Microsoft.EntityFrameworkCore.Design;

namespace DntSite.Web.Features.Persistence.UnitOfWork;

public class SqLiteContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var services = new ServiceCollection();
        services.AddOptions();
        services.AddLogging(cfg => cfg.AddConsole().AddDebug());
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
        services.AddSingleton<IAppFoldersService, AppFoldersService>();
        services.AddScoped<IWebHostEnvironment, TestHostingEnvironment>();
        services.AddEfCoreInterceptors(new TestHostingEnvironment());

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton(_ => configuration);
        services.Configure<StartupSettingsModel>(configuration.Bind);

        using var serviceProvider = services.BuildServiceProvider();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseConfiguredSqLite(serviceProvider);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
