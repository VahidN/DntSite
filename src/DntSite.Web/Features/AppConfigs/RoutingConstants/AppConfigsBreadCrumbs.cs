using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.AppConfigs.RoutingConstants;

public static class AppConfigsBreadCrumbs
{
    public static readonly BreadCrumb RootBreadCrumb = new()
    {
        Title = "خانه",
        Url = "/",
        GlyphIcon = DntBootstrapIcons.BiHouse
    };

    public static readonly BreadCrumb ServerInfoBreadCrumb = new()
    {
        Title = "مشخصات سرور",
        Url = AppConfigsRoutingConstants.ServerInfo,
        GlyphIcon = DntBootstrapIcons.BiServer,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb SiteConfigBreadCrumb = new()
    {
        Title = "تنظیمات برنامه",
        Url = AppConfigsRoutingConstants.SiteConfig,
        GlyphIcon = DntBootstrapIcons.BiGear,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb SystemLogsBreadCrumb = new()
    {
        Title = "لاگ سیستم",
        Url = AppConfigsRoutingConstants.SystemLogs,
        GlyphIcon = DntBootstrapIcons.BiCone,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [];
}
