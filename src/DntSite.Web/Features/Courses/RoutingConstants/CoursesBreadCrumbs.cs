using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Courses.RoutingConstants;

public static class CoursesBreadCrumbs
{
    public static readonly BreadCrumb CoursesTag = new()
    {
        Title = "گروه‌های دوره‌ها",
        Url = CoursesRoutingConstants.CoursesTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb CoursesWriters = new()
    {
        Title = "نویسنده‌های دوره‌ها",
        Url = CoursesRoutingConstants.CoursesWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb CoursesComments = new()
    {
        Title = "نظرات دوره‌ها",
        Url = CoursesRoutingConstants.CoursesComments,
        GlyphIcon = DntBootstrapIcons.BiChat
    };

    public static readonly BreadCrumb Courses = new()
    {
        Title = "دوره‌ها",
        Url = CoursesRoutingConstants.Courses,
        GlyphIcon = DntBootstrapIcons.BiMortarboard
    };

    public static readonly BreadCrumb WriteCourse = new()
    {
        Title = "تعریف شناسنامه دوره",
        Url = CoursesRoutingConstants.WriteCourse,
        GlyphIcon = DntBootstrapIcons.BiPencil
    };

    public static readonly BreadCrumb CoursesBookmarksBreadCrumb = new()
    {
        Title = "دوره‌های مورد علاقه",
        Url = CoursesRoutingConstants.CoursesBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb CoursesTopicsBookmarksBreadCrumb = new()
    {
        Title = "مطالب مورد علاقه",
        Url = CoursesRoutingConstants.CourseTopicsBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        CoursesBookmarksBreadCrumb, CoursesTopicsBookmarksBreadCrumb, CoursesTag, CoursesWriters, CoursesComments,
        Courses
    ];
}
