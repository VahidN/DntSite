namespace DntSite.Web.Features.Bookmarks.RoutingConstants;

public static class BookmarksRoutingConstants
{
    public const string MyBookmarks = "/my-bookmarks";
    public const string MyBookmarksPageCurrentPage = $"{MyBookmarks}/page/{{CurrentPage:int?}}";
}
