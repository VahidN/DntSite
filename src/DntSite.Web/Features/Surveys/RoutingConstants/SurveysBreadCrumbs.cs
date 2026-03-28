using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Surveys.RoutingConstants;

public static class SurveysBreadCrumbs
{
    public static readonly BreadCrumb WriteSurvey = new()
    {
        Title = "ارسال یک نظرسنجی",
        Url = SurveysRoutingConstants.WriteSurvey,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb SurveysTag = new()
    {
        Title = "گروه‌های نظرسنجی‌ها",
        Url = SurveysRoutingConstants.SurveysTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb SurveysWriters = new()
    {
        Title = "نویسنده‌های نظرسنجی‌ها",
        Url = SurveysRoutingConstants.SurveysWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb SurveysComments = new()
    {
        Title = "نظرات نظرسنجی‌ها",
        Url = SurveysRoutingConstants.SurveysComments,
        GlyphIcon = DntBootstrapIcons.BiChat
    };

    public static readonly BreadCrumb SurveysArchive = new()
    {
        Title = "نظرسنجی‌ها",
        Url = SurveysRoutingConstants.SurveysArchive,
        GlyphIcon = DntBootstrapIcons.BiCardChecklist
    };

    public static readonly BreadCrumb SurveysBookmarksBreadCrumb = new()
    {
        Title = "علاقمندی‌های شخصی",
        Url = SurveysRoutingConstants.SurveysBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        SurveysBookmarksBreadCrumb, WriteSurvey, SurveysTag, SurveysWriters, SurveysComments, SurveysArchive
    ];
}
