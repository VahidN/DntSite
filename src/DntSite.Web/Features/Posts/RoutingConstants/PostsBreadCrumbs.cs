using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Posts.RoutingConstants;

public static class PostsBreadCrumbs
{
    public static readonly BreadCrumb MyDrafts = new()
    {
        Title = "پیش‌نویس‌های من",
        Url = PostsRoutingConstants.MyDrafts,
        GlyphIcon = DntBootstrapIcons.BiListTask,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb ComingSoon = new()
    {
        Title = "به زودی در سایت",
        Url = PostsRoutingConstants.ComingSoon2,
        GlyphIcon = DntBootstrapIcons.BiClock
    };

    public static readonly BreadCrumb PostsWriters = new()
    {
        Title = "نویسنده‌های مطالب",
        Url = PostsRoutingConstants.PostsWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb Posts = new()
    {
        Title = "مطالب",
        Url = PostsRoutingConstants.Posts,
        GlyphIcon = DntBootstrapIcons.BiPencil
    };

    public static readonly BreadCrumb AllDraftsList = new()
    {
        Title = "تمام پیش نویس‌های منتشر نشده",
        Url = PostsRoutingConstants.AllDraftsList,
        GlyphIcon = DntBootstrapIcons.BiCardChecklist,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb WriteDraft = new()
    {
        Title = "ارسال یک مطالب",
        Url = PostsRoutingConstants.WriteDraft,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb Tags = new()
    {
        Title = "گروه‌های مطالب",
        Url = PostsRoutingConstants.Tag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb PostsComments = new()
    {
        Title = "نظرات مطالب",
        Url = PostsRoutingConstants.PostsComments,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        WriteDraft, MyDrafts, ComingSoon, PostsWriters, Tags, PostsComments, Posts
    ];
}
