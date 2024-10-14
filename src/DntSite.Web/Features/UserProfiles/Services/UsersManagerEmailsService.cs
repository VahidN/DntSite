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

        return emailsFactoryService.SendEmailAsync<WillBeDisabled, WillBeDisabledModel>(messageId: "NotifyInActiveUser",
            inReplyTo: "", references: "NotifyInActiveUser", new WillBeDisabledModel
            {
                BaseUrl = url,
                SiteName = siteName
            }, user.EMail, emailSubject: "جهت اطلاع", addIp: false);
    }

    public Task SendUserIsDisabledAsync(User user, string url, string siteName)
    {
        ArgumentNullException.ThrowIfNull(user);

        return emailsFactoryService.SendEmailAsync<Disabled, DisabledModel>(messageId: "UserIsDisabled", inReplyTo: "",
            references: "UserIsDisabled", new DisabledModel
            {
                BaseUrl = url,
                SiteName = siteName
            }, user.EMail, emailSubject: "جهت اطلاع", addIp: false);
    }

    public Task SendActivateYourAccountEmailAsync(User userInfo)
    {
        ArgumentNullException.ThrowIfNull(userInfo);

        var salt = string.Create(CultureInfo.InvariantCulture, $"{userInfo.RegistrationCode}-{userInfo.Id}");
        var queryString = protectionProviderService.Encrypt(salt);

        if (queryString.IsEmpty())
        {
            throw new InvalidOperationException(message: "queryString is null");
        }

        return emailsFactoryService.SendEmailAsync<ActivationEmail, ActivationEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"ActivateYourAccount/{DateTime.UtcNow.Ticks}"), inReplyTo: "",
            references: "", new ActivationEmailModel
            {
                Username = userInfo.UserName,
                FriendlyName = userInfo.FriendlyName,
                QueryString = queryString
            }, userInfo.EMail, emailSubject: "لطفا اکانت خود را فعال نمائید", addIp: false);
    }

    public Task SendForgottenPasswordConfirmEmailAsync(User userInfo)
    {
        ArgumentNullException.ThrowIfNull(userInfo);

        var queryString = protectionProviderService.Encrypt(string.Create(CultureInfo.InvariantCulture,
            $"{userInfo.RegistrationCode}-{userInfo.Id}"));

        if (queryString.IsEmpty())
        {
            throw new InvalidOperationException(message: "queryString is null");
        }

        return emailsFactoryService.SendEmailAsync<ForgottenPasswordConfirmEmail, ForgottenPasswordConfirmEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"ForgottenPasswordConfirm/{DateTime.UtcNow.Ticks}"),
            inReplyTo: "", references: "", new ForgottenPasswordConfirmEmailModel
            {
                QueryString = queryString
            }, userInfo.EMail, emailSubject: "تائید بازیابی کلمه عبور", addIp: false);
    }

    public Task SendResetPasswordEmailAsync(User userInfo, string originalPassword)
    {
        ArgumentNullException.ThrowIfNull(userInfo);

        return emailsFactoryService.SendEmailAsync<ResetPasswordEmail, ResetPasswordEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"ResetPassword/{DateTime.UtcNow.Ticks}"), inReplyTo: "",
            references: "", new ResetPasswordEmailModel
            {
                Username = userInfo.UserName,
                FriendlyName = userInfo.FriendlyName,
                OriginalPassword = originalPassword
            }, userInfo.EMail, emailSubject: "بازیابی کلمه عبور", addIp: false);
    }

    public Task UserProfileEditedEmailToAdminAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var nameHash = passwordHasherService.GetSha256Hash(user.UserName);

        return emailsFactoryService.SendEmailToAllAdminsAsync<UserProfileToAdmin, UserProfileToAdminModel>(
            string.Create(CultureInfo.InvariantCulture, $"UserProfile/{nameHash}"), inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"UserProfile/{nameHash}"), new UserProfileToAdminModel
            {
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
                Username = user.UserName,
                FriendlyName = user.FriendlyName,
                HomePageUrl = user.HomePageUrl ?? "",
                UserEmail = user.EMail,
                Photo = user.Photo ?? "",
                Description = user.Description ?? ""
            }, emailSubject: "حساب کاربری");
    }

    public Task UserProfileSendEmailAsync(UserProfileEmailModel userProfile, User user)
    {
        ArgumentNullException.ThrowIfNull(userProfile);
        ArgumentNullException.ThrowIfNull(user);

        var nameHash = passwordHasherService.GetSha256Hash(user.UserName);

        return emailsFactoryService.SendEmailAsync<UserProfileEmail, UserProfileEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"UserProfile/{nameHash}"), inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"UserProfile/{nameHash}"), new UserProfileEmailModel
            {
                Username = user.UserName,
                FriendlyName = user.FriendlyName,
                OriginalPassword = userProfile.OriginalPassword
            }, user.EMail, emailSubject: "تغییر مشخصات حساب کاربری", addIp: false);
    }

    public Task UsersManagerSendEmailAsync(string userName, string friendlyName, string message)
    {
        var nameHash = passwordHasherService.GetSha256Hash(userName);

        return emailsFactoryService.SendEmailToAllAdminsAsync<UsersManagerEmail, UsersManagerEmailModel>(
            string.Create(CultureInfo.InvariantCulture, $"UserProfile/{nameHash}"), inReplyTo: "",
            string.Create(CultureInfo.InvariantCulture, $"UserProfile/{nameHash}"), new UsersManagerEmailModel
            {
                Username = userName,
                FriendlyName = friendlyName,
                Operation = message
            }, emailSubject: "تغییر مشخصات حساب کاربری توسط مدیر");
    }
}
