using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUsersManagerEmailsService : IScopedService
{
    Task ResetNotActivatedUsersAndSendEmailAsync(DateTime? from);

    Task SendActivateYourAccountEmailAsync(User userInfo);

    Task SendForgottenPasswordConfirmEmailAsync(User userInfo);

    Task SendResetPasswordEmailAsync(User userInfo, string originalPassword);

    Task UserProfileSendEmailAsync(UserProfileEmailModel userProfile, User user);

    Task UserProfileEditedEmailToAdminAsync(User user);

    Task UsersManagerSendEmailAsync(string userName, string friendlyName, string message);

    Task SendUserIsDisabledAsync(User user, string url, string siteName);

    Task SendNotifyInActiveUserAsync(User user, string url, string siteName);
}
