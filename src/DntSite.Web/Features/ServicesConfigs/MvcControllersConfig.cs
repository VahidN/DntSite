using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Http.Timeouts;

namespace DntSite.Web.Features.ServicesConfigs;

public static class MvcControllersConfig
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new JsonStringEnumConverter()
        },
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

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
            .AddControllers(options => { options.Filters.Add<ApplyCorrectYeKeFilterAttribute>(); })
            .AddJsonOptions(AddCustomJsonOptions);

    private static void AddCustomJsonOptions(JsonOptions options)
    {
        var jsonSerializerOptions = options.JsonSerializerOptions;
        jsonSerializerOptions.NumberHandling = JsonSerializerOptions.NumberHandling;
        jsonSerializerOptions.PropertyNameCaseInsensitive = JsonSerializerOptions.PropertyNameCaseInsensitive;
        jsonSerializerOptions.WriteIndented = JsonSerializerOptions.WriteIndented;
        jsonSerializerOptions.DefaultIgnoreCondition = JsonSerializerOptions.DefaultIgnoreCondition;
        jsonSerializerOptions.Converters.AddRange(JsonSerializerOptions.Converters);
        jsonSerializerOptions.Encoder = JsonSerializerOptions.Encoder;
        jsonSerializerOptions.ReferenceHandler = JsonSerializerOptions.ReferenceHandler;
    }
}
