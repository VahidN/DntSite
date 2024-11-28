using System.Security.Claims;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUserRolesService : IScopedService
{
    public Task<ClaimsPrincipal> CreateCookieClaimsAsync(User user);

    public Task<List<Role>> FindUserRolesAsync(int? userId);

    public Task<bool> IsUserInRoleAsync(int? userId, string roleName);

    public Task<string[]> GetRolesForUserAsync(int? userId);

    public Task AddOrUpdateUserRolesAsync(int userId, IList<string> inputRoleValues);
}
