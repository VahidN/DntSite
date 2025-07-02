namespace DntSite.Web.Features.Stats.RoutingConstants;

public static class StatsRoutingConstants
{
    public const string RecalculatePostsCount = "/recalculate-posts-count";

    public const string TodayVisitedUsers = "/today-visited-users";
    public const string TodayVisitedUsersPageCurrentPage = $"{TodayVisitedUsers}/page/{{CurrentPage:int?}}";

    public const string SiteReferrersBase = "/site-referrers";
    public const string SiteReferrers = $"{SiteReferrersBase}/{{ReferrerType}}";
    public const string SiteReferrerPageCurrentPage = $"{SiteReferrers}/page/{{CurrentPage:int?}}";
    public const string SiteReferrersDeleteBase = $"{SiteReferrers}/delete";

    public const string SiteReferrersDeleteDeleteId =
        $"{SiteReferrersDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string OnlineVisitors = "/online-visitors";
    public const string OnlineVisitorsPageCurrentPage = $"{OnlineVisitors}/page/{{CurrentPage:int?}}";

    public const string OnlineVisitorsDeleteBase = $"{OnlineVisitors}/delete";

    public const string OnlineVisitorsDeleteDeleteId =
        $"{OnlineVisitorsDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string OnlineSpiderVisitors = $"{OnlineVisitors}/{{CategoryName}}";
    public const string OnlineSpiderVisitorsUrl = $"{OnlineVisitors}/spider";
    public const string OnlineSpiderVisitorsPageCurrentPage = $"{OnlineSpiderVisitors}/page/{{CurrentPage:int?}}";

    public const string MoreLocalPageReferrersBase = "/related-pages";
    public const string MoreLocalPageReferrers = $"{MoreLocalPageReferrersBase}/{{Url}}";

    public const string MoreLocalPageReferrersUrlPageCurrentPage =
        $"{MoreLocalPageReferrers}/page/{{CurrentPage:int?}}";

    public static readonly string[] IgnoresList =
    [
        "/api/", "/file/", "/feed", "/feeds", "/error", "/search-results/", "/whats-new", "/more-like-this/",
        "/delete", "/edit", "/filter/", "/page/", "/*/"
    ];
}
