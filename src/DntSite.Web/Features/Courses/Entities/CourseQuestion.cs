using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Entities;

public class CourseQuestion : BaseInteractiveEntity<CourseQuestion, CourseQuestionVisitor, CourseQuestionBookmark,
    CourseQuestionReaction, CourseQuestionTag, CourseQuestionComment, CourseQuestionCommentVisitor,
    CourseQuestionCommentBookmark, CourseQuestionCommentReaction, CourseQuestionUserFile, CourseQuestionUserFileVisitor>
{
    public required string Title { set; get; }

    [MaxLength] public required string Description { set; get; }

    public virtual Course Course { set; get; } = null!;

    public int CourseId { set; get; }
}
