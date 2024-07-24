using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Entities;

public class CourseQuestionComment : BaseCommentsEntity<CourseQuestionComment, CourseQuestion,
    CourseQuestionCommentVisitor, CourseQuestionCommentBookmark, CourseQuestionCommentReaction>
{
}
