using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;

public static class QuestionsBreadCrumbs
{
    public static readonly BreadCrumb WriteQuestion = new()
    {
        Title = "ارسال یک پرسش",
        Url = QuestionsRoutingConstants.WriteQuestion,
        GlyphIcon = DntBootstrapIcons.BiPencil,
        AllowAnonymous = false
    };

    public static readonly BreadCrumb QuestionsTag = new()
    {
        Title = "گروه‌های پرسش‌ها",
        Url = QuestionsRoutingConstants.QuestionsTag,
        GlyphIcon = DntBootstrapIcons.BiTag
    };

    public static readonly BreadCrumb QuestionsWriters = new()
    {
        Title = "پرسشگر‌ها",
        Url = QuestionsRoutingConstants.QuestionsWriters,
        GlyphIcon = DntBootstrapIcons.BiPerson
    };

    public static readonly BreadCrumb QuestionsComments = new()
    {
        Title = "پاسخ‌ها",
        Url = QuestionsRoutingConstants.QuestionsComments,
        GlyphIcon = DntBootstrapIcons.BiReply
    };

    public static readonly BreadCrumb Questions = new()
    {
        Title = "پرسش‌ها",
        Url = QuestionsRoutingConstants.Questions,
        GlyphIcon = DntBootstrapIcons.BiQuestion
    };

    public static readonly BreadCrumb QuestionsBookmarksBreadCrumb = new()
    {
        Title = "علاقمندی‌های شخصی",
        Url = QuestionsRoutingConstants.QuestionsBookmarks,
        GlyphIcon = DntBootstrapIcons.BiBookmarkHeart,
        AllowAnonymous = false
    };

    public static readonly IList<BreadCrumb> DefaultBreadCrumbs =
    [
        QuestionsBookmarksBreadCrumb, WriteQuestion, QuestionsTag, QuestionsWriters, QuestionsComments, Questions
    ];
}
