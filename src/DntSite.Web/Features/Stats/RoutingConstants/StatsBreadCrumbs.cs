using DntSite.Web.Common.BlazorSsr.Utils;

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

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [RecalculatePostsCount];
}
