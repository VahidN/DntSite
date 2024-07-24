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

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [RecalculatePostsCount];
}
