namespace DntSite.Web.Features.RssFeeds.Models;

public sealed class WhatsNewItemType
{
    public const string CommentsOf = "نظرات";
    public const string RepliesOf = "پاسخ";

    private static readonly Dictionary<string, WhatsNewItemType> WhatsNewItemTypes = new()
    {
        {
            nameof(QuestionsComments), new WhatsNewItemType($"{RepliesOf} به پرسش‌ها", bgColor: "bg-primary")
        },
        {
            nameof(Backlogs), new WhatsNewItemType(value: "پیشنهادها", bgColor: "bg-secondary")
        },
        {
            nameof(Questions), new WhatsNewItemType(value: "پرسش‌ها", bgColor: "bg-success")
        },
        {
            nameof(LearningPaths), new WhatsNewItemType(value: "مسیرراه‌ها", bgColor: "bg-danger")
        },
        {
            nameof(AllCoursesTopics), new WhatsNewItemType(value: "مطالب دوره‌ها", bgColor: "bg-warning")
        },
        {
            nameof(AllCourses), new WhatsNewItemType(value: "دوره‌ها", bgColor: "bg-info")
        },
        {
            nameof(AllVotes), new WhatsNewItemType(value: "نظرسنجی‌ها", bgColor: "bg-dark")
        },
        {
            nameof(AllAdvertisements), new WhatsNewItemType(value: "آگهی‌ها", bgColor: "bg-primary bg-gradient")
        },
        {
            nameof(AllDrafts), new WhatsNewItemType(value: "به زودی", bgColor: "bg-secondary bg-gradient")
        },
        {
            nameof(ProjectsNews), new WhatsNewItemType(value: "پروژه‌ها", bgColor: "bg-success bg-gradient")
        },
        {
            nameof(ProjectsFiles), new WhatsNewItemType(value: "فایل‌های پروژه‌ها", bgColor: "bg-danger bg-gradient")
        },
        {
            nameof(ProjectsIssues),
            new WhatsNewItemType(value: "بازخوردهای پروژه‌ها", bgColor: "bg-warning bg-gradient")
        },
        {
            nameof(ProjectsIssuesReplies),
            new WhatsNewItemType($"{RepliesOf} به بازخورد‌های پروژه‌ها", bgColor: "bg-info bg-gradient")
        },
        {
            nameof(VotesReplies), new WhatsNewItemType($"{CommentsOf} نظرسنجی‌ها", bgColor: "bg-dark bg-gradient")
        },
        {
            nameof(AdvertisementComments),
            new WhatsNewItemType($"{CommentsOf} آگهی‌ها", bgColor: "bg-primary-subtle text-primary-emphasis")
        },
        {
            nameof(ProjectFaqs),
            new WhatsNewItemType(value: "راهنماهای پروژه", bgColor: "bg-secondary-subtle text-secondary-emphasis")
        },
        {
            nameof(ProjectsFaqs),
            new WhatsNewItemType(value: "راهنماهای پروژه‌ها", bgColor: "bg-light text-dark")
        },
        {
            nameof(ProjectFiles),
            new WhatsNewItemType(value: "فایل‌های پروژه‌", bgColor: "bg-danger-subtle text-danger-emphasis")
        },
        {
            nameof(ProjectIssues),
            new WhatsNewItemType(value: "بازخورد‌های پروژه‌", bgColor: "bg-warning-subtle text-warning-emphasis")
        },
        {
            nameof(ProjectIssuesReplies),
            new WhatsNewItemType($"{RepliesOf} ‌به بازخورد‌های پروژه‌", bgColor: "bg-info-subtle text-info-emphasis")
        },
        {
            nameof(Posts), new WhatsNewItemType(value: "مطالب", bgColor: "bg-success-subtle text-success-emphasis")
        },
        {
            nameof(Comments),
            new WhatsNewItemType($"{CommentsOf} مطالب", bgColor: "bg-primary-subtle text-primary-emphasis bg-gradient")
        },
        {
            nameof(News),
            new WhatsNewItemType(value: "اشتراک‌ها", bgColor: "bg-secondary-subtle text-secondary-emphasis bg-gradient")
        },
        {
            nameof(Tag),
            new WhatsNewItemType(value: "گروه‌ها", bgColor: "bg-success-subtle text-success-emphasis bg-gradient")
        },
        {
            nameof(Author),
            new WhatsNewItemType(value: "نویسنده‌ها", bgColor: "bg-danger-subtle text-danger-emphasis bg-gradient")
        },
        {
            nameof(NewsComments),
            new WhatsNewItemType($"{CommentsOf} اشتراک‌ها",
                bgColor: "bg-warning-subtle text-warning-emphasis bg-gradient")
        },
        {
            nameof(NewsAuthor),
            new WhatsNewItemType(value: "اشتراک‌های اشخاص", bgColor: "bg-info-subtle text-info-emphasis bg-gradient")
        },
        {
            nameof(CourseTopicsReplies),
            new WhatsNewItemType(value: "بازخوردهای دوره", bgColor: "bg-light text-dark bg-gradient")
        }
    };

