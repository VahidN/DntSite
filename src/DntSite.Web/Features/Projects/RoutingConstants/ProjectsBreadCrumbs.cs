using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Projects.RoutingConstants;

public static class ProjectsBreadCrumbs
{
    public static readonly BreadCrumb WriteProjects = new()
    {
        Title = "ارسال یک پروژه",
        Url = ProjectsRoutingConstants.WriteProject,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb ProjectsTag = new()
    {
        Title = "گروه‌های پروژه‌ها",
        Url = ProjectsRoutingConstants.ProjectsTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb ProjectsWriters = new()
    {
        Title = "نویسنده‌های پروژه‌ها",
        Url = ProjectsRoutingConstants.ProjectsWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb ProjectsComments = new()
    {
        Title = "نظرات پروژه‌ها",
        Url = ProjectsRoutingConstants.ProjectsComments,
        GlyphIcon = DntBootstrapIcons.BiChat
    };

    public static readonly BreadCrumb ProjectsFeedbacks = new()
    {
        Title = "بازخوردهای پروژه‌‌ها",
        Url = ProjectsRoutingConstants.ProjectsFeedbacks,
        GlyphIcon = DntBootstrapIcons.BiQuestionCircle
    };

    public static readonly BreadCrumb ProjectsFaqs = new()
    {
        Title = "راهنماهای پروژه‌‌ها",
        Url = ProjectsRoutingConstants.ProjectsFaqs,
        GlyphIcon = DntBootstrapIcons.BiFiletypeDoc
    };

    public static readonly BreadCrumb ProjectsReleases = new()
    {
        Title = "فایل‌های پروژه‌‌ها",
        Url = ProjectsRoutingConstants.ProjectsReleases,
        GlyphIcon = DntBootstrapIcons.BiFile
    };

    public static readonly BreadCrumb Projects = new()
    {
        Title = "پروژه‌ها",
        Url = ProjectsRoutingConstants.Projects,
        GlyphIcon = DntBootstrapIcons.BiPuzzle
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        WriteProjects, ProjectsTag, ProjectsWriters, ProjectsFeedbacks, ProjectsReleases, ProjectsFaqs,
        ProjectsComments, Projects
    ];

    public static readonly BreadCrumb ProjectFaqsBookmarksBreadCrumb = new()
    {
        Title = "علاقمندی‌های شخصی",
        Url = ProjectsRoutingConstants.ProjectFaqsBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb ProjectIssuesBookmarksBreadCrumb = new()
    {
        Title = "علاقمندی‌های شخصی",
        Url = ProjectsRoutingConstants.ProjectIssuesBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb ProjectReleasesBookmarksBreadCrumb = new()
    {
        Title = "علاقمندی‌های شخصی",
        Url = ProjectsRoutingConstants.ProjectReleasesBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb ProjectsBookmarksBreadCrumb = new()
    {
        Title = "علاقمندی‌های شخصی",
        Url = ProjectsRoutingConstants.ProjectsBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static BreadCrumb Project(string name, int? projectId)
        => new()
        {
            Title = $"پروژه {name}",
            Url = string.Create(CultureInfo.InvariantCulture,
                $"{ProjectsRoutingConstants.ProjectsDetailsBase}/{projectId}"),
            GlyphIcon = DntBootstrapIcons.BiPuzzle
        };

    public static BreadCrumb WriteProjectFeedback(int? projectId)
        => new()
        {
            AllowAnonymous = false,
            Title = "ارسال یک بازخورد جدید",
            GlyphIcon = DntBootstrapIcons.BiChatRight,
            Url = string.Create(CultureInfo.InvariantCulture,
                $"{ProjectsRoutingConstants.WriteProjectFeedbackBase}/{projectId}")
        };

    public static BreadCrumb ProjectComments(int? projectId)
        => new()
        {
            Title = "نظرات پروژه‌",
            Url = string.Create(CultureInfo.InvariantCulture,
                $"{ProjectsRoutingConstants.ProjectCommentsBase}/{projectId}"),
            GlyphIcon = DntBootstrapIcons.BiChat
        };

    public static BreadCrumb ProjectFeedbacks(int? projectId)
        => new()
        {
            Title = "بازخوردهای پروژه‌",
            Url = string.Create(CultureInfo.InvariantCulture,
                $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{projectId}"),
            GlyphIcon = DntBootstrapIcons.BiQuestionCircle
        };

    public static BreadCrumb ProjectFaqs(int? projectId)
        => new()
        {
            Title = "راهنماهای پروژه‌",
            Url =
                string.Create(CultureInfo.InvariantCulture, $"{ProjectsRoutingConstants.ProjectFaqsBase}/{projectId}"),
            GlyphIcon = DntBootstrapIcons.BiFiletypeDoc
        };

    public static BreadCrumb ProjectReleases(int? projectId)
        => new()
        {
            Title = "فایل‌های پروژه",
            Url = string.Create(CultureInfo.InvariantCulture,
                $"{ProjectsRoutingConstants.ProjectReleasesBase}/{projectId}"),
            GlyphIcon = DntBootstrapIcons.BiFile
        };

    public static IList<BreadCrumb> DefaultProjectBreadCrumbs(string name, int? projectId)
        =>
        [
            Project(name, projectId), WriteProjectFeedback(projectId), ProjectComments(projectId),
            ProjectFeedbacks(projectId), ProjectFaqs(projectId), ProjectReleases(projectId)
        ];
}
