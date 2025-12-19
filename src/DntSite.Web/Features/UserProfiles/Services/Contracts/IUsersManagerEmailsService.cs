using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUsersManagerEmailsService : IScopedService
{
    Task ResetNotActivatedUsersAndSendEmailAsync(DateTime? from, CancellationToken cancellationToken = default);

    Task SendUserActivatedEmailAsync(string userName, string email);

    Task SendActivateYourAccountEmailAsync(User userInfo);

    Task SendForgottenPasswordConfirmEmailAsync(User userInfo);

    Task SendResetPasswordEmailAsync(User userInfo, string originalPassword);

    Task UserProfileSendEmailAsync(UserProfileEmailModel userProfile, User user);

    Task UserProfileEditedEmailToAdminAsync(User user);

    Task UsersManagerSendEmailAsync(string userName, string friendlyName, string message);

    Task SendUserIsDisabledAsync(string email);

    Task SendNotifyInActiveUserAsync(string email);
}
