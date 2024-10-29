using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.Searches.RoutingConstants;
using DntSite.Web.Features.Stats.Models;

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

    public static readonly BreadCrumb SiteExternalReferrers = new()
    {
        Title = "ارجاعات خارجی",
        Url = $"{StatsRoutingConstants.SiteReferrersBase}/{nameof(SiteReferrerType.External)}",
        GlyphIcon = DntBootstrapIcons.BiSignpost
    };

    public static readonly BreadCrumb SiteInternalReferrers = new()
    {
        Title = "ارجاعات داخلی",
        Url = $"{StatsRoutingConstants.SiteReferrersBase}/{nameof(SiteReferrerType.Internal)}",
        GlyphIcon = DntBootstrapIcons.BiSignpost2
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

    public static readonly IList<BreadCrumb> SiteStatsBreadCrumbs =
    [
        OnlineVisitors, OnlineSpiders, SiteExternalReferrers, SiteInternalReferrers,
        SearchesBreadCrumbs.SearchedItems
    ];
}
