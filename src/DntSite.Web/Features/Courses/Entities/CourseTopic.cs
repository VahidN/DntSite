using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Entities;

public class CourseTopic : BaseInteractiveEntity<CourseTopic, CourseTopicVisitor, CourseTopicBookmark,
    CourseTopicReaction, CourseTopicTag, CourseTopicComment, CourseTopicCommentVisitor, CourseTopicCommentBookmark,
    CourseTopicCommentReaction, CourseTopicUserFile, CourseTopicUserFileVisitor>
{
    public Guid DisplayId { set; get; }

    public required string Title { set; get; }

    [MaxLength] public required string Body { set; get; }

    public int ReadingTimeMinutes { set; get; }

    public bool IsMainTopic { set; get; }

    public virtual Course Course { set; get; } = null!;

    public int CourseId { set; get; }
}
