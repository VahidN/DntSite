namespace DntSite.Web.Features.RssFeeds.Models;

public sealed class WhatsNewItemType
{
    public const string CommentsOf = "نظرات";
    public const string RepliesOf = "پاسخ";
    public const string BacklogsName = "پیشنهادها";
    public const string QuestionsName = "پرسش‌ها";
    public const string LearningPathsName = "مسیرراه‌ها";
    public const string AllCoursesTopicsName = "مطالب دوره‌ها";
    public const string AllCoursesName = "دوره‌ها";
    public const string AllVotesName = "نظرسنجی‌ها";
    public const string AllAdvertisementsName = "آگهی‌ها";
    public const string AllDraftsName = "به زودی";
    public const string ProjectsNewsName = "پروژه‌ها";
    public const string ProjectsFilesName = "فایل‌های پروژه‌ها";
    public const string ProjectsIssuesName = "بازخوردهای پروژه‌ها";
    public const string ProjectFaqsName = "راهنماهای پروژه";
    public const string ProjectsFaqsName = "راهنماهای پروژه‌ها";
    public const string ProjectFilesName = "فایل‌های پروژه‌";
    public const string ProjectIssuesName = "بازخورد‌های پروژه‌";
    public const string PostsName = "مطالب";
    public const string NewsName = "اشتراک‌ها";
    public const string NewsTagName = "گروه اشتراک‌ها";
    public const string TagName = "گروه‌ها";
    public const string AuthorName = "نویسنده‌ها";
    public const string NewsAuthorName = "اشتراک‌های اشخاص";
    public const string CourseTopicsRepliesName = "بازخوردهای دوره";

    private static readonly Dictionary<string, WhatsNewItemType> WhatsNewItemTypes = new()
    {
        {
            nameof(QuestionsComments), new WhatsNewItemType($"{RepliesOf} به پرسش‌ها", bgColor: "bg-primary")
        },
        {
            nameof(Backlogs), new WhatsNewItemType(BacklogsName, bgColor: "bg-secondary")
        },
        {
            nameof(Questions), new WhatsNewItemType(QuestionsName, bgColor: "bg-success")
        },
        {
            nameof(LearningPaths), new WhatsNewItemType(LearningPathsName, bgColor: "bg-danger")
        },
        {
            nameof(AllCoursesTopics), new WhatsNewItemType(AllCoursesTopicsName, bgColor: "bg-warning")
        },
        {
            nameof(AllCourses), new WhatsNewItemType(AllCoursesName, bgColor: "bg-info")
        },
        {
            nameof(AllVotes), new WhatsNewItemType(AllVotesName, bgColor: "bg-dark")
        },
        {
            nameof(AllAdvertisements), new WhatsNewItemType(AllAdvertisementsName, bgColor: "bg-primary bg-gradient")
        },
        {
            nameof(AllDrafts), new WhatsNewItemType(AllDraftsName, bgColor: "bg-secondary bg-gradient")
        },
        {
            nameof(ProjectsNews), new WhatsNewItemType(ProjectsNewsName, bgColor: "bg-success bg-gradient")
        },
        {
            nameof(ProjectsFiles), new WhatsNewItemType(ProjectsFilesName, bgColor: "bg-danger bg-gradient")
        },
        {
            nameof(ProjectsIssues), new WhatsNewItemType(ProjectsIssuesName, bgColor: "bg-warning bg-gradient")
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
            new WhatsNewItemType(ProjectFaqsName, bgColor: "bg-secondary-subtle text-secondary-emphasis")
        },
        {
            nameof(ProjectsFaqs), new WhatsNewItemType(ProjectsFaqsName, bgColor: "bg-light text-dark")
        },
        {
            nameof(ProjectFiles),
            new WhatsNewItemType(ProjectFilesName, bgColor: "bg-danger-subtle text-danger-emphasis")
        },
        {
            nameof(ProjectIssues),
            new WhatsNewItemType(ProjectIssuesName, bgColor: "bg-warning-subtle text-warning-emphasis")
        },
        {
            nameof(ProjectIssuesReplies),
            new WhatsNewItemType($"{RepliesOf} ‌به بازخورد‌های پروژه‌", bgColor: "bg-info-subtle text-info-emphasis")
        },
        {
            nameof(Posts), new WhatsNewItemType(PostsName, bgColor: "bg-success-subtle text-success-emphasis")
        },
        {
            nameof(Comments),
            new WhatsNewItemType($"{CommentsOf} مطالب", bgColor: "bg-primary-subtle text-primary-emphasis bg-gradient")
        },
        {
            nameof(News),
            new WhatsNewItemType(NewsName, bgColor: "bg-secondary-subtle text-secondary-emphasis bg-gradient")
        },
        {
            nameof(NewsTag),
            new WhatsNewItemType(NewsTagName, bgColor: "bg-secondary-subtle text-secondary-emphasis bg-gradient")
        },
        {
            nameof(Tag), new WhatsNewItemType(TagName, bgColor: "bg-success-subtle text-success-emphasis bg-gradient")
        },
        {
            nameof(Author),
            new WhatsNewItemType(AuthorName, bgColor: "bg-danger-subtle text-danger-emphasis bg-gradient")
        },
        {
            nameof(NewsComments),
            new WhatsNewItemType($"{CommentsOf} اشتراک‌ها",
                bgColor: "bg-warning-subtle text-warning-emphasis bg-gradient")
        },
        {
            nameof(NewsAuthor),
            new WhatsNewItemType(NewsAuthorName, bgColor: "bg-info-subtle text-info-emphasis bg-gradient")
        },
        {
            nameof(CourseTopicsReplies),
            new WhatsNewItemType(CourseTopicsRepliesName, bgColor: "bg-light text-dark bg-gradient")
        }
    };

    private WhatsNewItemType(string value, string bgColor)
    {
        Value = value;
        BgColor = bgColor;
    }

    public string Name => WhatsNewItemTypes
        .FirstOrDefault(x => string.Equals(x.Value.Value, Value, StringComparison.Ordinal))
        .Key;

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

    public static WhatsNewItemType NewsTag => WhatsNewItemTypes[nameof(NewsTag)];

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
