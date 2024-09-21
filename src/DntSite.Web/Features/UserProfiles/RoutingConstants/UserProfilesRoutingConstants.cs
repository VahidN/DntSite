namespace DntSite.Web.Features.UserProfiles.RoutingConstants;

public static class UserProfilesRoutingConstants
{
    public const string ActivationBase = "/Activation";
    public const string ActivationData = $"{ActivationBase}/{{Data}}";

    public const string ChangePassword = "/change-password";

    public const string ChangeUserPasswordBase = "/change-user-password";

    public const string ChangeUserPasswordUserId =
        $"{ChangeUserPasswordBase}/{{UserId:{EncryptedRouteConstraint.Name}}}";

    public const string EditProfile = "/edit-profile";
    public const string EditProfileEditUserId = $"{EditProfile}/{{EditUserId:{EncryptedRouteConstraint.Name}}}";

    public const string EditSocialNetworks = "/edit-social-networks";

    public const string EditSocialNetworksEditUserId =
        $"{EditSocialNetworks}/{{EditUserId:{EncryptedRouteConstraint.Name}}}";

    public const string ForgottenPassword = "/forgotten-password";
    public const string ForgottenPasswordResetBase = $"{ForgottenPassword}/reset";
    public const string ForgottenPasswordResetId = $"{ForgottenPasswordResetBase}/{{Id}}";

    public const string Login = "/login";

    public const string Logout = "/logout";

    public const string Register = "/register";

    public const string SendActivationEmails = "/send-activation-emails";

    public const string SendMassMail = "/send-mass-mail";

    public const string Users = "/users";
    public const string UsersPageCurrentPage = $"{Users}/page/{{CurrentPage:int}}";

    public const string OldUsers = "/siteusers";
    public const string OldUsersPageCurrentPage = $"{OldUsers}/index/{{CurrentPage:int}}";

    public const string UsersFilterBase = $"{Users}/filter";
    public const string UsersFilterFilterPageCurrentPage = $"{UsersFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string UsersName = $"{Users}/{{Name}}";

    public const string User = "/user";
    public const string UserPageCurrentPage = $"{User}/page/{{CurrentPage:int}}";

    public const string UserFilterBase = $"{User}/filter";
    public const string UserFilterFilterPageCurrentPage = $"{UserFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string UserName = $"{User}/{{Name}}";

    public const string UsersBirthdays = "/birthdays";

    public const string UsersManager = "/users-manager";

    public const string UsersManagerFilterBase = $"{UsersManager}/filter";

    public const string UsersManagerFilterFilterPageCurrentPage =
        $"{UsersManagerFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string JobSeekers = "/job-seekers";
}
