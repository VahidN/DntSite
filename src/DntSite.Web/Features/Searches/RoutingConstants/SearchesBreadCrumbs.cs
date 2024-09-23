using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Searches.RoutingConstants;

public static class SearchesBreadCrumbs
{
    public static readonly BreadCrumb SearchedItems = new()
    {
        Title = "آمار جستجوها",
        Url = SearchesRoutingConstants.SearchedItems,
        GlyphIcon = DntBootstrapIcons.BiSearch,
        AllowAnonymous = false,
        AllowedRoles = CustomRoles.Admin
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [SearchedItems];

    public static BreadCrumb GetSearchResults(string title, string url)
        => new()
        {
            Title = title,
            Url = url,
            GlyphIcon = DntBootstrapIcons.BiSearch
        };
}
