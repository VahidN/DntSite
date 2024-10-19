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

    public virtual ICollection<Role> Roles { set; get; } = new List<Role>();

    public virtual ICollection<Role> CreatedRoles { set; get; } = new List<Role>();

    public virtual ICollection<PrivateMessage> SentPrivateMessages { set; get; } = new List<PrivateMessage>();

    public virtual ICollection<PrivateMessage> ReceivedPrivateMessages { set; get; } = new List<PrivateMessage>();

    public virtual ICollection<Advertisement> Advertisements { set; get; } = new List<Advertisement>();

    public virtual ICollection<AdvertisementComment> AdvertisementComments { set; get; } =
        new List<AdvertisementComment>();

    public virtual ICollection<Course> UserAllowedCourses { set; get; } = new List<Course>();

    public virtual ICollection<Backlog> Backlogs { set; get; } = new List<Backlog>();

    public virtual ICollection<SurveyItem> SurveyItems { set; get; } = new List<SurveyItem>();

    public virtual ICollection<UserUsedPassword> UserUsedPasswords { get; set; } = new List<UserUsedPassword>();

    public virtual ICollection<AdvertisementTag> AdvertisementTags { get; set; } = new List<AdvertisementTag>();

    public virtual ICollection<AdvertisementVisitor> AdvertisementVisitors { get; set; } =
        new List<AdvertisementVisitor>();

    public virtual ICollection<AppDataProtectionKey> AppDataProtectionKeys { get; set; } =
        new List<AppDataProtectionKey>();

    public virtual ICollection<AppLogItem> AppLogItems { get; set; } = new List<AppLogItem>();

    public virtual ICollection<AdvertisementCommentVisitor> AdvertisementCommentVisitors { get; set; } =
        new List<AdvertisementCommentVisitor>();

    public virtual ICollection<AdvertisementBookmark> AdvertisementBookmarks { get; set; } =
        new List<AdvertisementBookmark>();

    public virtual ICollection<AdvertisementCommentBookmark> AdvertisementCommentBookmarks { get; set; } =
        new List<AdvertisementCommentBookmark>();

    public virtual ICollection<AdvertisementCommentReaction> AdvertisementCommentReactions { get; set; } =
        new List<AdvertisementCommentReaction>();

    public virtual ICollection<AdvertisementReaction> AdvertisementReactions { get; set; } =
        new List<AdvertisementReaction>();

    public virtual ICollection<Backlog> DoneBacklogs { get; set; } = new List<Backlog>();

    public virtual ICollection<BacklogBookmark> BacklogBookmarks { get; set; } = new List<BacklogBookmark>();

    public virtual ICollection<BacklogVisitor> BacklogVisitors { get; set; } = new List<BacklogVisitor>();

    public virtual ICollection<BacklogTag> BacklogTags { get; set; } = new List<BacklogTag>();

    public virtual ICollection<BacklogReaction> BacklogReactions { get; set; } = new List<BacklogReaction>();

    public virtual ICollection<BacklogCommentVisitor> BacklogCommentVisitors { get; set; } =
        new List<BacklogCommentVisitor>();

    public virtual ICollection<BacklogCommentReaction> BacklogCommentReactions { get; set; } =
        new List<BacklogCommentReaction>();

    public virtual ICollection<BacklogComment> BacklogComments { get; set; } = new List<BacklogComment>();

    public virtual ICollection<BacklogCommentBookmark> BacklogCommentBookmarks { get; set; } =
        new List<BacklogCommentBookmark>();

    public virtual ICollection<CourseBookmark> CourseBookmarks { get; set; } = new List<CourseBookmark>();

    public virtual ICollection<CourseCommentBookmark> CourseCommentBookmarks { get; set; } =
        new List<CourseCommentBookmark>();

    public virtual ICollection<CourseComment> CourseComments { get; set; } = new List<CourseComment>();

    public virtual ICollection<CourseCommentReaction> CourseCommentReactions { get; set; } =
        new List<CourseCommentReaction>();

    public virtual ICollection<CourseCommentVisitor> CourseCommentVisitors { get; set; } =
        new List<CourseCommentVisitor>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<CourseQuestionBookmark> CourseQuestionBookmarks { get; set; } =
        new List<CourseQuestionBookmark>();

    public virtual ICollection<CourseQuestionCommentBookmark> CourseQuestionCommentBookmarks { get; set; } =
        new List<CourseQuestionCommentBookmark>();

    public virtual ICollection<CourseQuestionComment> CourseQuestionComments { get; set; } =
        new List<CourseQuestionComment>();

    public virtual ICollection<CourseQuestionCommentReaction> CourseQuestionCommentReactions { get; set; } =
        new List<CourseQuestionCommentReaction>();

    public virtual ICollection<CourseQuestionCommentVisitor> CourseQuestionCommentVisitors { get; set; } =
        new List<CourseQuestionCommentVisitor>();

    public virtual ICollection<CourseQuestion> CourseQuestions { get; set; } = new List<CourseQuestion>();

    public virtual ICollection<CourseQuestionReaction> CourseQuestionReactions { get; set; } =
        new List<CourseQuestionReaction>();

    public virtual ICollection<CourseQuestionTag> CourseQuestionTags { get; set; } = new List<CourseQuestionTag>();

    public virtual ICollection<CourseQuestionVisitor> CourseQuestionVisitors { get; set; } =
        new List<CourseQuestionVisitor>();

    public virtual ICollection<CourseReaction> CourseReactions { get; set; } = new List<CourseReaction>();

    public virtual ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();

    public virtual ICollection<CourseTopicBookmark> CourseTopicBookmarks { get; set; } =
        new List<CourseTopicBookmark>();

    public virtual ICollection<CourseTopicCommentBookmark> CourseTopicCommentBookmarks { get; set; } =
        new List<CourseTopicCommentBookmark>();

    public virtual ICollection<CourseTopicComment> CourseTopicComments { get; set; } = new List<CourseTopicComment>();

    public virtual ICollection<CourseTopicCommentReaction> CourseTopicCommentReactions { get; set; } =
        new List<CourseTopicCommentReaction>();

    public virtual ICollection<CourseTopicCommentVisitor> CourseTopicCommentVisitors { get; set; } =
        new List<CourseTopicCommentVisitor>();

    public virtual ICollection<CourseTopic> CourseTopics { get; set; } = new List<CourseTopic>();

    public virtual ICollection<CourseTopicReaction> CourseTopicReactions { get; set; } =
        new List<CourseTopicReaction>();

    public virtual ICollection<CourseTopicTag> CourseTopicTags { get; set; } = new List<CourseTopicTag>();

    public virtual ICollection<CourseTopicVisitor> CourseTopicVisitors { get; set; } = new List<CourseTopicVisitor>();

    public virtual ICollection<CourseVisitor> CourseVisitors { get; set; } = new List<CourseVisitor>();

    public virtual ICollection<UserProfileBookmark> UserBookmarks { get; set; } = new List<UserProfileBookmark>();

    public virtual ICollection<UserProfileCommentBookmark> UserCommentBookmarks { get; set; } =
        new List<UserProfileCommentBookmark>();

    public virtual ICollection<UserProfileComment> UserComments { get; set; } = new List<UserProfileComment>();

    public virtual ICollection<UserProfileCommentReaction> UserCommentReactions { get; set; } =
        new List<UserProfileCommentReaction>();

    public virtual ICollection<UserProfileCommentVisitor> UserCommentVisitors { get; set; } =
        new List<UserProfileCommentVisitor>();

    public virtual ICollection<UserProfileReaction> UserReactions { get; set; } = new List<UserProfileReaction>();

    public virtual ICollection<UserProfileVisitor> UserVisitors { get; set; } = new List<UserProfileVisitor>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<DailyNewsItem> DailyNewsItems { get; set; } = new List<DailyNewsItem>();

    public virtual ICollection<DailyNewsItemBookmark> DailyNewsItemBookmarks { get; set; } =
        new List<DailyNewsItemBookmark>();

    public virtual ICollection<DailyNewsItemComment> DailyNewsItemComments { get; set; } =
        new List<DailyNewsItemComment>();

    public virtual ICollection<DailyNewsItemCommentBookmark> DailyNewsItemCommentBookmarks { get; set; } =
        new List<DailyNewsItemCommentBookmark>();

    public virtual ICollection<DailyNewsItemCommentReaction> DailyNewsItemCommentReactions { get; set; } =
        new List<DailyNewsItemCommentReaction>();

    public virtual ICollection<DailyNewsItemCommentVisitor> DailyNewsItemCommentVisitors { get; set; } =
        new List<DailyNewsItemCommentVisitor>();

    public virtual ICollection<DailyNewsItemReaction> DailyNewsItemReactions { get; set; } =
        new List<DailyNewsItemReaction>();

    public virtual ICollection<DailyNewsItemTag> DailyNewsItemTags { get; set; } = new List<DailyNewsItemTag>();

    public virtual ICollection<DailyNewsItemVisitor> DailyNewsItemVisitors { get; set; } =
        new List<DailyNewsItemVisitor>();

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    public virtual ICollection<BlogPostBookmark> BlogPostBookmarks { get; set; } = new List<BlogPostBookmark>();

    public virtual ICollection<BlogPostComment> BlogPostComments { get; set; } = new List<BlogPostComment>();

    public virtual ICollection<BlogPostCommentBookmark> BlogPostCommentBookmarks { get; set; } =
        new List<BlogPostCommentBookmark>();

    public virtual ICollection<BlogPostCommentReaction> BlogPostCommentReactions { get; set; } =
        new List<BlogPostCommentReaction>();

    public virtual ICollection<BlogPostCommentVisitor> BlogPostCommentVisitors { get; set; } =
        new List<BlogPostCommentVisitor>();

    public virtual ICollection<BlogPostDraft> BlogPostDrafts { get; set; } = new List<BlogPostDraft>();

    public virtual ICollection<BlogPostReaction> BlogPostReactions { get; set; } = new List<BlogPostReaction>();

    public virtual ICollection<BlogPostTag> BlogPostTags { get; set; } = new List<BlogPostTag>();

    public virtual ICollection<BlogPostVisitor> BlogPostVisitors { get; set; } = new List<BlogPostVisitor>();

    public virtual ICollection<MassEmail> MassEmails { get; set; } = new List<MassEmail>();

    public virtual ICollection<PrivateMessageBookmark> PrivateMessageBookmarks { get; set; } =
        new List<PrivateMessageBookmark>();

    public virtual ICollection<PrivateMessageComment> PrivateMessageComments { get; set; } =
        new List<PrivateMessageComment>();

    public virtual ICollection<PrivateMessageCommentBookmark> PrivateMessageCommentBookmarks { get; set; } =
        new List<PrivateMessageCommentBookmark>();

    public virtual ICollection<PrivateMessageCommentReaction> PrivateMessageCommentReactions { get; set; } =
        new List<PrivateMessageCommentReaction>();

    public virtual ICollection<PrivateMessageCommentVisitor> PrivateMessageCommentVisitors { get; set; } =
        new List<PrivateMessageCommentVisitor>();

    public virtual ICollection<PrivateMessageReaction> PrivateMessageReactions { get; set; } =
        new List<PrivateMessageReaction>();

    public virtual ICollection<PrivateMessageTag> PrivateMessageTags { get; set; } = new List<PrivateMessageTag>();

    public virtual ICollection<PrivateMessageVisitor> PrivateMessageVisitors { get; set; } =
        new List<PrivateMessageVisitor>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<ProjectBookmark> ProjectBookmarks { get; set; } = new List<ProjectBookmark>();

    public virtual ICollection<ProjectComment> ProjectComments { get; set; } = new List<ProjectComment>();

    public virtual ICollection<ProjectCommentBookmark> ProjectCommentBookmarks { get; set; } =
        new List<ProjectCommentBookmark>();

    public virtual ICollection<ProjectCommentReaction> ProjectCommentReactions { get; set; } =
        new List<ProjectCommentReaction>();

    public virtual ICollection<ProjectCommentVisitor> ProjectCommentVisitors { get; set; } =
        new List<ProjectCommentVisitor>();

    public virtual ICollection<ProjectFaq> ProjectFaqs { get; set; } = new List<ProjectFaq>();

    public virtual ICollection<ProjectFaqBookmark> ProjectFaqBookmarks { get; set; } = new List<ProjectFaqBookmark>();

    public virtual ICollection<ProjectFaqComment> ProjectFaqComments { get; set; } = new List<ProjectFaqComment>();

    public virtual ICollection<ProjectFaqCommentBookmark> ProjectFaqCommentBookmarks { get; set; } =
        new List<ProjectFaqCommentBookmark>();

    public virtual ICollection<ProjectFaqCommentReaction> ProjectFaqCommentReactions { get; set; } =
        new List<ProjectFaqCommentReaction>();

    public virtual ICollection<ProjectFaqCommentVisitor> ProjectFaqCommentVisitors { get; set; } =
        new List<ProjectFaqCommentVisitor>();

    public virtual ICollection<ProjectFaqReaction> ProjectFaqReactions { get; set; } = new List<ProjectFaqReaction>();

    public virtual ICollection<ProjectFaqTag> ProjectFaqTags { get; set; } = new List<ProjectFaqTag>();

    public virtual ICollection<ProjectFaqVisitor> ProjectFaqVisitors { get; set; } = new List<ProjectFaqVisitor>();

    public virtual ICollection<ProjectIssue> ProjectIssues { get; set; } = new List<ProjectIssue>();

    public virtual ICollection<ProjectIssueBookmark> ProjectIssueBookmarks { get; set; } =
        new List<ProjectIssueBookmark>();

    public virtual ICollection<ProjectIssueComment> ProjectIssueComments { get; set; } =
        new List<ProjectIssueComment>();

    public virtual ICollection<ProjectIssueCommentBookmark> ProjectIssueCommentBookmarks { get; set; } =
        new List<ProjectIssueCommentBookmark>();

    public virtual ICollection<ProjectIssueCommentReaction> ProjectIssueCommentReactions { get; set; } =
        new List<ProjectIssueCommentReaction>();

    public virtual ICollection<ProjectIssueCommentVisitor> ProjectIssueCommentVisitors { get; set; } =
        new List<ProjectIssueCommentVisitor>();

    public virtual ICollection<ProjectIssuePriority> ProjectIssuePriorities { get; set; } =
        new List<ProjectIssuePriority>();

    public virtual ICollection<ProjectIssueReaction> ProjectIssueReactions { get; set; } =
        new List<ProjectIssueReaction>();

    public virtual ICollection<ProjectIssueStatus> ProjectIssueStatus { get; set; } = new List<ProjectIssueStatus>();

    public virtual ICollection<ProjectIssueTag> ProjectIssueTags { get; set; } = new List<ProjectIssueTag>();

    public virtual ICollection<ProjectIssueType> ProjectIssueTypes { get; set; } = new List<ProjectIssueType>();

    public virtual ICollection<ProjectIssueVisitor> ProjectIssueVisitors { get; set; } =
        new List<ProjectIssueVisitor>();

    public virtual ICollection<ProjectReaction> ProjectReactions { get; set; } = new List<ProjectReaction>();

    public virtual ICollection<ProjectRelease> ProjectReleases { get; set; } = new List<ProjectRelease>();

    public virtual ICollection<ProjectReleaseBookmark> ProjectReleaseBookmarks { get; set; } =
        new List<ProjectReleaseBookmark>();

    public virtual ICollection<ProjectReleaseComment> ProjectReleaseComments { get; set; } =
        new List<ProjectReleaseComment>();

    public virtual ICollection<ProjectReleaseCommentBookmark> ProjectReleaseCommentBookmarks { get; set; } =
        new List<ProjectReleaseCommentBookmark>();

    public virtual ICollection<ProjectReleaseCommentReaction> ProjectReleaseCommentReactions { get; set; } =
        new List<ProjectReleaseCommentReaction>();

    public virtual ICollection<ProjectReleaseCommentVisitor> ProjectReleaseCommentVisitors { get; set; } =
        new List<ProjectReleaseCommentVisitor>();

    public virtual ICollection<ProjectReleaseReaction> ProjectReleaseReactions { get; set; } =
        new List<ProjectReleaseReaction>();

    public virtual ICollection<ProjectReleaseTag> ProjectReleaseTags { get; set; } = new List<ProjectReleaseTag>();

    public virtual ICollection<ProjectReleaseVisitor> ProjectReleaseVisitors { get; set; } =
        new List<ProjectReleaseVisitor>();

    public virtual ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();

    public virtual ICollection<ProjectVisitor> ProjectVisitors { get; set; } = new List<ProjectVisitor>();

    public virtual ICollection<LearningPath> LearningPaths { get; set; } = new List<LearningPath>();

    public virtual ICollection<LearningPathBookmark> LearningPathBookmarks { get; set; } =
        new List<LearningPathBookmark>();

    public virtual ICollection<LearningPathComment> LearningPathComments { get; set; } =
        new List<LearningPathComment>();

    public virtual ICollection<LearningPathCommentBookmark> LearningPathCommentBookmarks { get; set; } =
        new List<LearningPathCommentBookmark>();

    public virtual ICollection<LearningPathCommentReaction> LearningPathCommentReactions { get; set; } =
        new List<LearningPathCommentReaction>();

    public virtual ICollection<LearningPathCommentVisitor> LearningPathCommentVisitors { get; set; } =
        new List<LearningPathCommentVisitor>();

    public virtual ICollection<LearningPathReaction> LearningPathReactions { get; set; } =
        new List<LearningPathReaction>();

    public virtual ICollection<LearningPathTag> LearningPathTags { get; set; } = new List<LearningPathTag>();

    public virtual ICollection<LearningPathVisitor> LearningPathVisitors { get; set; } =
        new List<LearningPathVisitor>();

    public virtual ICollection<SearchItem> SearchItems { get; set; } = new List<SearchItem>();

    public virtual ICollection<SearchItemBookmark> SearchItemBookmarks { get; set; } = new List<SearchItemBookmark>();

    public virtual ICollection<SearchItemComment> SearchItemComments { get; set; } = new List<SearchItemComment>();

    public virtual ICollection<SearchItemCommentBookmark> SearchItemCommentBookmarks { get; set; } =
        new List<SearchItemCommentBookmark>();

    public virtual ICollection<SearchItemCommentReaction> SearchItemCommentReactions { get; set; } =
        new List<SearchItemCommentReaction>();

    public virtual ICollection<SearchItemCommentVisitor> SearchItemCommentVisitors { get; set; } =
        new List<SearchItemCommentVisitor>();

    public virtual ICollection<SearchItemReaction> SearchItemReactions { get; set; } = new List<SearchItemReaction>();

    public virtual ICollection<SearchItemTag> SearchItemTags { get; set; } = new List<SearchItemTag>();

    public virtual ICollection<SearchItemVisitor> SearchItemVisitors { get; set; } = new List<SearchItemVisitor>();

    public virtual ICollection<CustomSidebar> CustomSidebars { get; set; } = new List<CustomSidebar>();

    public virtual ICollection<StackExchangeQuestion> StackExchangeQuestions { get; set; } =
        new List<StackExchangeQuestion>();

    public virtual ICollection<StackExchangeQuestionBookmark> StackExchangeQuestionBookmarks { get; set; } =
        new List<StackExchangeQuestionBookmark>();

    public virtual ICollection<StackExchangeQuestionComment> StackExchangeQuestionComments { get; set; } =
        new List<StackExchangeQuestionComment>();

    public virtual ICollection<StackExchangeQuestionCommentBookmark>
        StackExchangeQuestionCommentBookmarks { get; set; } = new List<StackExchangeQuestionCommentBookmark>();

    public virtual ICollection<StackExchangeQuestionCommentReaction>
        StackExchangeQuestionCommentReactions { get; set; } = new List<StackExchangeQuestionCommentReaction>();

    public virtual ICollection<StackExchangeQuestionCommentVisitor> StackExchangeQuestionCommentVisitors { get; set; } =
        new List<StackExchangeQuestionCommentVisitor>();

    public virtual ICollection<StackExchangeQuestionReaction> StackExchangeQuestionReactions { get; set; } =
        new List<StackExchangeQuestionReaction>();

    public virtual ICollection<StackExchangeQuestionTag> StackExchangeQuestionTags { get; set; } =
        new List<StackExchangeQuestionTag>();

    public virtual ICollection<StackExchangeQuestionVisitor> StackExchangeQuestionVisitors { get; set; } =
        new List<StackExchangeQuestionVisitor>();

    public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();

    public virtual ICollection<SurveyBookmark> SurveyBookmarks { get; set; } = new List<SurveyBookmark>();

    public virtual ICollection<SurveyComment> SurveyComments { get; set; } = new List<SurveyComment>();

    public virtual ICollection<SurveyCommentBookmark> SurveyCommentBookmarks { get; set; } =
        new List<SurveyCommentBookmark>();

    public virtual ICollection<SurveyCommentReaction> SurveyCommentReactions { get; set; } =
        new List<SurveyCommentReaction>();

    public virtual ICollection<SurveyCommentVisitor> SurveyCommentVisitors { get; set; } =
        new List<SurveyCommentVisitor>();

    public virtual ICollection<SurveyReaction> SurveyReactions { get; set; } = new List<SurveyReaction>();

    public virtual ICollection<SurveyTag> SurveyTags { get; set; } = new List<SurveyTag>();

    public virtual ICollection<SurveyVisitor> SurveyVisitors { get; set; } = new List<SurveyVisitor>();

    public virtual ICollection<AppSetting> AppSettings { get; set; } = new List<AppSetting>();

    public virtual ICollection<AdvertisementUserFile> AdvertisementUserFiles { get; set; } =
        new List<AdvertisementUserFile>();

    public virtual ICollection<AdvertisementUserFileVisitor> AdvertisementUserFileVisitors { get; set; } =
        new List<AdvertisementUserFileVisitor>();

    public virtual ICollection<BacklogUserFile> BacklogUserFiles { get; set; } = new List<BacklogUserFile>();

    public virtual ICollection<BacklogUserFileVisitor> BacklogUserFileVisitors { get; set; } =
        new List<BacklogUserFileVisitor>();

    public virtual ICollection<CourseTopicUserFileVisitor> CourseTopicUserFileVisitors { get; set; } =
        new List<CourseTopicUserFileVisitor>();

    public virtual ICollection<CourseUserFile> CourseUserFiles { get; set; } = new List<CourseUserFile>();

    public virtual ICollection<CourseUserFileVisitor> CourseUserFileVisitors { get; set; } =
        new List<CourseUserFileVisitor>();

    public virtual ICollection<UserProfileUserFile> UserProfileUserFiles { get; set; } =
        new List<UserProfileUserFile>();

    public virtual ICollection<UserProfileUserFileVisitor> UserProfileUserFileVisitors { get; set; } =
        new List<UserProfileUserFileVisitor>();

    public virtual ICollection<DailyNewsItemUserFile> DailyNewsItemUserFiles { get; set; } =
        new List<DailyNewsItemUserFile>();

    public virtual ICollection<DailyNewsItemUserFileVisitor> DailyNewsItemUserFileVisitors { get; set; } =
        new List<DailyNewsItemUserFileVisitor>();

    public virtual ICollection<BlogPostUserFile> BlogPostUserFiles { get; set; } = new List<BlogPostUserFile>();

    public virtual ICollection<BlogPostUserFileVisitor> BlogPostUserFileVisitors { get; set; } =
        new List<BlogPostUserFileVisitor>();

    public virtual ICollection<PrivateMessageUserFile> PrivateMessageUserFiles { get; set; } =
        new List<PrivateMessageUserFile>();

    public virtual ICollection<PrivateMessageUserFileVisitor> PrivateMessageUserFileVisitors { get; set; } =
        new List<PrivateMessageUserFileVisitor>();

    public virtual ICollection<ProjectFaqUserFile> ProjectFaqUserFiles { get; set; } = new List<ProjectFaqUserFile>();

    public virtual ICollection<ProjectFaqUserFileVisitor> ProjectFaqUserFileVisitors { get; set; } =
        new List<ProjectFaqUserFileVisitor>();

    public virtual ICollection<ProjectIssueUserFile> ProjectIssueUserFiles { get; set; } =
        new List<ProjectIssueUserFile>();

    public virtual ICollection<ProjectIssueUserFileVisitor> ProjectIssueUserFileVisitors { get; set; } =
        new List<ProjectIssueUserFileVisitor>();

    public virtual ICollection<ProjectReleaseUserFile> ProjectReleaseUserFiles { get; set; } =
        new List<ProjectReleaseUserFile>();

    public virtual ICollection<ProjectReleaseUserFileVisitor> ProjectReleaseUserFileVisitors { get; set; } =
        new List<ProjectReleaseUserFileVisitor>();

    public virtual ICollection<ProjectUserFile> ProjectUserFiles { get; set; } = new List<ProjectUserFile>();

    public virtual ICollection<ProjectUserFileVisitor> ProjectUserFileVisitors { get; set; } =
        new List<ProjectUserFileVisitor>();

    public virtual ICollection<LearningPathUserFile> LearningPathUserFiles { get; set; } =
        new List<LearningPathUserFile>();

    public virtual ICollection<LearningPathUserFileVisitor> LearningPathUserFileVisitors { get; set; } =
        new List<LearningPathUserFileVisitor>();

    public virtual ICollection<SearchItemUserFile> SearchItemUserFiles { get; set; } = new List<SearchItemUserFile>();

    public virtual ICollection<SearchItemUserFileVisitor> SearchItemUserFileVisitors { get; set; } =
        new List<SearchItemUserFileVisitor>();

    public virtual ICollection<StackExchangeQuestionUserFile> StackExchangeQuestionUserFiles { get; set; } =
        new List<StackExchangeQuestionUserFile>();

    public virtual ICollection<StackExchangeQuestionUserFileVisitor>
        StackExchangeQuestionUserFileVisitors { get; set; } = new List<StackExchangeQuestionUserFileVisitor>();

    public virtual ICollection<SurveyUserFile> SurveyUserFiles { get; set; } = new List<SurveyUserFile>();

    public virtual ICollection<SurveyUserFileVisitor> SurveyUserFileVisitors { get; set; } =
        new List<SurveyUserFileVisitor>();

    public virtual ICollection<CourseQuestionUserFile> CourseQuestionUserFiles { get; set; } =
        new List<CourseQuestionUserFile>();

    public virtual ICollection<CourseQuestionUserFileVisitor> CourseQuestionUserFileVisitors { get; set; } =
        new List<CourseQuestionUserFileVisitor>();

    public virtual ICollection<CourseTopicUserFile> CourseTopicUserFiles { get; set; } =
        new List<CourseTopicUserFile>();

    public virtual ICollection<UserProfileReaction> UserProfileReactionsForUsers { get; set; } =
        new List<UserProfileReaction>();

    public virtual ICollection<UserProfileCommentReaction> UserProfileCommentReactionsForUsers { get; set; } =
        new List<UserProfileCommentReaction>();

    public virtual ICollection<SurveyReaction> SurveyReactionsForUsers { get; set; } = new List<SurveyReaction>();

    public virtual ICollection<SurveyCommentReaction> SurveyCommentReactionsForUsers { get; set; } =
        new List<SurveyCommentReaction>();

    public virtual ICollection<StackExchangeQuestionReaction> StackExchangeQuestionReactionsForUsers { get; set; } =
        new List<StackExchangeQuestionReaction>();

    public virtual ICollection<StackExchangeQuestionCommentReaction> StackExchangeQuestionCommentReactionsForUsers
    {
        get;
        set;
    } = new List<StackExchangeQuestionCommentReaction>();

    public virtual ICollection<SearchItemReaction> SearchItemReactionsForUsers { get; set; } =
        new List<SearchItemReaction>();

    public virtual ICollection<SearchItemCommentReaction> SearchItemCommentReactionsForUsers { get; set; } =
        new List<SearchItemCommentReaction>();

    public virtual ICollection<LearningPathReaction> LearningPathReactionsForUsers { get; set; } =
        new List<LearningPathReaction>();

    public virtual ICollection<LearningPathCommentReaction> LearningPathCommentReactionsForUsers { get; set; } =
        new List<LearningPathCommentReaction>();

    public virtual ICollection<ProjectReleaseReaction> ProjectReleaseReactionsForUsers { get; set; } =
        new List<ProjectReleaseReaction>();

    public virtual ICollection<ProjectReleaseCommentReaction> ProjectReleaseCommentReactionsForUsers { get; set; } =
        new List<ProjectReleaseCommentReaction>();

    public virtual ICollection<ProjectReaction> ProjectReactionsForUsers { get; set; } = new List<ProjectReaction>();

    public virtual ICollection<ProjectIssueReaction> ProjectIssueReactionsForUsers { get; set; } =
        new List<ProjectIssueReaction>();

    public virtual ICollection<ProjectIssueCommentReaction> ProjectIssueCommentReactionsForUsers { get; set; } =
        new List<ProjectIssueCommentReaction>();

    public virtual ICollection<ProjectFaqReaction> ProjectFaqReactionsForUsers { get; set; } =
        new List<ProjectFaqReaction>();

    public virtual ICollection<ProjectFaqCommentReaction> ProjectFaqCommentReactionsForUsers { get; set; } =
        new List<ProjectFaqCommentReaction>();

    public virtual ICollection<ProjectCommentReaction> ProjectCommentReactionsForUsers { get; set; } =
        new List<ProjectCommentReaction>();

    public virtual ICollection<PrivateMessageReaction> PrivateMessageReactionsForUsers { get; set; } =
        new List<PrivateMessageReaction>();

    public virtual ICollection<PrivateMessageCommentReaction> PrivateMessageCommentReactionsForUsers { get; set; } =
        new List<PrivateMessageCommentReaction>();

    public virtual ICollection<BlogPostReaction> BlogPostReactionsForUsers { get; set; } = new List<BlogPostReaction>();

    public virtual ICollection<BlogPostCommentReaction> BlogPostCommentReactionsForUsers { get; set; } =
        new List<BlogPostCommentReaction>();

    public virtual ICollection<DailyNewsItemReaction> DailyNewsItemReactionsForUsers { get; set; } =
        new List<DailyNewsItemReaction>();

    public virtual ICollection<DailyNewsItemCommentReaction> DailyNewsItemCommentReactionsForUsers { get; set; } =
        new List<DailyNewsItemCommentReaction>();

    public virtual ICollection<CourseTopicReaction> CourseTopicReactionsForUsers { get; set; } =
        new List<CourseTopicReaction>();

    public virtual ICollection<CourseTopicCommentReaction> CourseTopicCommentReactionsForUsers { get; set; } =
        new List<CourseTopicCommentReaction>();

    public virtual ICollection<CourseReaction> CourseReactionsForUsers { get; set; } = new List<CourseReaction>();

    public virtual ICollection<CourseQuestionReaction> CourseQuestionReactionsForUsers { get; set; } =
        new List<CourseQuestionReaction>();

    public virtual ICollection<CourseQuestionCommentReaction> CourseQuestionCommentReactionsForUsers { get; set; } =
        new List<CourseQuestionCommentReaction>();

    public virtual ICollection<CourseCommentReaction> CourseCommentReactionsForUsers { get; set; } =
        new List<CourseCommentReaction>();

    public virtual ICollection<BacklogReaction> BacklogReactionsForUsers { get; set; } = new List<BacklogReaction>();

    public virtual ICollection<BacklogCommentReaction> BacklogCommentReactionsForUsers { get; set; } =
        new List<BacklogCommentReaction>();

    public virtual ICollection<AdvertisementReaction> AdvertisementReactionsForUsers { get; set; } =
        new List<AdvertisementReaction>();

    public virtual ICollection<AdvertisementCommentReaction> AdvertisementCommentReactionsForUsers { get; set; } =
        new List<AdvertisementCommentReaction>();

    public virtual ICollection<SiteReferrer> SiteReferrers { get; set; } = new List<SiteReferrer>();

    public virtual ICollection<SiteUrl> SiteUrls { get; set; } = new List<SiteUrl>();
}
