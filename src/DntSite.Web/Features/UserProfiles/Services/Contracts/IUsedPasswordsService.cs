namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUsedPasswordsService : IScopedService
{
    Task<bool> IsPreviouslyUsedPasswordAsync(int? userId, string newPassword);

    Task AddToUsedPasswordsListAsync(int userId, string hashedPassword);

    Task<bool> IsLastUserPasswordTooOldAsync(int userId);

    Task<DateTime?> GetLastUserPasswordChangeDateAsync(int? userId);
}
