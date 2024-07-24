using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.RoadMaps.RoutingConstants;

public static class RoadMapsBreadCrumbs
{
    public static readonly BreadCrumb WriteLearningPath = new()
    {
        Title = "ایجاد یک نقشه راه",
        Url = RoadMapsRoutingConstants.WriteLearningPath,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb LearningPathsTag = new()
    {
        Title = "گروه‌های نقشه‌های راه",
        Url = RoadMapsRoutingConstants.LearningPathsTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb LearningPathsWriters = new()
    {
        Title = "نویسنده‌های نقشه‌های راه",
        Url = RoadMapsRoutingConstants.LearningPathsWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb LearningPaths = new()
    {
        Title = "نقشه‌های راه",
        Url = RoadMapsRoutingConstants.LearningPaths2,
        GlyphIcon = DntBootstrapIcons.BiSignMergeRight
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        WriteLearningPath, LearningPathsTag, LearningPathsWriters, LearningPaths
    ];
}
