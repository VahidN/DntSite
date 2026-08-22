using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Bookmarks.RoutingConstants;

public static class BookmarksBreadCrumbs
{
    public static readonly BreadCrumb MyBookmarks = new()
    {
        Title = "علاقمندی‌های من",
        Url = BookmarksRoutingConstants.MyBookmarks,
        GlyphIcon = DntBootstrapIcons.BiHeart,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [MyBookmarks];
}
