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

    public static BreadCrumb GetBreadCrumb(string? title, string? url, string glyphIcon = DntBootstrapIcons.BiSearch)
        => new()
        {
            Title = title ?? "نتایج جستجو",
            Url = url ?? "/",
            GlyphIcon = glyphIcon
        };
}
