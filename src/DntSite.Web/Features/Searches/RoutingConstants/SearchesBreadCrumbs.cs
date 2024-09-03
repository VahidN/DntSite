using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Searches.RoutingConstants;

public static class SearchesBreadCrumbs
{
    public static readonly BreadCrumb SearchedItems = new()
    {
        Title = "آمار جستجوها",
        Url = SearchesRoutingConstants.SearchedItems,
        GlyphIcon = DntBootstrapIcons.BiSearch
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [SearchedItems];
}
