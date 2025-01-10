using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUserProfilesManagerService : IScopedService
{
    public string UsersCantRegisterErrorMessage { get; }

    public Task DisableInactiveUsersAsync(int month);

    public Task NotifyInactiveUsersAsync(int month, CancellationToken cancellationToken);

    public Task ResetRegistrationCodeAsync(User user);

    public Task UpdateUserLastActivityDateAsync(int userId);

    public Task<User> AddUserAsync(RegisterModel model);

    public Task<OperationResult> ActivateEmailAsync(string name);

    public Task<int> GetGeneralAdvertisementUserIdAsync();

    public Task UpdateUserImageFromGravatarAsync(User user);

    public Task<OperationResult<(string Password, User? User)>> ResetPasswordAsync(string name);

    public Task<OperationResult> ChangeUserPasswordAsync(int? userId, string newPassword);

    public Task SendActivateYourAccountEmailAsync(int userId);

    public Task UserIsNotRestrictedAsync(int userId);

    public Task UserIsRestrictedAsync(int userId);

    public Task DisableUserAsync(int userId);

    public Task ActivateUserAsync(int userId);

    public Task<OperationResult> RegisterUserAsync(RegisterModel? model, bool canUsersRegister, int? currentUserId);

    public Task<OperationResult> ProcessForgottenPasswordAsync(ForgottenPasswordModel? model);

    public Task<OperationResult> EditUserSocialNetworksAsync(int? editUserId, UserSocialNetworkModel? model);

    public Task<OperationResult> EditUserProfileAsync(UserProfileModel? model, int? editUserId);
}
