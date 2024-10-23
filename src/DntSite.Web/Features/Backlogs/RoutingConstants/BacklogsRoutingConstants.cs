namespace DntSite.Web.Features.Backlogs.RoutingConstants;

public static class BacklogsRoutingConstants
{
    public const string PostsBase = "/post";
    public const string Backlogs = "/backlogs";
    public const string BacklogsPageCurrentPage = "/backlogs/page/{CurrentPage:int?}";
    public const string BacklogsFilterBase = "/backlogs/filter";

    public const string BacklogsFilterFilterPageCurrentPage =
        $"{BacklogsFilterBase}/{{Filter}}/page/{{CurrentPage:int?}}";

    public const string BacklogsDetailsBase = "/backlogs/details";
    public const string BacklogsDetailsBacklogId = $"{BacklogsDetailsBase}/{{BacklogId:int}}";
    public const string BacklogsTag = "/backlogs-tag";
    public const string BacklogsTagPageCurrentPage = "/backlogs-tag/page/{CurrentPage:int?}";
    public const string BacklogsTagTagName = "/backlogs-tag/{TagName}";
    public const string BacklogsTagTagNamePageCurrentPage = "/backlogs-tag/{TagName}/page/{CurrentPage:int?}";
    public const string BacklogsWriters = "/backlogs-writers";
    public const string BacklogsWritersPageCurrentPage = "/backlogs-writers/page/{CurrentPage:int?}";
    public const string BacklogsWritersUserFriendlyName = "/backlogs-writers/{UserFriendlyName}";

    public const string BacklogsWritersUserFriendlyNamePageCurrentPage =
        "/backlogs-writers/{UserFriendlyName}/page/{CurrentPage:int?}";

    public const string WriteBacklog = "/write-backlog";
    public const string WriteBacklogEditEditId = $"/write-backlog/edit/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteBacklogDeleteDeleteId =
        $"/write-backlog/delete/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string CommentsUrlTemplate = $"{BacklogsDetailsBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{BacklogsDetailsBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{BacklogsTag}/{{0}}";
    public const string EditPostUrlTemplate = $"{WriteBacklog}/edit/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteBacklog}/delete/{{0}}";
}
