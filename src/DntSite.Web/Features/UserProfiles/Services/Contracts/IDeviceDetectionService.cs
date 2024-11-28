using System.Security.Claims;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IDeviceDetectionService : IScopedService
{
    public string GetDeviceDetails(HttpContext? context);

    public string GetCurrentRequestDeviceDetails();

    public string GetDeviceDetailsHash(HttpContext? context);

    public string GetCurrentRequestDeviceDetailsHash();

    public string? GetUserTokenDeviceDetailsHash(ClaimsIdentity? claimsIdentity);

    public string? GetCurrentUserTokenDeviceDetailsHash();

    public bool HasUserTokenValidDeviceDetails(ClaimsIdentity? claimsIdentity);

    public bool HasCurrentUserTokenValidDeviceDetails();
}