    private WhatsNewItemType(string value, string bgColor)
    {
        Value = value;
        BgColor = bgColor;
    }

    public string Value { get; }

    public string BgColor { get; }

    public static WhatsNewItemType QuestionsComments => WhatsNewItemTypes[nameof(QuestionsComments)];

    public static WhatsNewItemType Backlogs => WhatsNewItemTypes[nameof(Backlogs)];

    public static WhatsNewItemType Questions => WhatsNewItemTypes[nameof(Questions)];

    public static WhatsNewItemType LearningPaths => WhatsNewItemTypes[nameof(LearningPaths)];

    public static WhatsNewItemType AllCoursesTopics => WhatsNewItemTypes[nameof(AllCoursesTopics)];

    public static WhatsNewItemType AllCourses => WhatsNewItemTypes[nameof(AllCourses)];

    public static WhatsNewItemType AllVotes => WhatsNewItemTypes[nameof(AllVotes)];

    public static WhatsNewItemType AllAdvertisements => WhatsNewItemTypes[nameof(AllAdvertisements)];

    public static WhatsNewItemType AllDrafts => WhatsNewItemTypes[nameof(AllDrafts)];

    public static WhatsNewItemType ProjectsNews => WhatsNewItemTypes[nameof(ProjectsNews)];

    public static WhatsNewItemType ProjectsFiles => WhatsNewItemTypes[nameof(ProjectsFiles)];

    public static WhatsNewItemType ProjectsIssues => WhatsNewItemTypes[nameof(ProjectsIssues)];

    public static WhatsNewItemType ProjectsIssuesReplies => WhatsNewItemTypes[nameof(ProjectsIssuesReplies)];

    public static WhatsNewItemType VotesReplies => WhatsNewItemTypes[nameof(VotesReplies)];

    public static WhatsNewItemType AdvertisementComments => WhatsNewItemTypes[nameof(AdvertisementComments)];

    public static WhatsNewItemType ProjectFaqs => WhatsNewItemTypes[nameof(ProjectFaqs)];

    public static WhatsNewItemType ProjectsFaqs => WhatsNewItemTypes[nameof(ProjectsFaqs)];

    public static WhatsNewItemType ProjectFiles => WhatsNewItemTypes[nameof(ProjectFiles)];

    public static WhatsNewItemType ProjectIssues => WhatsNewItemTypes[nameof(ProjectIssues)];

    public static WhatsNewItemType ProjectIssuesReplies => WhatsNewItemTypes[nameof(ProjectIssuesReplies)];

    public static WhatsNewItemType Posts => WhatsNewItemTypes[nameof(Posts)];

    public static WhatsNewItemType Comments => WhatsNewItemTypes[nameof(Comments)];

    public static WhatsNewItemType News => WhatsNewItemTypes[nameof(News)];

    public static WhatsNewItemType Tag => WhatsNewItemTypes[nameof(Tag)];

    public static WhatsNewItemType Author => WhatsNewItemTypes[nameof(Author)];

    public static WhatsNewItemType NewsComments => WhatsNewItemTypes[nameof(NewsComments)];

    public static WhatsNewItemType NewsAuthor => WhatsNewItemTypes[nameof(NewsAuthor)];

    public static WhatsNewItemType CourseTopicsReplies => WhatsNewItemTypes[nameof(CourseTopicsReplies)];

    public static WhatsNewItemType Get(string value)
    {
        var item = WhatsNewItemTypes.Values.FirstOrDefault(item
            => string.Equals(item.Value, value, StringComparison.Ordinal));

        return item ?? new WhatsNewItemType(value, bgColor: "bg-secondary");
    }
}
