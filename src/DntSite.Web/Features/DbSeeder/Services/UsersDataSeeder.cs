using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.DbSeeder.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.DbSeeder.Services;

public class UsersDataSeeder(
    IUnitOfWork uow,
    IPasswordHasherService passwordHasherService,
    IOptionsSnapshot<StartupSettingsModel> siteSettingsRoot) : IDataSeeder
{
    private readonly IPasswordHasherService _passwordHasherService =
        passwordHasherService ?? throw new ArgumentNullException(nameof(passwordHasherService));

    private readonly DbSet<Role> _roles = uow.DbSet<Role>();

    private readonly StartupSettingsModel _startupSettings = siteSettingsRoot is null
        ? throw new ArgumentNullException(nameof(siteSettingsRoot))
        : siteSettingsRoot.Value;

    private readonly IUnitOfWork _uow = uow ?? throw new ArgumentNullException(nameof(uow));
    private readonly DbSet<User> _users = uow.DbSet<User>();
    private readonly DbSet<UserUsedPassword> _userUsedPasswords = uow.DbSet<UserUsedPassword>();

    public int Order { get; set; } = 2;

    public void SeedData()
    {
        if (_users.Any())
        {
            return;
        }

        // Add Admin user and its password history
        var adminUserOptions = _startupSettings.AdminUserSeed;

        var roles = _roles.ToList();

        var adminUser = new User
        {
            UserName = adminUserOptions.Username,
            FriendlyName = adminUserOptions.FriendlyName,
            IsActive = true,
            LastVisitDateTime = null,
            HashedPassword = _passwordHasherService.GetPbkdf2Hash(adminUserOptions.Password),
            SerialNumber = Guid.NewGuid().ToString("N"),
            EMail = adminUserOptions.Email,
            EmailIsValidated = true,
            Description = "-",
            RegistrationCode = Guid.NewGuid().ToString("N"),
            ReceiveDailyEmails = true,
            Roles = roles
        };

        _users.Add(adminUser);

        _userUsedPasswords.Add(new UserUsedPassword
        {
            User = adminUser,
            HashedPassword = adminUser.HashedPassword
        });

        _uow.SaveChanges();
    }
}
