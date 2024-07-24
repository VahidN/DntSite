using DntSite.Web.Features;
using DntSite.Web.Features.Common.Utils.Security;
using DntSite.Web.Features.DbLogger.Services;
using DntSite.Web.Features.DbSeeder.Services;
using DntSite.Web.Features.ServicesConfigs;
using DntSite.Web.Features.UserProfiles.Endpoints;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder.Logging, builder.Environment, builder.Configuration);
ConfigureServices(builder.Services, builder.Environment, builder.Configuration);
var webApp = builder.Build();
ConfigureMiddlewares(webApp, webApp.Environment);
ConfigureEndpoints(webApp);
InitApplication(webApp);
await RunAsync(webApp);

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

    app.UseExceptionHandler(errorHandlingPath: "/Error", createScopeForErrors: true);

    var headerPolicyCollection = SecurityHeadersBuilder.GetCsp(env.IsDevelopment());
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

void ConfigureEndpoints(IEndpointRouteBuilder app)
{
    app.MapControllers();
    app.AddChangePasswordEndpoint();
    app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
}

void InitApplication(IHost app)
{
    app.CompileAutoMapperConfig();
    app.InitializeDb();
}

Task RunAsync(WebApplication webApplication)
{
    var runTask = webApplication.RunAsync();

    if (!Debugger.IsAttached)
    {
        var server = webApplication.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;

        if (addresses is null || addresses.Count == 0)
        {
            addresses = ["-> http://localhost:5000"];
        }

        WriteLine(Invariant(
            $"{DateTime.UtcNow:HH:mm:ss.fff} Started webApp.RunAsync() @ {string.Join(separator: ", ", addresses)}"));
    }

    return runTask;
}
