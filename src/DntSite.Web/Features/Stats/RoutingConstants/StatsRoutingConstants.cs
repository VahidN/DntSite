namespace DntSite.Web.Features.Stats.RoutingConstants;

public static class StatsRoutingConstants
{
    public const string RecalculatePostsCount = "/recalculate-posts-count";

    public const string TodayVisitedUsers = "/today-visited-users";
    public const string TodayVisitedUsersPageCurrentPage = $"{TodayVisitedUsers}/page/{{CurrentPage:int}}";
}
