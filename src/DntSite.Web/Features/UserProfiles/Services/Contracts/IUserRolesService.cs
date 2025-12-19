using System.Security.Claims;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUserRolesService : IScopedService
{
    Task<ClaimsPrincipal> CreateCookieClaimsAsync(User user);

    Task<List<Role>> FindUserRolesAsync(int? userId);

    Task<bool> IsUserInRoleAsync(int? userId, string roleName);

    Task<string[]> GetRolesForUserAsync(int? userId);

    Task AddOrUpdateUserRolesAsync(int userId, IList<string> inputRoleValues);
}
