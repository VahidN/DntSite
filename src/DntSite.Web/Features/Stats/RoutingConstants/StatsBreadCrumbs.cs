using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.UserProfiles.RoutingConstants;

namespace DntSite.Web.Features.Stats.RoutingConstants;

public static class StatsBreadCrumbs
{
    public static readonly BreadCrumb RecalculatePostsCount = new()
    {
        Title = "بازشماری آمار",
        Url = StatsRoutingConstants.RecalculatePostsCount,
        GlyphIcon = DntBootstrapIcons.BiRepeat,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb TodayVisitedUsers = new()
    {
        Title = "مراجعان امروز",
        Url = StatsRoutingConstants.TodayVisitedUsers,
        GlyphIcon = DntBootstrapIcons.BiCalendar2Range
    };

    public static readonly BreadCrumb SiteReferrers = new()
    {
        Title = "ارجاع دهنده‌ها",
        Url = StatsRoutingConstants.SiteReferrers,
        GlyphIcon = DntBootstrapIcons.BiSignpost
    };

    public static readonly BreadCrumb OnlineVisitors = new()
    {
        Title = "کاربران آنلاین",
        Url = StatsRoutingConstants.OnlineVisitors,
        GlyphIcon = DntBootstrapIcons.BiPeopleFill
    };

    public static readonly BreadCrumb OnlineSpiders = new()
    {
        Title = "خزنده‌های آنلاین",
        Url = StatsRoutingConstants.OnlineSpiderVisitorsUrl,
        GlyphIcon = DntBootstrapIcons.BiBug
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [RecalculatePostsCount];

    public static readonly IList<BreadCrumb> OnlineUsersStatsBreadCrumbs =
    [
        UserProfilesBreadCrumbs.UsersBirthdays, TodayVisitedUsers, OnlineVisitors, OnlineSpiders, SiteReferrers,
        UserProfilesBreadCrumbs.Users
    ];
}
