using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUserProfilesManagerService : IScopedService
{
    string UsersCantRegisterErrorMessage { get; }

    Task DisableInactiveUsersAsync(int month);

    Task ResetRegistrationCodeAsync(User user);

    Task UpdateUserLastActivityDateAsync(int userId);

    Task<User> AddUserAsync(RegisterModel model);

    Task<OperationResult> ActivateEmailAsync(string name);

    Task<int> GetGeneralAdvertisementUserIdAsync();

    Task UpdateUserImageFromGravatarAsync(User user);

    Task<OperationResult<(string Password, User? User)>> ResetPasswordAsync(string name);

    Task<OperationResult> ChangeUserPasswordAsync(int? userId, string newPassword);

    Task SendActivateYourAccountEmailAsync(int userId);

    Task UserIsNotRestrictedAsync(int userId);

    Task UserIsRestrictedAsync(int userId);

    Task DisableUserAsync(int userId);

    Task ActivateUserAsync(int userId);

    Task<OperationResult> RegisterUserAsync(RegisterModel? model, bool canUsersRegister, int? currentUserId);

    Task<OperationResult> ProcessForgottenPasswordAsync(ForgottenPasswordModel? model);

    Task<OperationResult> EditUserSocialNetworksAsync(int? editUserId, UserSocialNetworkModel? model);

    Task<OperationResult> EditUserProfileAsync(UserProfileModel? model, int? editUserId);
}
