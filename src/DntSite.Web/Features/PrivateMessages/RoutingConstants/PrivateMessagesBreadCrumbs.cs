using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.PrivateMessages.RoutingConstants;

public static class PrivateMessagesBreadCrumbs
{
    public static readonly BreadCrumb Login = new()
    {
        Title = "ورود",
        Url = UserProfilesRoutingConstants.Login,
        GlyphIcon = DntBootstrapIcons.BiDoorOpen
    };

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

    public static readonly BreadCrumb PrivateMessagesViewer = new()
    {
        Title = "کنترل پیام\u200cهای خصوصی سیستم",
        Url = PrivateMessagesRoutingConstants.PrivateMessagesViewer,
        GlyphIcon = DntBootstrapIcons.BiSnapchat,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb MyPrivateMessages = new()
    {
        Title = "پیام‌های خصوصی من",
        Url = PrivateMessagesRoutingConstants.MyPrivateMessages,
        GlyphIcon = DntBootstrapIcons.BiInbox,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [Users, Login];
}
