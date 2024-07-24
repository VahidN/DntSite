using DntSite.Web.Features.Persistence.BaseDomainEntities.EfConfig;
using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Courses.Entities;

public class Course : BaseInteractiveEntity<Course, CourseVisitor, CourseBookmark, CourseReaction, CourseTag,
    CourseComment, CourseCommentVisitor, CourseCommentBookmark, CourseCommentReaction, CourseUserFile,
    CourseUserFileVisitor>
{
    public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }

    [MaxLength] public string? TopicsList { set; get; }

    [MaxLength] public string? Requirements { set; get; }

    [MaxLength] public string? HowToPay { set; get; }

    public bool IsFree { set; get; }

    public int NumberOfPostsRequired { set; get; }

    public int NumberOfMonthsRequired { set; get; }

    public int NumberOfTopics { set; get; }

    public int NumberOfHelperTopics { set; get; }

    [IgnoreAudit] public int NumberOfQuestions { set; get; }

    [IgnoreAudit] public int NumberOfQuestionsComments { set; get; }

    [IgnoreAudit] public int NumberOfComments { set; get; }

    public bool IsReadyToPublish { set; get; }

    public int NumberOfTotalRatingsRequired { set; get; }

    public int NumberOfMonthsTotalRatingsRequired { set; get; }

    public CourseType Perm { set; get; }

    public virtual ICollection<User> CourseAllowedUsers { set; get; } = new List<User>();

    public virtual ICollection<CourseTopic> CourseTopics { set; get; } = new List<CourseTopic>();

    public virtual ICollection<CourseQuestion> CourseQuestions { set; get; } = new List<CourseQuestion>();
}
