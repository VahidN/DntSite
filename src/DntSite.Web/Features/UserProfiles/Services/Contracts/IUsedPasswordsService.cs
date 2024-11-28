namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUsedPasswordsService : IScopedService
{
    public Task<bool> IsPreviouslyUsedPasswordAsync(int? userId, string newPassword);

    public Task AddToUsedPasswordsListAsync(int userId, string hashedPassword);

    public Task<bool> IsLastUserPasswordTooOldAsync(int userId);

    public Task<DateTime?> GetLastUserPasswordChangeDateAsync(int? userId);
}
