using System.Security.Claims;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DntSite.Web.Features.UserProfiles.Services;

public class CookieValidatorService(
    IUsersInfoService usersService,
    IUserProfilesManagerService userProfilesManagerService,
    IDeviceDetectionService deviceDetectionService) : ICookieValidatorService
{
    public async Task<bool> IsValidateUserAsync(ClaimsIdentity? claimsIdentity)
    {
        if (claimsIdentity?.Claims is null || !claimsIdentity.Claims.Any())
        {
            // this is not our issued cookie
            return false;
        }

        if (!deviceDetectionService.HasUserTokenValidDeviceDetails(claimsIdentity))
        {
            // Detected usage of an old token from a new device! Please login again!
            return false;
        }

        var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);

        if (serialNumberClaim is null)
        {
            // this is not our issued cookie
            return false;
        }

        var userId = GetUserId(claimsIdentity);

        if (!userId.HasValue)
        {
            // this is not our issued cookie
            return false;
        }

        var user = await usersService.FindUserAsync(userId.Value);

        if (user is null ||
            !string.Equals(user.SerialNumber ?? "", serialNumberClaim.Value, StringComparison.Ordinal) ||
            !user.IsActive)
        {
            // user has changed his/her password/roles/stat/IsActive
            return false;
        }

        return true;
    }

    public async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
        var userId = GetUserId(claimsIdentity);

        if (!userId.HasValue || !await IsValidateUserAsync(claimsIdentity))
        {
            await HandleUnauthorizedRequestAsync(context);

            return;
        }

        await userProfilesManagerService.UpdateUserLastActivityDateAsync(userId.Value);
    }

    private static int? GetUserId(ClaimsIdentity? claimsIdentity)
    {
        var userIdString = claimsIdentity?.FindFirst(ClaimTypes.UserData)?.Value;

        return !string.IsNullOrWhiteSpace(userIdString) && int.TryParse(userIdString, NumberStyles.Number,
            CultureInfo.InvariantCulture, out var userId)
            ? userId
            : null;
    }

    private static Task HandleUnauthorizedRequestAsync(CookieValidatePrincipalContext context)
    {
        context.RejectPrincipal();

        return context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
