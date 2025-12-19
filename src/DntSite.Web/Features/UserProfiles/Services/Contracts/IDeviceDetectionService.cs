using System.Security.Claims;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IDeviceDetectionService : IScopedService
{
    string GetDeviceDetails(HttpContext? context);

    string GetCurrentRequestDeviceDetails();

    string GetDeviceDetailsHash(HttpContext? context);

    string GetCurrentRequestDeviceDetailsHash();

    string? GetUserTokenDeviceDetailsHash(ClaimsIdentity? claimsIdentity);

    string? GetCurrentUserTokenDeviceDetailsHash();

    bool HasUserTokenValidDeviceDetails(ClaimsIdentity? claimsIdentity);

    bool HasCurrentUserTokenValidDeviceDetails();
}
