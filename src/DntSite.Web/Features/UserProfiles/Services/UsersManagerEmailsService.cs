using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.UserProfiles.EmailLayouts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Models;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.UserProfiles.Services;

public class UsersManagerEmailsService(
    ICommonService commonService,
    IEmailsFactoryService emailsFactoryService,
    IPasswordHasherService passwordHasherService,
    IProtectionProviderService protectionProviderService) : IUsersManagerEmailsService
{
    public async Task ResetNotActivatedUsersAndSendEmailAsync(DateTime? from)
    {
        var users = await commonService.NotValidatedEmailsUsersAsync(from);

        foreach (var user in users)
        {
            await SendActivateYourAccountEmailAsync(user);
        }
    }

    public Task SendNotifyInActiveUserAsync(User user, string url, string siteName)
    {
        ArgumentNullException.ThrowIfNull(user);

        return emailsFactoryService.SendEmailAsync<WillBeDisabled, WillBeDisabledModel>("NotifyInActiveUser", "",
            "NotifyInActiveUser", new WillBeDisabledModel
            {
                BaseUrl = url,
                SiteName = siteName
            }, user.EMail, "جهت اطلاع", false);
    }

    public Task SendUserIsDisabledAsync(User user, string url, string siteName)
    {
        ArgumentNullException.ThrowIfNull(user);

        return emailsFactoryService.SendEmailAsync<Disabled, DisabledModel>("UserIsDisabled", "", "UserIsDisabled",
            new DisabledModel
            {
                BaseUrl = url,
                SiteName = siteName
            }, user.EMail, "جهت اطلاع", false);
    }

    public Task SendActivateYourAccountEmailAsync(User userInfo)
    {
        ArgumentNullException.ThrowIfNull(userInfo);

        var salt = Invariant($"{userInfo.RegistrationCode}-{userInfo.Id}");
        var queryString = protectionProviderService.Encrypt(salt);

        return emailsFactoryService.SendEmailAsync<ActivationEmail, ActivationEmailModel>(
            Invariant($"ActivateYourAccount/{DateTime.UtcNow.Ticks}"), "", "", new ActivationEmailModel
            {
                Username = userInfo.UserName,
                FriendlyName = userInfo.FriendlyName,
                QueryString = queryString
            }, userInfo.EMail, "لطفا اکانت خود را فعال نمائید", false);
    }

    public Task SendForgottenPasswordConfirmEmailAsync(User userInfo)
    {
        ArgumentNullException.ThrowIfNull(userInfo);

        var queryString = protectionProviderService.Encrypt(Invariant($"{userInfo.RegistrationCode}-{userInfo.Id}"));

        return emailsFactoryService.SendEmailAsync<ForgottenPasswordConfirmEmail, ForgottenPasswordConfirmEmailModel>(
            Invariant($"ForgottenPasswordConfirm/{DateTime.UtcNow.Ticks}"), "", "",
            new ForgottenPasswordConfirmEmailModel
            {
                QueryString = queryString
            }, userInfo.EMail, "تائید بازیابی کلمه عبور", false);
    }

    public Task SendResetPasswordEmailAsync(User userInfo, string originalPassword)
    {
        ArgumentNullException.ThrowIfNull(userInfo);

        return emailsFactoryService.SendEmailAsync<ResetPasswordEmail, ResetPasswordEmailModel>(
            Invariant($"ResetPassword/{DateTime.UtcNow.Ticks}"), "", "", new ResetPasswordEmailModel
            {
                Username = userInfo.UserName,
                FriendlyName = userInfo.FriendlyName,
                OriginalPassword = originalPassword
            }, userInfo.EMail, "بازیابی کلمه عبور", false);
    }

    public Task UserProfileEditedEmailToAdminAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var nameHash = passwordHasherService.GetSha256Hash(user.UserName);

        return emailsFactoryService.SendEmailToAllAdminsAsync<UserProfileToAdmin, UserProfileToAdminModel>(
            Invariant($"UserProfile/{nameHash}"), "", Invariant($"UserProfile/{nameHash}"), new UserProfileToAdminModel
            {
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
                Username = user.UserName,
                FriendlyName = user.FriendlyName,
                HomePageUrl = user.HomePageUrl ?? "",
                UserEmail = user.EMail,
                Photo = user.Photo ?? "",
                Description = user.Description ?? ""
            }, "حساب کاربری");
    }

    public Task UserProfileSendEmailAsync(UserProfileEmailModel userProfile, User user)
    {
        ArgumentNullException.ThrowIfNull(userProfile);
        ArgumentNullException.ThrowIfNull(user);

        var nameHash = passwordHasherService.GetSha256Hash(user.UserName);

        return emailsFactoryService.SendEmailAsync<UserProfileEmail, UserProfileEmailModel>(
            Invariant($"UserProfile/{nameHash}"), "", Invariant($"UserProfile/{nameHash}"), new UserProfileEmailModel
            {
                Username = user.UserName,
                FriendlyName = user.FriendlyName,
                OriginalPassword = userProfile.OriginalPassword
            }, user.EMail, "تغییر مشخصات حساب کاربری", false);
    }

    public Task UsersManagerSendEmailAsync(string userName, string friendlyName, string message)
    {
        var nameHash = passwordHasherService.GetSha256Hash(userName);

        return emailsFactoryService.SendEmailToAllAdminsAsync<UsersManagerEmail, UsersManagerEmailModel>(
            Invariant($"UserProfile/{nameHash}"), "", Invariant($"UserProfile/{nameHash}"), new UsersManagerEmailModel
            {
                Username = userName,
                FriendlyName = friendlyName,
                Operation = message
            }, "تغییر مشخصات حساب کاربری توسط مدیر");
    }
}
