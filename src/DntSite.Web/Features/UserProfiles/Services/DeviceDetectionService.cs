using System.Security.Claims;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.Net.Http.Headers;
using UAParser;

namespace DntSite.Web.Features.UserProfiles.Services;

/// <summary>
///     To invalidate an old user's token from a new device
/// </summary>
public class DeviceDetectionService(IPasswordHasherService securityService, IHttpContextAccessor httpContextAccessor)
    : IDeviceDetectionService
{
    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    private readonly IPasswordHasherService _securityService =
        securityService ?? throw new ArgumentNullException(nameof(securityService));

    public string GetCurrentRequestDeviceDetails() => GetDeviceDetails(_httpContextAccessor.HttpContext);

    public string GetDeviceDetails(HttpContext? context)
    {
        var ua = GetUserAgent(context);

        if (ua is null)
        {
            return "unknown";
        }

        var client = Parser.GetDefault().Parse(ua);
        var deviceInfo = client.Device.Family;
        var browserInfo = $"{client.UA.Family}, {client.UA.Major}.{client.UA.Minor}";
        var osInfo = $"{client.OS.Family}, {client.OS.Major}.{client.OS.Minor}";

        return $"{deviceInfo}, {browserInfo}, {osInfo}";
    }

    public string GetDeviceDetailsHash(HttpContext? context)
        => _securityService.GetSha256Hash(GetDeviceDetails(context));

    public string GetCurrentRequestDeviceDetailsHash() => GetDeviceDetailsHash(_httpContextAccessor.HttpContext);

    public string? GetCurrentUserTokenDeviceDetailsHash()
        => GetUserTokenDeviceDetailsHash(_httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity);

    public string? GetUserTokenDeviceDetailsHash(ClaimsIdentity? claimsIdentity)
    {
        if (claimsIdentity?.Claims is null || !claimsIdentity.Claims.Any())
        {
            return null;
        }

        return claimsIdentity.FindFirst(ClaimTypes.System)?.Value;
    }

    public bool HasCurrentUserTokenValidDeviceDetails()
        => HasUserTokenValidDeviceDetails(_httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity);

    public bool HasUserTokenValidDeviceDetails(ClaimsIdentity? claimsIdentity)
        => string.Equals(GetCurrentRequestDeviceDetailsHash(), GetUserTokenDeviceDetailsHash(claimsIdentity),
            StringComparison.Ordinal);

    private static string? GetUserAgent(HttpContext? context)
    {
        if (context is null)
        {
            return null;
        }

        return context.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var userAgent)
            ? userAgent.ToString()
            : null;
    }
}
