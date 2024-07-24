using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.SideBar.RoutingConstants;

public static class SideBarBreadCrumbs
{
    public static readonly BreadCrumb CustomSideBar = new()
    {
        Title = "طراحی منوی سفارشی کنار صفحه",
        Url = SideBarRoutingConstants.CustomSideBar,
        GlyphIcon = DntBootstrapIcons.BiWindowSidebar,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [CustomSideBar];
}
