using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Courses.Entities;

public class CourseComment : BaseCommentsEntity<CourseComment, Course, CourseCommentVisitor, CourseCommentBookmark,
    CourseCommentReaction>;
