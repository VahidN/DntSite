using System.Security.Claims;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DntSite.Web.Features.UserProfiles.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    IUserRolesService rolesService,
    IUsersInfoService usersService,
    IAppSettingsService appSettingsService) : ICurrentUserService
{
    public int? GetCurrentUserId() => httpContextAccessor.HttpContext?.User.GetUserId();

    public Task<bool> IsCurrentUserInRoleAsync(string roleName)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return Task.FromResult(result: false);
        }

        if (!IsCurrentUserAuthenticated())
        {
            return Task.FromResult(result: false);
        }

        return httpContext.User.HasClaim(ClaimTypes.Role, roleName)
            ? Task.FromResult(result: true)
            : rolesService.IsUserInRoleAsync(GetCurrentUserId(), roleName);
    }

    public bool IsCurrentUserInRole(string roleName)
    {
        var httpContext = httpContextAccessor.HttpContext;

        return httpContext is not null && IsCurrentUserAuthenticated() &&
               httpContext.User.HasClaim(ClaimTypes.Role, roleName);
    }

    public bool IsCurrentUserAuthenticated() => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Task<bool> IsCurrentUserAdminAsync() => IsCurrentUserInRoleAsync(CustomRoles.Admin);

    public bool IsCurrentUserAdmin() => IsCurrentUserInRole(CustomRoles.Admin);

    public async Task<CurrentUserModel> GetCurrentUserAsync()
        => new()
        {
            User = await usersService.FindUserAsync(GetCurrentUserId()),
            UserId = GetCurrentUserId(),
            IsAuthenticated = IsCurrentUserAuthenticated(),
            IsAdmin = await IsCurrentUserAdminAsync()
        };

    public async Task<User?> GetCurrentImpersonatedUserAsync(int? impersonatedUserId)
    {
        if (impersonatedUserId is > 0)
        {
            return !await IsCurrentUserAdminAsync() ? null : await usersService.FindUserAsync(impersonatedUserId.Value);
        }

        return await usersService.FindUserAsync(GetCurrentUserId());
    }

    public async Task ClearExistingAuthenticationCookiesAsync(bool clearAdminCookies)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null || httpContext.IsGetRequest())
        {
            return;
        }

        var isAdmin = await IsCurrentUserAdminAsync();

        if (isAdmin && !clearAdminCookies)
        {
            return;
        }

        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<bool> CanCurrentUserRegisterAsync()
    {
        if (await IsCurrentUserAdminAsync())
        {
            return true;
        }

        var cfg = await appSettingsService.GetAppSettingsAsync();

        return cfg is { CanUsersRegister: true };
    }
}
