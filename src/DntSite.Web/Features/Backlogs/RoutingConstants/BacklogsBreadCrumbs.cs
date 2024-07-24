using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Backlogs.RoutingConstants;

public static class BacklogsBreadCrumbs
{
    public static readonly BreadCrumb WriteBacklogBreadCrumb = new()
    {
        Title = "ارسال پیشنهاد",
        Url = BacklogsRoutingConstants.WriteBacklog,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb BacklogsTagBreadCrumb = new()
    {
        Title = "گروه‌های پیشنهاد‌ها",
        Url = BacklogsRoutingConstants.BacklogsTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb BacklogsWritersBreadCrumb = new()
    {
        Title = "نویسنده‌های پیشنهاد‌ها",
        Url = BacklogsRoutingConstants.BacklogsWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb BacklogsBreadCrumb = new()
    {
        Title = "پیشنهاد‌ها",
        Url = BacklogsRoutingConstants.Backlogs,
        GlyphIcon = DntBootstrapIcons.BiListTask
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        WriteBacklogBreadCrumb, BacklogsTagBreadCrumb, BacklogsWritersBreadCrumb, BacklogsBreadCrumb
    ];
}
