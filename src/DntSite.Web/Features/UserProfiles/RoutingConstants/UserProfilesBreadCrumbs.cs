using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.UserProfiles.RoutingConstants;

public static class UserProfilesBreadCrumbs
{
    public static readonly BreadCrumb Users = new()
    {
        Title = "کاربران",
        Url = UserProfilesRoutingConstants.Users,
        GlyphIcon = DntBootstrapIcons.BiPeople
    };

    public static readonly BreadCrumb UsersManager = new()
    {
        Title = "مدیریت کاربران",
        Url = UserProfilesRoutingConstants.UsersManager,
        GlyphIcon = DntBootstrapIcons.BiPeople,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb UsersBirthdays = new()
    {
        Title = "تولدهای امروز",
        Url = UserProfilesRoutingConstants.UsersBirthdays,
        GlyphIcon = DntBootstrapIcons.BiCake2
    };

    public static readonly BreadCrumb SendMassMail = new()
    {
        Title = "ارسال ایمیل به کاربران",
        Url = UserProfilesRoutingConstants.SendMassMail,
        GlyphIcon = DntBootstrapIcons.BiWechat,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb SendActivationEmails = new()
    {
        Title = "ارسال مجدد ایمیل‌های فعال سازی کاربران",
        Url = UserProfilesRoutingConstants.SendActivationEmails,
        GlyphIcon = DntBootstrapIcons.BiVolumeUp,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb Login = new()
    {
        Title = "ورود",
        Url = UserProfilesRoutingConstants.Login,
        GlyphIcon = DntBootstrapIcons.BiDoorOpen
    };

    public static readonly BreadCrumb JobSeekers = new()
    {
        Title = "جویندگان کار",
        Url = UserProfilesRoutingConstants.JobSeekers,
        GlyphIcon = DntBootstrapIcons.BiPersonWorkspace
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [];
}
