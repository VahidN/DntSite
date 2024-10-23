namespace DntSite.Web.Features.Posts.RoutingConstants;

public static class PostsRoutingConstants
{
    public const string ComingSoon1 = "/ComingSoon";
    public const string ComingSoon2 = "/coming-soon";

    public const string MyDrafts = "/my-drafts";

    public const string PostBase = "/post";
    public const string PostId = $"{PostBase}/{{Id:int}}";
    public const string PostIdSlug = $"{PostBase}/{{Id:int}}/{{Slug}}";

    // Such as /2012/05/ef-code-first-15.html
    public const string OldBloggerPostUrls = "/{PublishYear}/{PublishMonth}/{OldTitle}.html";

    public const string PageCurrentPage = "/page/{CurrentPage:int?}";
    public const string Posts = "/posts";
    public const string PostsArchive = "/PostsArchive";
    public const string PostsPageCurrentPage = $"{Posts}/page/{{CurrentPage:int?}}";

    public const string PostsFilterFilterBase = $"{Posts}/filter";

    public const string PostsFilterFilterPageCurrentPage =
        $"{PostsFilterFilterBase}/{{Filter}}/page/{{CurrentPage:int?}}";

    public const string PostsWriters = "/posts-writers";
    public const string PostsWritersPageCurrentPage = $"{PostsWriters}/page/{{CurrentPage:int?}}";
    public const string PostsWritersUserFriendlyName = $"{PostsWriters}/{{UserFriendlyName}}";

    public const string PostsWritersOld = "/writer";
    public const string PostsWritersOldUserFriendlyName = $"{PostsWritersOld}/{{UserFriendlyName}}";

    public const string PostsWritersUserFriendlyNamePageCurrentPage =
        $"{PostsWriters}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string AllDraftsList = "/all-drafts-list";
    public const string PostsComments = "/posts-comments";
    public const string PostsCommentsPageCurrentPage = $"{PostsComments}/page/{{CurrentPage:int?}}";
    public const string PostsCommentsUserFriendlyName = $"{PostsComments}/{{UserFriendlyName}}";

    public const string OldPostsCommentsUserFriendlyName = "/comments/{UserFriendlyName}";

    public const string PostsCommentsOld = "/commentsarchive";
    public const string PostsCommentsWithIndexOld = $"{PostsCommentsOld}/index/{{CurrentPage:int?}}";

    public const string PostsCommentsUserFriendlyNamePageCurrentPage =
        $"{PostsComments}/{{UserFriendlyName}}/page/{{CurrentPage:int?}}";

    public const string ShowDraftBase = "/show-draft";
    public const string ShowDraftShowId = $"{ShowDraftBase}/{{ShowId:int}}";

    public const string Tag = "/tag";
    public const string TagPageCurrentPage = $"{Tag}/page/{{CurrentPage:int?}}";
    public const string TagTagName = $"{Tag}/{{TagName}}";
    public const string TagTagNamePageCurrentPage = $"{Tag}/{{TagName}}/page/{{CurrentPage:int?}}";

    public const string Search = "/search";
    public const string SearchLabelTagName = $"{Search}/label/{{TagName}}";

    public const string WriteArticleEditBase = "/write-article/edit";
    public const string WriteArticleEditEditId = $"{WriteArticleEditBase}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteArticleDeleteBase = "/write-article/delete";

    public const string WriteArticleDeleteDeleteId =
        $"{WriteArticleDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteDraft = "/write-draft";
    public const string WriteDraftEditBase = $"{WriteDraft}/edit";
    public const string WriteDraftEditEditId = $"{WriteDraftEditBase}/{{EditId:{EncryptedRouteConstraint.Name}}}";

    public const string WriteDraftDeleteBase = $"{WriteDraft}/delete";

    public const string WriteDraftDeleteDeleteId =
        $"{WriteDraftDeleteBase}/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string CommentsUrlTemplate = $"{PostBase}/{{0}}#comments";
    public const string PostUrlTemplate = $"{PostBase}/{{0}}";
    public const string PostTagUrlTemplate = $"{Tag}/{{0}}";

    public const string EditPostUrlTemplate = $"{WriteArticleEditBase}/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteArticleDeleteBase}/{{0}}";
}
