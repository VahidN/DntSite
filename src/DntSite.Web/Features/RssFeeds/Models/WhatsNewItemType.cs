namespace DntSite.Web.Features.RssFeeds.Models;

public sealed class WhatsNewItemType
{
    private WhatsNewItemType(string value) => Value = value;

    public string Value { get; }

    public static WhatsNewItemType QuestionsComments => new(value: "پاسخ به پرسش‌ها");

    public static WhatsNewItemType Backlogs => new(value: "پیشنهادها");

    public static WhatsNewItemType Questions => new(value: "پرسش‌ها");

    public static WhatsNewItemType LearningPaths => new(value: "مسیرراه‌ها");

    public static WhatsNewItemType AllCoursesTopics => new(value: "مطالب دوره‌ها");

    public static WhatsNewItemType AllCourses => new(value: "دوره‌ها");

    public static WhatsNewItemType AllVotes => new(value: "نظرسنجی‌ها");

    public static WhatsNewItemType AllAdvertisements => new(value: "آگهی‌ها");

    public static WhatsNewItemType AllDrafts => new(value: "به زودی");

    public static WhatsNewItemType ProjectsNews => new(value: "پروژه‌ها");

    public static WhatsNewItemType ProjectsFiles => new(value: "فایل‌های پروژه‌ها");

    public static WhatsNewItemType ProjectsIssues => new(value: "بازخوردهای پروژه‌ها");

    public static WhatsNewItemType ProjectsIssuesReplies => new(value: "پاسخ به بازخورد‌های پروژه‌ها");

    public static WhatsNewItemType VotesReplies => new(value: "نظرات نظرسنجی‌ها");

    public static WhatsNewItemType AdvertisementComments => new(value: "نظرات آگهی‌ها");

    public static WhatsNewItemType ProjectFaqs => new(value: "راهنماهای پروژه");

    public static WhatsNewItemType ProjectsFaqs => new(value: "راهنماهای پروژه‌ها");

    public static WhatsNewItemType ProjectFiles => new(value: "فایل‌های پروژه‌");

    public static WhatsNewItemType ProjectIssues => new(value: "بازخورد‌های پروژه‌");

    public static WhatsNewItemType ProjectIssuesReplies => new(value: "پاسخ ‌به بازخورد‌های پروژه‌");

    public static WhatsNewItemType Posts => new(value: "مطالب");

    public static WhatsNewItemType Comments => new(value: "نظرات مطالب");

    public static WhatsNewItemType News => new(value: "اشتراک‌ها");

    public static WhatsNewItemType Tag => new(value: "گروه‌ها");

    public static WhatsNewItemType Author => new(value: "نویسنده‌ها");

    public static WhatsNewItemType NewsComments => new(value: "نظرات اشتراک‌ها");

    public static WhatsNewItemType NewsAuthor => new(value: "اشتراک‌های اشخاص");

    public static WhatsNewItemType CourseTopicsReplies => new(value: "بازخوردهای دوره");
}
