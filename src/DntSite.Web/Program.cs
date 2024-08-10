using DntSite.Web.Features;
using DntSite.Web.Features.Common.Utils.Security;
using DntSite.Web.Features.DbLogger.Services;
using DntSite.Web.Features.DbSeeder.Services;
using DntSite.Web.Features.ServicesConfigs;
using DntSite.Web.Features.Stats.Middlewares;
using DntSite.Web.Features.UserProfiles.Endpoints;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder.Logging, builder.Environment, builder.Configuration);
ConfigureServices(builder.Services, builder.Environment, builder.Configuration);
var webApp = builder.Build();
ConfigureMiddlewares(webApp, webApp.Environment);
ConfigureEndpoints(webApp);
InitApplication(webApp);
await RunAsync(webApp, webApp.Environment);

void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
{
    services.AddRazorComponents().AddInteractiveServerComponents();
    services.AddControllers();
    services.AddCustomizedServices(configuration, environment);
}

void ConfigureLogging(ILoggingBuilder logging, IHostEnvironment env, IConfiguration configuration)
{
    logging.ClearProviders();

    if (env.IsDevelopment())
    {
        logging.AddConsole();
        logging.AddDebug();
    }

    logging.AddConfiguration(configuration.GetSection(key: "Logging"));
    logging.AddDbLogger(); // You can change its Log Level using the `appsettings.json` file -> Logging -> LogLevel -> Default
}

void ConfigureMiddlewares(IApplicationBuilder app, IHostEnvironment env)
{
    if (OperatingSystem.IsLinux())
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
    }

    app.UseMiddleware<OnlineVisitorsMiddleware>();

    app.UseExceptionHandler(errorHandlingPath: "/Error", createScopeForErrors: true);

    var headerPolicyCollection = SecurityHeadersBuilder.GetCsp(env.IsDevelopment(), enableCrossOriginPolicy: false);

    app.UseSecurityHeaders(headerPolicyCollection);

    if (!env.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseStatusCodePagesWithReExecute(pathFormat: "/Error/{0}");

    var provider = new FileExtensionContentTypeProvider();

    app.UseStaticFiles(new StaticFileOptions
    {
        ContentTypeProvider = provider
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();
    app.UseOutputCache();
}

void ConfigureEndpoints(WebApplication app)
{
    app.MapControllers();
    app.AddChangePasswordEndpoint();
    app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
    app.UseRequestTimeouts();
}

void InitApplication(IHost app)
{
    app.CompileAutoMapperConfig();
    app.InitializeDb();
}

Task RunAsync(WebApplication webApplication, IHostEnvironment env)
{
    var runTask = webApplication.RunAsync();

    if (!Debugger.IsAttached)
    {
        var hostEndPoints = string.Join(separator: ", ", webApplication.GetKestrelListeningAddresses());

        WriteLine(Invariant(
            $"{DateTime.UtcNow:HH:mm:ss.fff} Started webApp[V{Assembly.GetExecutingAssembly().GetBuildDateTime()}].RunAsync() with IsDevelopment:{env.IsDevelopment()} @ {hostEndPoints}"));
    }

    return runTask;
}
