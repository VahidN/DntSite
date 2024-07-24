using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Persistence.UnitOfWork;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Services;

public class UsedPasswordsService(
    IUnitOfWork uow,
    IAppSettingsService appSettingsService,
    IPasswordHasherService passwordHasherService) : IUsedPasswordsService
{
    private const int DefaultChangePasswordReminderDays = 90;
    private const int DefaultNotAllowedPreviouslyUsedPasswords = 5;

    private readonly DbSet<UserUsedPassword> _userUsedPasswords = uow.DbSet<UserUsedPassword>();

    public async Task AddToUsedPasswordsListAsync(int userId, string hashedPassword)
    {
        await _userUsedPasswords.AddAsync(new UserUsedPassword
        {
            UserId = userId,
            HashedPassword = hashedPassword
        });

        await uow.SaveChangesAsync();
    }

    public async Task<DateTime?> GetLastUserPasswordChangeDateAsync(int? userId)
    {
        if (userId is null)
        {
            return null;
        }

        var lastPasswordHistory = await _userUsedPasswords.AsNoTracking()
            .OrderByDescending(userUsedPassword => userUsedPassword.Id)
            .FirstOrDefaultAsync(userUsedPassword => userUsedPassword.UserId == userId.Value);

        return lastPasswordHistory?.Audit.CreatedAt;
    }

    public async Task<bool> IsLastUserPasswordTooOldAsync(int userId)
    {
        var changePasswordReminderDays =
            (await appSettingsService.GetAppSettingsAsync())?.UsedPasswords.ChangePasswordReminderDays;

        if (changePasswordReminderDays is null or 0)
        {
            changePasswordReminderDays = DefaultChangePasswordReminderDays;
        }

        var createdDateTime = await GetLastUserPasswordChangeDateAsync(userId);

        return createdDateTime != null &&
               createdDateTime.Value.AddDays(changePasswordReminderDays.Value) < DateTime.UtcNow;
    }

    /// <summary>
    ///     This method will be used by CustomPasswordValidator automatically,
    ///     every time a user wants to change his/her info.
    /// </summary>
    public async Task<bool> IsPreviouslyUsedPasswordAsync(int? userId, string newPassword)
    {
        if (!userId.HasValue)
        {
            return false;
        }

        var notAllowedPreviouslyUsedPasswords = (await appSettingsService.GetAppSettingsAsync())?.UsedPasswords
            .NotAllowedPreviouslyUsedPasswords;

        if (notAllowedPreviouslyUsedPasswords is null or 0)
        {
            notAllowedPreviouslyUsedPasswords = DefaultNotAllowedPreviouslyUsedPasswords;
        }

        var lastUsedHashedPasswords = await _userUsedPasswords.AsNoTracking()
            .Where(userUsedPassword => userUsedPassword.UserId == userId.Value)
            .OrderByDescending(userUsedPassword => userUsedPassword.Id)
            .Select(userUsedPassword => userUsedPassword.HashedPassword)
            .Take(notAllowedPreviouslyUsedPasswords.Value)
            .ToListAsync();

        return lastUsedHashedPasswords.Any(hashedPassword
            => passwordHasherService.IsValidPbkdf2Hash(hashedPassword, newPassword));
    }
}
