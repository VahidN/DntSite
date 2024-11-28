using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface ICurrentUserService : IScopedService
{
    public Task<bool> IsCurrentUserSpiderAsync();

    public bool IsCurrentUserAuthenticated();

    public int? GetCurrentUserId();

    public Task<User?> GetCurrentImpersonatedUserAsync(int? impersonatedUserId);

    public bool IsCurrentUserInRole(string roleName);

    public Task<bool> IsCurrentUserInRoleAsync(string roleName);

    public Task<bool> IsCurrentUserAdminAsync();

    public bool IsCurrentUserAdmin();

    public Task<CurrentUserModel> GetCurrentUserAsync();

    public Task ClearExistingAuthenticationCookiesAsync(bool clearAdminCookies);

    public Task<bool> CanCurrentUserRegisterAsync();
}
