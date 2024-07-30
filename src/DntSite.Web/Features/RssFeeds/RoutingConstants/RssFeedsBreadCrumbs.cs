using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.Common.RoutingConstants;

namespace DntSite.Web.Features.RssFeeds.RoutingConstants;

public static class RssFeedsBreadCrumbs
{
    public static readonly BreadCrumb WhatsNew = new()
    {
        Title = "تازه چه‌خبر",
        Url = RssFeedsRoutingConstants.WhatsNew,
        GlyphIcon = DntBootstrapIcons.BiNewspaper
    };

    public static readonly BreadCrumb WhatsNewFeed = new()
    {
        Title = "فید آخرین تغییرات",
        Url = ApiUrlsRoutingConstants.Feed.HttpAny.Index,
        GlyphIcon = DntBootstrapIcons.BiRss
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [WhatsNewFeed, WhatsNew];
}
