using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Entities;

public class CourseTopicComment : BaseCommentsEntity<CourseTopicComment, CourseTopic, CourseTopicCommentVisitor,
    CourseTopicCommentBookmark, CourseTopicCommentReaction>
{
}
