using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.News.RoutingConstants;

public static class NewsBreadCrumbs
{
    public static readonly BreadCrumb WriteNews = new()
    {
        Title = "ارسال یک اشتراک",
        Url = NewsRoutingConstants.WriteNews,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb NewsTag = new()
    {
        Title = "گروه‌های اشتراک‌ها",
        Url = NewsRoutingConstants.NewsTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb NewsWriters = new()
    {
        Title = "نویسنده‌های اشتراک‌ها",
        Url = NewsRoutingConstants.NewsWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb NewsComments = new()
    {
        Title = "نظرات اشتراک‌ها",
        Url = NewsRoutingConstants.NewsComments,
        GlyphIcon = DntBootstrapIcons.BiChat
    };

    public static readonly BreadCrumb News = new()
    {
        Title = "اشتراک‌ها",
        Url = NewsRoutingConstants.News,
        GlyphIcon = DntBootstrapIcons.BiShare
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs = [WriteNews, NewsTag, NewsWriters, NewsComments, News];
}
