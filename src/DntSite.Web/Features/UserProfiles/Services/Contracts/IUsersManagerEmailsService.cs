using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.UserProfiles.Services.Contracts;

public interface IUsersManagerEmailsService : IScopedService
{
    public Task ResetNotActivatedUsersAndSendEmailAsync(DateTime? from, CancellationToken cancellationToken = default);

    public Task SendUserActivatedEmailAsync(string userName, string email);

    public Task SendActivateYourAccountEmailAsync(User userInfo);

    public Task SendForgottenPasswordConfirmEmailAsync(User userInfo);

    public Task SendResetPasswordEmailAsync(User userInfo, string originalPassword);

    public Task UserProfileSendEmailAsync(UserProfileEmailModel userProfile, User user);

    public Task UserProfileEditedEmailToAdminAsync(User user);

    public Task UsersManagerSendEmailAsync(string userName, string friendlyName, string message);

    public Task SendUserIsDisabledAsync(string email);

    public Task SendNotifyInActiveUserAsync(string email);
}
