namespace DntSite.Web.Features.Posts.RoutingConstants;

public static class PostsRoutingConstants
{
    public const string ComingSoon1 = "/ComingSoon";
    public const string ComingSoon2 = "/coming-soon";

    public const string MyDrafts = "/my-drafts";

    public const string PostBase = "/post";
    public const string PostId = $"{PostBase}/{{Id:int}}";

    public const string PostIdSlug = $"{PostBase}/{{Id:int}}/{{Slug}}";
    public const string Root = "/";
    public const string PageCurrentPage = "/page/{CurrentPage:int}";
    public const string Posts = "/posts";
    public const string PostsPageCurrentPage = "/posts/page/{CurrentPage:int}";

    public const string PostsFilterFilterBase = "/posts/filter";

    public const string PostsFilterFilterPageCurrentPage =
        $"{PostsFilterFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string PostsWriters = "/posts-writers";
    public const string PostsWritersPageCurrentPage = "/posts-writers/page/{CurrentPage:int}";
    public const string PostsWritersUserFriendlyName = "/posts-writers/{UserFriendlyName}";

    public const string PostsWritersUserFriendlyNamePageCurrentPage =
        "/posts-writers/{UserFriendlyName}/page/{CurrentPage:int}";

    public const string AllDraftsList = "/all-drafts-list";
    public const string PostsComments = "/posts-comments";
    public const string PostsCommentsPageCurrentPage = "/posts-comments/page/{CurrentPage:int}";
    public const string PostsCommentsUserFriendlyName = "/posts-comments/{UserFriendlyName}";

    public const string PostsCommentsUserFriendlyNamePageCurrentPage =
        "/posts-comments/{UserFriendlyName}/page/{CurrentPage:int}";

    public const string ShowDraftBase = "/show-draft";
    public const string ShowDraftShowId = $"{ShowDraftBase}/{{ShowId:int}}";

    public const string Tag = "/tag";
    public const string TagPageCurrentPage = "/tag/page/{CurrentPage:int}";
    public const string TagTagName = "/tag/{TagName}";
    public const string TagTagNamePageCurrentPage = "/tag/{TagName}/page/{CurrentPage:int}";

    public const string WriteArticleEditBase = "/write-article/edit";
    public const string WriteArticleEditEditId = $"{WriteArticleEditBase}/{{EditId:int}}";

    public const string WriteArticleDeleteBase = "/write-article/delete";
    public const string WriteArticleDeleteDeleteId = $"{WriteArticleDeleteBase}/{{DeleteId:int}}";

    public const string WriteDraft = "/write-draft";

    public const string WriteDraftEditBase = "/write-draft/edit";
    public const string WriteDraftEditEditId = $"{WriteDraftEditBase}/{{EditId:int}}";

    public const string WriteDraftDeleteBase = "/write-draft/delete";
    public const string WriteDraftDeleteDeleteId = $"{WriteDraftDeleteBase}/{{DeleteId:int}}";

    public const string CommentsUrlTemplate = $"{PostBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{PostBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{Tag}/{{0}}";
    public const string EditPostUrlTemplate = $"{WriteArticleEditBase}/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteArticleDeleteBase}/{{0}}";
}
