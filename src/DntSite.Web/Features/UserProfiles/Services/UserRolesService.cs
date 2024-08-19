using System.Security.Claims;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DntSite.Web.Features.UserProfiles.Services;

public class UserRolesService(
    IUnitOfWork uow,
    IEmailsFactoryService emailsFactoryService,
    IDeviceDetectionService deviceDetectionService) : IUserRolesService
{
    public const string DisplayNameClaim = "DisplayName";

    private readonly DbSet<Role> _roles = uow.DbSet<Role>();

    public async Task<ClaimsPrincipal> CreateCookieClaimsAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)));
        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        identity.AddClaim(new Claim(DisplayNameClaim, user.FriendlyName ?? ""));

        // to invalidate the cookie
        identity.AddClaim(new Claim(ClaimTypes.SerialNumber, user.SerialNumber ?? ""));

        identity.AddClaim(new Claim(ClaimTypes.System, deviceDetectionService.GetCurrentRequestDeviceDetailsHash(),
            ClaimValueTypes.String));

        // custom data
        identity.AddClaim(new Claim(ClaimTypes.UserData, user.Id.ToString(CultureInfo.InvariantCulture)));

        // add roles
        var roles = await FindUserRolesAsync(user.Id);

        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }

        return new ClaimsPrincipal(identity);
    }

    public Task<List<Role>> FindUserRolesAsync(int? userId)
    {
        if (!userId.HasValue)
        {
            return Task.FromResult(new List<Role>());
        }

        var query = from role in _roles
            from user in role.AssociatedEntities
            where user.Id == userId.Value
            select role;

        return query.OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<string[]> GetRolesForUserAsync(int? userId)
    {
        if (!userId.HasValue)
        {
            return [];
        }

        var roles = await FindUserRolesAsync(userId);

        return roles.Count == 0 ? [] : roles.Select(x => x.Name).ToArray();
    }

    public async Task<bool> IsUserInRoleAsync(int? userId, string roleName)
    {
        if (!userId.HasValue)
        {
            return false;
        }

        var query = from role in _roles.AsNoTracking()
            where role.Name == roleName
            from user in role.AssociatedEntities
            where user.Id == userId.Value
            select role;

        var userRole = await query.OrderBy(x => x.Id).FirstOrDefaultAsync();

        return userRole is not null;
    }

    public async Task AddOrUpdateUserRolesAsync(int userId, IList<string> inputRoleValues)
    {
        var user = await uow.DbSet<User>().Include(user => user.Roles).FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null)
        {
            return;
        }

        if (inputRoleValues is null || inputRoleValues.Count == 0)
        {
            user.Roles.Clear();
            await uow.SaveChangesAsync();

            return;
        }

        var currentUserRoleValues = user.Roles.Select(role => role.Name).ToList();
        var newRoleValuesToAdd = inputRoleValues.Except(currentUserRoleValues).ToList();

        var correspondingDbNewRolesToAdd = await _roles
            .Where(role => newRoleValuesToAdd.Contains(role.Name))
            .ToListAsync();

        correspondingDbNewRolesToAdd.ForEach(user.Roles.Add);

        var remainingNewRolesToAdd = newRoleValuesToAdd
            .Except(correspondingDbNewRolesToAdd.Select(role => role.Name))
            .ToList();

        remainingNewRolesToAdd.ForEach(value => user.Roles.Add(new Role
        {
            Name = value
        }));

        var removedRoleValues = currentUserRoleValues.Except(inputRoleValues).ToList();
        var rolesListToRemove = user.Roles.Where(role => removedRoleValues.Contains(role.Name)).ToList();
        rolesListToRemove.ForEach(role => user.Roles.Remove(role));

        await uow.SaveChangesAsync();

        await emailsFactoryService.SendTextToAllAdminsAsync($"{user.FriendlyName} به عنوان مدیریت سیستم ثبت شد. ");
    }
}
