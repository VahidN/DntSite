using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.PrivateMessages.Entities;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.Searches.Entities;
using DntSite.Web.Features.SideBar.Entities;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Surveys.Entities;

namespace DntSite.Web.Features.UserProfiles.Entities;

public class User : BaseInteractiveEntity<User, UserProfileVisitor, UserProfileBookmark, UserProfileReaction, Role,
    UserProfileComment, UserProfileCommentVisitor, UserProfileCommentBookmark, UserProfileCommentReaction,
    UserProfileUserFile, UserProfileUserFileVisitor>
{
    [IgnoreAudit] public UserStat UserStat { get; set; } = new();

    public UserSocialNetwork? UserSocialNetwork { get; set; }

    public string FriendlyName { get; set; } = default!;

    public string UserName { set; get; } = default!;

    [IgnoreAudit] public string HashedPassword { set; get; } = default!;

    public string EMail { set; get; } = default!;

    public bool IsActive { set; get; }

    public bool ReceiveDailyEmails { set; get; }

    public bool EmailIsValidated { set; get; }

    [IgnoreAudit] public string RegistrationCode { set; get; } = default!;

    [IgnoreAudit] public DateTime? LastVisitDateTime { set; get; }

    public bool IsRestricted { set; get; }

    public string? HomePageUrl { set; get; }

    public string? Photo { set; get; }

    public string? Description { set; get; }

    public DateTime? DateOfBirth { set; get; }

    public string? Location { set; get; }

    public bool IsJobsSeeker { set; get; }

    public bool IsEmailPublic { set; get; }

    /// <summary>
    ///     every time the user changes his Password,
    ///     or an admin changes his Roles or stat/IsActive,
    ///     create a new `SerialNumber` GUID and store it in the DB.
    /// </summary>
    [IgnoreAudit]
    public string? SerialNumber { get; set; }

    public virtual ICollection<Role> Roles { set; get; } = [];

    public virtual ICollection<Role> CreatedRoles { set; get; } = [];

    public virtual ICollection<PrivateMessage> SentPrivateMessages { set; get; } = [];

    public virtual ICollection<PrivateMessage> ReceivedPrivateMessages { set; get; } = [];

    public virtual ICollection<Advertisement> Advertisements { set; get; } = [];

    public virtual ICollection<AdvertisementComment> AdvertisementComments { set; get; } = [];

    public virtual ICollection<Course> UserAllowedCourses { set; get; } = [];

    public virtual ICollection<Backlog> Backlogs { set; get; } = [];

    public virtual ICollection<SurveyItem> SurveyItems { set; get; } = [];

    public virtual ICollection<UserUsedPassword> UserUsedPasswords { get; set; } = [];

    public virtual ICollection<AdvertisementTag> AdvertisementTags { get; set; } = [];

    public virtual ICollection<AdvertisementVisitor> AdvertisementVisitors { get; set; } = [];

    public virtual ICollection<AppDataProtectionKey> AppDataProtectionKeys { get; set; } = [];

    public virtual ICollection<AppLogItem> AppLogItems { get; set; } = [];

    public virtual ICollection<AdvertisementCommentVisitor> AdvertisementCommentVisitors { get; set; } = [];

    public virtual ICollection<AdvertisementBookmark> AdvertisementBookmarks { get; set; } = [];

    public virtual ICollection<AdvertisementCommentBookmark> AdvertisementCommentBookmarks { get; set; } = [];

    public virtual ICollection<AdvertisementCommentReaction> AdvertisementCommentReactions { get; set; } = [];

    public virtual ICollection<AdvertisementReaction> AdvertisementReactions { get; set; } = [];

    public virtual ICollection<Backlog> DoneBacklogs { get; set; } = [];

    public virtual ICollection<BacklogBookmark> BacklogBookmarks { get; set; } = [];

    public virtual ICollection<BacklogVisitor> BacklogVisitors { get; set; } = [];

    public virtual ICollection<BacklogTag> BacklogTags { get; set; } = [];

    public virtual ICollection<BacklogReaction> BacklogReactions { get; set; } = [];

    public virtual ICollection<BacklogCommentVisitor> BacklogCommentVisitors { get; set; } = [];

    public virtual ICollection<BacklogCommentReaction> BacklogCommentReactions { get; set; } = [];

    public virtual ICollection<BacklogComment> BacklogComments { get; set; } = [];

    public virtual ICollection<BacklogCommentBookmark> BacklogCommentBookmarks { get; set; } = [];

    public virtual ICollection<CourseBookmark> CourseBookmarks { get; set; } = [];

    public virtual ICollection<CourseCommentBookmark> CourseCommentBookmarks { get; set; } = [];

    public virtual ICollection<CourseComment> CourseComments { get; set; } = [];

    public virtual ICollection<CourseCommentReaction> CourseCommentReactions { get; set; } = [];

    public virtual ICollection<CourseCommentVisitor> CourseCommentVisitors { get; set; } = [];

    public virtual ICollection<Course> Courses { get; set; } = [];

    public virtual ICollection<CourseQuestionBookmark> CourseQuestionBookmarks { get; set; } = [];

    public virtual ICollection<CourseQuestionCommentBookmark> CourseQuestionCommentBookmarks { get; set; } = [];

    public virtual ICollection<CourseQuestionComment> CourseQuestionComments { get; set; } = [];

    public virtual ICollection<CourseQuestionCommentReaction> CourseQuestionCommentReactions { get; set; } = [];

    public virtual ICollection<CourseQuestionCommentVisitor> CourseQuestionCommentVisitors { get; set; } = [];

    public virtual ICollection<CourseQuestion> CourseQuestions { get; set; } = [];

    public virtual ICollection<CourseQuestionReaction> CourseQuestionReactions { get; set; } = [];

    public virtual ICollection<CourseQuestionTag> CourseQuestionTags { get; set; } = [];

    public virtual ICollection<CourseQuestionVisitor> CourseQuestionVisitors { get; set; } = [];

    public virtual ICollection<CourseReaction> CourseReactions { get; set; } = [];

    public virtual ICollection<CourseTag> CourseTags { get; set; } = [];

    public virtual ICollection<CourseTopicBookmark> CourseTopicBookmarks { get; set; } = [];

    public virtual ICollection<CourseTopicCommentBookmark> CourseTopicCommentBookmarks { get; set; } = [];

    public virtual ICollection<CourseTopicComment> CourseTopicComments { get; set; } = [];

    public virtual ICollection<CourseTopicCommentReaction> CourseTopicCommentReactions { get; set; } = [];

    public virtual ICollection<CourseTopicCommentVisitor> CourseTopicCommentVisitors { get; set; } = [];

    public virtual ICollection<CourseTopic> CourseTopics { get; set; } = [];

    public virtual ICollection<CourseTopicReaction> CourseTopicReactions { get; set; } = [];

    public virtual ICollection<CourseTopicTag> CourseTopicTags { get; set; } = [];

    public virtual ICollection<CourseTopicVisitor> CourseTopicVisitors { get; set; } = [];

    public virtual ICollection<CourseVisitor> CourseVisitors { get; set; } = [];

    public virtual ICollection<UserProfileBookmark> UserBookmarks { get; set; } = [];

    public virtual ICollection<UserProfileCommentBookmark> UserCommentBookmarks { get; set; } = [];

    public virtual ICollection<UserProfileComment> UserComments { get; set; } = [];

    public virtual ICollection<UserProfileCommentReaction> UserCommentReactions { get; set; } = [];

    public virtual ICollection<UserProfileCommentVisitor> UserCommentVisitors { get; set; } = [];

    public virtual ICollection<UserProfileReaction> UserReactions { get; set; } = [];

    public virtual ICollection<UserProfileVisitor> UserVisitors { get; set; } = [];

    public virtual ICollection<User> Users { get; set; } = [];

    public virtual ICollection<DailyNewsItem> DailyNewsItems { get; set; } = [];

    public virtual ICollection<DailyNewsItemBookmark> DailyNewsItemBookmarks { get; set; } = [];

    public virtual ICollection<DailyNewsItemComment> DailyNewsItemComments { get; set; } = [];

    public virtual ICollection<DailyNewsItemCommentBookmark> DailyNewsItemCommentBookmarks { get; set; } = [];

    public virtual ICollection<DailyNewsItemCommentReaction> DailyNewsItemCommentReactions { get; set; } = [];

    public virtual ICollection<DailyNewsItemCommentVisitor> DailyNewsItemCommentVisitors { get; set; } = [];

    public virtual ICollection<DailyNewsItemReaction> DailyNewsItemReactions { get; set; } = [];

    public virtual ICollection<DailyNewsItemTag> DailyNewsItemTags { get; set; } = [];

    public virtual ICollection<DailyNewsItemVisitor> DailyNewsItemVisitors { get; set; } = [];

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = [];

    public virtual ICollection<BlogPostBookmark> BlogPostBookmarks { get; set; } = [];

    public virtual ICollection<BlogPostComment> BlogPostComments { get; set; } = [];

    public virtual ICollection<BlogPostCommentBookmark> BlogPostCommentBookmarks { get; set; } = [];

    public virtual ICollection<BlogPostCommentReaction> BlogPostCommentReactions { get; set; } = [];

    public virtual ICollection<BlogPostCommentVisitor> BlogPostCommentVisitors { get; set; } = [];

    public virtual ICollection<BlogPostDraft> BlogPostDrafts { get; set; } = [];

    public virtual ICollection<BlogPostReaction> BlogPostReactions { get; set; } = [];

    public virtual ICollection<BlogPostTag> BlogPostTags { get; set; } = [];

    public virtual ICollection<BlogPostVisitor> BlogPostVisitors { get; set; } = [];

    public virtual ICollection<MassEmail> MassEmails { get; set; } = [];

    public virtual ICollection<DailyNewsItemAIBacklog> DailyNewsItemAIBacklogs { get; set; } = [];

    public virtual ICollection<PrivateMessageBookmark> PrivateMessageBookmarks { get; set; } = [];

    public virtual ICollection<PrivateMessageComment> PrivateMessageComments { get; set; } = [];

    public virtual ICollection<PrivateMessageCommentBookmark> PrivateMessageCommentBookmarks { get; set; } = [];

    public virtual ICollection<PrivateMessageCommentReaction> PrivateMessageCommentReactions { get; set; } = [];

    public virtual ICollection<PrivateMessageCommentVisitor> PrivateMessageCommentVisitors { get; set; } = [];

    public virtual ICollection<PrivateMessageReaction> PrivateMessageReactions { get; set; } = [];

    public virtual ICollection<PrivateMessageTag> PrivateMessageTags { get; set; } = [];

    public virtual ICollection<PrivateMessageVisitor> PrivateMessageVisitors { get; set; } = [];

    public virtual ICollection<Project> Projects { get; set; } = [];

    public virtual ICollection<ProjectBookmark> ProjectBookmarks { get; set; } = [];

    public virtual ICollection<ProjectComment> ProjectComments { get; set; } = [];

    public virtual ICollection<ProjectCommentBookmark> ProjectCommentBookmarks { get; set; } = [];

    public virtual ICollection<ProjectCommentReaction> ProjectCommentReactions { get; set; } = [];

    public virtual ICollection<ProjectCommentVisitor> ProjectCommentVisitors { get; set; } = [];

    public virtual ICollection<ProjectFaq> ProjectFaqs { get; set; } = [];

    public virtual ICollection<ProjectFaqBookmark> ProjectFaqBookmarks { get; set; } = [];

    public virtual ICollection<ProjectFaqComment> ProjectFaqComments { get; set; } = [];

    public virtual ICollection<ProjectFaqCommentBookmark> ProjectFaqCommentBookmarks { get; set; } = [];

    public virtual ICollection<ProjectFaqCommentReaction> ProjectFaqCommentReactions { get; set; } = [];

    public virtual ICollection<ProjectFaqCommentVisitor> ProjectFaqCommentVisitors { get; set; } = [];

    public virtual ICollection<ProjectFaqReaction> ProjectFaqReactions { get; set; } = [];

    public virtual ICollection<ProjectFaqTag> ProjectFaqTags { get; set; } = [];

    public virtual ICollection<ProjectFaqVisitor> ProjectFaqVisitors { get; set; } = [];

    public virtual ICollection<ProjectIssue> ProjectIssues { get; set; } = [];

    public virtual ICollection<ProjectIssueBookmark> ProjectIssueBookmarks { get; set; } = [];

    public virtual ICollection<ProjectIssueComment> ProjectIssueComments { get; set; } = [];

    public virtual ICollection<ProjectIssueCommentBookmark> ProjectIssueCommentBookmarks { get; set; } = [];

    public virtual ICollection<ProjectIssueCommentReaction> ProjectIssueCommentReactions { get; set; } = [];

    public virtual ICollection<ProjectIssueCommentVisitor> ProjectIssueCommentVisitors { get; set; } = [];

    public virtual ICollection<ProjectIssuePriority> ProjectIssuePriorities { get; set; } = [];

    public virtual ICollection<ProjectIssueReaction> ProjectIssueReactions { get; set; } = [];

    public virtual ICollection<ProjectIssueStatus> ProjectIssueStatus { get; set; } = [];

    public virtual ICollection<ProjectIssueTag> ProjectIssueTags { get; set; } = [];

    public virtual ICollection<ProjectIssueType> ProjectIssueTypes { get; set; } = [];

    public virtual ICollection<ProjectIssueVisitor> ProjectIssueVisitors { get; set; } = [];

    public virtual ICollection<ProjectReaction> ProjectReactions { get; set; } = [];

    public virtual ICollection<ProjectRelease> ProjectReleases { get; set; } = [];

    public virtual ICollection<ProjectReleaseBookmark> ProjectReleaseBookmarks { get; set; } = [];

    public virtual ICollection<ProjectReleaseComment> ProjectReleaseComments { get; set; } = [];

    public virtual ICollection<ProjectReleaseCommentBookmark> ProjectReleaseCommentBookmarks { get; set; } = [];

    public virtual ICollection<ProjectReleaseCommentReaction> ProjectReleaseCommentReactions { get; set; } = [];

    public virtual ICollection<ProjectReleaseCommentVisitor> ProjectReleaseCommentVisitors { get; set; } = [];

    public virtual ICollection<ProjectReleaseReaction> ProjectReleaseReactions { get; set; } = [];

    public virtual ICollection<ProjectReleaseTag> ProjectReleaseTags { get; set; } = [];

    public virtual ICollection<ProjectReleaseVisitor> ProjectReleaseVisitors { get; set; } = [];

    public virtual ICollection<ProjectTag> ProjectTags { get; set; } = [];

    public virtual ICollection<ProjectVisitor> ProjectVisitors { get; set; } = [];

    public virtual ICollection<LearningPath> LearningPaths { get; set; } = [];

    public virtual ICollection<LearningPathBookmark> LearningPathBookmarks { get; set; } = [];

    public virtual ICollection<LearningPathComment> LearningPathComments { get; set; } = [];

    public virtual ICollection<LearningPathCommentBookmark> LearningPathCommentBookmarks { get; set; } = [];

    public virtual ICollection<LearningPathCommentReaction> LearningPathCommentReactions { get; set; } = [];

    public virtual ICollection<LearningPathCommentVisitor> LearningPathCommentVisitors { get; set; } = [];

    public virtual ICollection<LearningPathReaction> LearningPathReactions { get; set; } = [];

    public virtual ICollection<LearningPathTag> LearningPathTags { get; set; } = [];

    public virtual ICollection<LearningPathVisitor> LearningPathVisitors { get; set; } = [];

    public virtual ICollection<SearchItem> SearchItems { get; set; } = [];

    public virtual ICollection<SearchItemBookmark> SearchItemBookmarks { get; set; } = [];

    public virtual ICollection<SearchItemComment> SearchItemComments { get; set; } = [];

    public virtual ICollection<SearchItemCommentBookmark> SearchItemCommentBookmarks { get; set; } = [];

    public virtual ICollection<SearchItemCommentReaction> SearchItemCommentReactions { get; set; } = [];

    public virtual ICollection<SearchItemCommentVisitor> SearchItemCommentVisitors { get; set; } = [];

    public virtual ICollection<SearchItemReaction> SearchItemReactions { get; set; } = [];

    public virtual ICollection<SearchItemTag> SearchItemTags { get; set; } = [];

    public virtual ICollection<SearchItemVisitor> SearchItemVisitors { get; set; } = [];

    public virtual ICollection<CustomSidebar> CustomSidebars { get; set; } = [];

    public virtual ICollection<StackExchangeQuestion> StackExchangeQuestions { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionBookmark> StackExchangeQuestionBookmarks { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionComment> StackExchangeQuestionComments { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionCommentBookmark>
        StackExchangeQuestionCommentBookmarks { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionCommentReaction>
        StackExchangeQuestionCommentReactions { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionCommentVisitor> StackExchangeQuestionCommentVisitors { get; set; } =
    [
    ];

    public virtual ICollection<StackExchangeQuestionReaction> StackExchangeQuestionReactions { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionTag> StackExchangeQuestionTags { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionVisitor> StackExchangeQuestionVisitors { get; set; } = [];

    public virtual ICollection<Survey> Surveys { get; set; } = [];

    public virtual ICollection<SurveyBookmark> SurveyBookmarks { get; set; } = [];

    public virtual ICollection<SurveyComment> SurveyComments { get; set; } = [];

    public virtual ICollection<SurveyCommentBookmark> SurveyCommentBookmarks { get; set; } = [];

    public virtual ICollection<SurveyCommentReaction> SurveyCommentReactions { get; set; } = [];

    public virtual ICollection<SurveyCommentVisitor> SurveyCommentVisitors { get; set; } = [];

    public virtual ICollection<SurveyReaction> SurveyReactions { get; set; } = [];

    public virtual ICollection<SurveyTag> SurveyTags { get; set; } = [];

    public virtual ICollection<SurveyVisitor> SurveyVisitors { get; set; } = [];

    public virtual ICollection<AppSetting> AppSettings { get; set; } = [];

    public virtual ICollection<AdvertisementUserFile> AdvertisementUserFiles { get; set; } = [];

    public virtual ICollection<AdvertisementUserFileVisitor> AdvertisementUserFileVisitors { get; set; } = [];

    public virtual ICollection<BacklogUserFile> BacklogUserFiles { get; set; } = [];

    public virtual ICollection<BacklogUserFileVisitor> BacklogUserFileVisitors { get; set; } = [];

    public virtual ICollection<CourseTopicUserFileVisitor> CourseTopicUserFileVisitors { get; set; } = [];

    public virtual ICollection<CourseUserFile> CourseUserFiles { get; set; } = [];

    public virtual ICollection<CourseUserFileVisitor> CourseUserFileVisitors { get; set; } = [];

    public virtual ICollection<UserProfileUserFile> UserProfileUserFiles { get; set; } = [];

    public virtual ICollection<UserProfileUserFileVisitor> UserProfileUserFileVisitors { get; set; } = [];

    public virtual ICollection<DailyNewsItemUserFile> DailyNewsItemUserFiles { get; set; } = [];

    public virtual ICollection<DailyNewsItemUserFileVisitor> DailyNewsItemUserFileVisitors { get; set; } = [];

    public virtual ICollection<BlogPostUserFile> BlogPostUserFiles { get; set; } = [];

    public virtual ICollection<BlogPostUserFileVisitor> BlogPostUserFileVisitors { get; set; } = [];

    public virtual ICollection<PrivateMessageUserFile> PrivateMessageUserFiles { get; set; } = [];

    public virtual ICollection<PrivateMessageUserFileVisitor> PrivateMessageUserFileVisitors { get; set; } = [];

    public virtual ICollection<ProjectFaqUserFile> ProjectFaqUserFiles { get; set; } = [];

    public virtual ICollection<ProjectFaqUserFileVisitor> ProjectFaqUserFileVisitors { get; set; } = [];

    public virtual ICollection<ProjectIssueUserFile> ProjectIssueUserFiles { get; set; } = [];

    public virtual ICollection<ProjectIssueUserFileVisitor> ProjectIssueUserFileVisitors { get; set; } = [];

    public virtual ICollection<ProjectReleaseUserFile> ProjectReleaseUserFiles { get; set; } = [];

    public virtual ICollection<ProjectReleaseUserFileVisitor> ProjectReleaseUserFileVisitors { get; set; } = [];

    public virtual ICollection<ProjectUserFile> ProjectUserFiles { get; set; } = [];

    public virtual ICollection<ProjectUserFileVisitor> ProjectUserFileVisitors { get; set; } = [];

    public virtual ICollection<LearningPathUserFile> LearningPathUserFiles { get; set; } = [];

    public virtual ICollection<LearningPathUserFileVisitor> LearningPathUserFileVisitors { get; set; } = [];

    public virtual ICollection<SearchItemUserFile> SearchItemUserFiles { get; set; } = [];

    public virtual ICollection<SearchItemUserFileVisitor> SearchItemUserFileVisitors { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionUserFile> StackExchangeQuestionUserFiles { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionUserFileVisitor>
        StackExchangeQuestionUserFileVisitors { get; set; } = [];

    public virtual ICollection<SurveyUserFile> SurveyUserFiles { get; set; } = [];

    public virtual ICollection<SurveyUserFileVisitor> SurveyUserFileVisitors { get; set; } = [];

    public virtual ICollection<CourseQuestionUserFile> CourseQuestionUserFiles { get; set; } = [];

    public virtual ICollection<CourseQuestionUserFileVisitor> CourseQuestionUserFileVisitors { get; set; } = [];

    public virtual ICollection<CourseTopicUserFile> CourseTopicUserFiles { get; set; } = [];

    public virtual ICollection<UserProfileReaction> UserProfileReactionsForUsers { get; set; } = [];

    public virtual ICollection<UserProfileCommentReaction> UserProfileCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<SurveyReaction> SurveyReactionsForUsers { get; set; } = [];

    public virtual ICollection<SurveyCommentReaction> SurveyCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionReaction> StackExchangeQuestionReactionsForUsers { get; set; } = [];

    public virtual ICollection<StackExchangeQuestionCommentReaction> StackExchangeQuestionCommentReactionsForUsers
    {
        get;
        set;
    } = [];

    public virtual ICollection<SearchItemReaction> SearchItemReactionsForUsers { get; set; } = [];

    public virtual ICollection<SearchItemCommentReaction> SearchItemCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<LearningPathReaction> LearningPathReactionsForUsers { get; set; } = [];

    public virtual ICollection<LearningPathCommentReaction> LearningPathCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectReleaseReaction> ProjectReleaseReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectReleaseCommentReaction> ProjectReleaseCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectReaction> ProjectReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectIssueReaction> ProjectIssueReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectIssueCommentReaction> ProjectIssueCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectFaqReaction> ProjectFaqReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectFaqCommentReaction> ProjectFaqCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<ProjectCommentReaction> ProjectCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<PrivateMessageReaction> PrivateMessageReactionsForUsers { get; set; } = [];

    public virtual ICollection<PrivateMessageCommentReaction> PrivateMessageCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<BlogPostReaction> BlogPostReactionsForUsers { get; set; } = [];

    public virtual ICollection<BlogPostCommentReaction> BlogPostCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<DailyNewsItemReaction> DailyNewsItemReactionsForUsers { get; set; } = [];

    public virtual ICollection<DailyNewsItemCommentReaction> DailyNewsItemCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<CourseTopicReaction> CourseTopicReactionsForUsers { get; set; } = [];

    public virtual ICollection<CourseTopicCommentReaction> CourseTopicCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<CourseReaction> CourseReactionsForUsers { get; set; } = [];

    public virtual ICollection<CourseQuestionReaction> CourseQuestionReactionsForUsers { get; set; } = [];

    public virtual ICollection<CourseQuestionCommentReaction> CourseQuestionCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<CourseCommentReaction> CourseCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<BacklogReaction> BacklogReactionsForUsers { get; set; } = [];

    public virtual ICollection<BacklogCommentReaction> BacklogCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<AdvertisementReaction> AdvertisementReactionsForUsers { get; set; } = [];

    public virtual ICollection<AdvertisementCommentReaction> AdvertisementCommentReactionsForUsers { get; set; } = [];

    public virtual ICollection<SiteReferrer> SiteReferrers { get; set; } = [];

    public virtual ICollection<SiteUrl> SiteUrls { get; set; } = [];
}
