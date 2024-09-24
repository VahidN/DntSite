namespace DntSite.Web.Features.News.RoutingConstants;

public static class NewsRoutingConstants
{
    public const string News = "/news";
    public const string NewsArchive = "/newsarchive";
    public const string NewsArchivePageCurrentPage = $"{NewsArchive}/index/{{CurrentPage:int}}";
    public const string NewsPageCurrentPage = $"{News}/page/{{CurrentPage:int}}";

    public const string NewsFilterBase = $"{News}/filter";
    public const string NewsFilterFilterPageCurrentPage = $"{NewsFilterBase}/{{Filter}}/page/{{CurrentPage:int}}";

    public const string NewsRedirectBase = $"{News}/redirect";
    public const string NewsRedirectRedirectId = $"{NewsRedirectBase}/{{RedirectId:int}}";
    public const string NewsArchiveNewsRedirectId = $"{NewsArchive}/news/{{RedirectId:int}}";

    public const string NewsDetailsBase = $"{News}/details";
    public const string NewsDetailsNewsId = $"{NewsDetailsBase}/{{NewsId:int}}";
    public const string NewsArchiveDetailsNewsId = $"{NewsArchive}/details/{{NewsId:int}}";

    public const string NewsTag = "/news-tag";
    public const string NewsTagPageCurrentPage = $"{NewsTag}/page/{{CurrentPage:int}}";
    public const string NewsTagTagName = $"{NewsTag}/{{TagName}}";
    public const string NewsTagOldTagName = "/newstag/{TagName}";
    public const string NewsTagTagNamePageCurrentPage = $"{NewsTag}/{{TagName}}/page/{{CurrentPage:int}}";

    public const string NewsWriters = "/news-writers";
    public const string NewsWritersPageCurrentPage = $"{NewsWriters}/page/{{CurrentPage:int}}";
    public const string NewsWritersUserFriendlyName = $"{NewsWriters}/{{UserFriendlyName}}";

    public const string NewsWriterOld = "/newswriter";
    public const string NewsWriterOldUserFriendlyName = $"{NewsWriterOld}/{{UserFriendlyName}}";

    public const string NewsWritersUserFriendlyNamePageCurrentPage =
        $"{NewsWriters}/{{UserFriendlyName}}/page/{{CurrentPage:int}}";

    public const string NewsComments = "/news-comments";
    public const string NewsCommentsPageCurrentPage = $"{NewsComments}/page/{{CurrentPage:int}}";
    public const string NewsCommentsUserFriendlyName = $"{NewsComments}/{{UserFriendlyName}}";

    public const string NewsCommentsOld = $"{NewsArchive}/comments";

    public const string NewsCommentsUserFriendlyNamePageCurrentPage =
        $"{NewsComments}/{{UserFriendlyName}}/page/{{CurrentPage:int}}";

    public const string DailyLinks = "/DailyLinks";
    public const string WriteNews = "/write-news";
    public const string WriteNewsEditEditId = $"{WriteNews}/edit/{{EditId:{EncryptedRouteConstraint.Name}}}";
    public const string WriteNewsDeleteDeleteId = $"{WriteNews}/delete/{{DeleteId:{EncryptedRouteConstraint.Name}}}";

    public const string CommentsUrlTemplate = $"{News}/details/{{0}}#comments";
    public const string PostUrlTemplate = $"{News}/details/{{0}}";
    public const string PostTagUrlTemplate = "/news-tag/{0}";

    public const string EditPostUrlTemplate = $"{WriteNews}/edit/{{0}}";
    public const string DeletePostUrlTemplate = $"{WriteNews}/delete/{{0}}";
}
