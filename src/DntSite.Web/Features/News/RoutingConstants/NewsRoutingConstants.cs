namespace DntSite.Web.Features.News.RoutingConstants;

public static class NewsRoutingConstants
{
    public const string News = "/news";
    public const string NewsPageCurrentPage = "/news/page/{CurrentPage:int}";

    public const string NewsFilterBase = "/news/filter";
    public const string NewsFilterFilterPageCurrentPage = $"{NewsFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string NewsRedirectBase = "/news/redirect";
    public const string NewsRedirectRedirectId = $"{NewsRedirectBase}/{{RedirectId:int}}";

    public const string NewsDetailsBase = "/news/details";
    public const string NewsDetailsNewsId = $"{NewsDetailsBase}/{{NewsId:int}}";

    public const string NewsTag = "/news-tag";
    public const string NewsTagPageCurrentPage = "/news-tag/page/{CurrentPage:int}";
    public const string NewsTagTagName = "/news-tag/{TagName}";
    public const string NewsTagTagNamePageCurrentPage = "/news-tag/{TagName}/page/{CurrentPage:int}";
    public const string NewsWriters = "/news-writers";
    public const string NewsWritersPageCurrentPage = "/news-writers/page/{CurrentPage:int}";
    public const string NewsWritersUserFriendlyName = "/news-writers/{UserFriendlyName}";

    public const string NewsWritersUserFriendlyNamePageCurrentPage =
        "/news-writers/{UserFriendlyName}/page/{CurrentPage:int}";

    public const string NewsComments = "/news-comments";
    public const string NewsCommentsPageCurrentPage = "/news-comments/page/{CurrentPage:int}";
    public const string NewsCommentsUserFriendlyName = "/news-comments/{UserFriendlyName}";

    public const string NewsCommentsUserFriendlyNamePageCurrentPage =
        "/news-comments/{UserFriendlyName}/page/{CurrentPage:int}";

    public const string DailyLinks = "/DailyLinks";
    public const string WriteNews = "/write-news";
    public const string WriteNewsEditEditId = "/write-news/edit/{EditId:int}";
    public const string WriteNewsDeleteDeleteId = "/write-news/delete/{DeleteId:int}";

    public const string CommentsUrlTemplate = "news/details/{0}#comments";
    public const string PostUrlTemplate = "news/details/{0}";
    public const string PostTagUrlTemplate = "news-tag/{0}";
    public const string EditPostUrlTemplate = "write-news/edit/{0}";
    public const string DeletePostUrlTemplate = "write-news/delete/{0}";
}
