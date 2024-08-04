using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Http.Timeouts;

namespace DntSite.Web.Features.ServicesConfigs;

public static class MvcControllersConfig
{
    public static IMvcBuilder AddCustomizedControllers(this IServiceCollection services)
        => services.AddProblemDetails()
            .Configure<RouteOptions>(opt =>
            {
                opt.ConstraintMap.Add(EncryptedRouteConstraint.Name, typeof(EncryptedRouteConstraint));
            })
            .AddRequestTimeouts(options =>
            {
                options.DefaultPolicy = new RequestTimeoutPolicy
                {
                    Timeout = TimeSpan.FromMinutes(value: 30),
                    TimeoutStatusCode = StatusCodes.Status503ServiceUnavailable
                };
            })
            .AddLargeFilesUploadSupport()
            .AddOutputCache(options => { options.AddPolicy(AlwaysCachePolicy.Name, AlwaysCachePolicy.Instance); })
            .AddControllers(options => { options.Filters.Add(typeof(ApplyCorrectYeKeFilterAttribute)); })
            .AddJsonOptions(AddCustomJsonOptions);

    private static void AddCustomJsonOptions(JsonOptions options)
    {
        var jsonSerializerOptions = options.JsonSerializerOptions;
        jsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        jsonSerializerOptions.WriteIndented = true;
        jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        jsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        jsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }
}
