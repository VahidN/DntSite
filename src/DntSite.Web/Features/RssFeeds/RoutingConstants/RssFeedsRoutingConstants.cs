namespace DntSite.Web.Features.RssFeeds.RoutingConstants;

public static class RssFeedsRoutingConstants
{
    public const string Root = "/";

    public const string WhatsNew = "/whats-new";
    public const string WhatsNewPageCurrentPage = $"{WhatsNew}/page/{{CurrentPage:int?}}";

    public const string SiteLogs = "/SiteLogs";
    public const string SiteLogsPageCurrentPage = $"{SiteLogs}/index/{{CurrentPage:int?}}";
}
