using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface ICurrentUserService : IScopedService
{
    bool IsCurrentUserAuthenticated();

    int? GetCurrentUserId();

    Task<User?> GetCurrentImpersonatedUserAsync(int? impersonatedUserId);

    bool IsCurrentUserInRole(string roleName);

    Task<bool> IsCurrentUserInRoleAsync(string roleName);

    Task<bool> IsCurrentUserAdminAsync();

    bool IsCurrentUserAdmin();

    Task<CurrentUserModel> GetCurrentUserAsync();

    Task ClearExistingAuthenticationCookiesAsync(bool clearAdminCookies);

    Task<bool> CanCurrentUserRegisterAsync();
}
