using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseTopicModel
{
    public CourseTopic? ThisTopic { set; get; }

    public CourseTopic? NextTopic { set; get; }

    public CourseTopic? PreviousTopic { set; get; }

    public IList<CourseTopicComment> CommentsList { set; get; } = new List<CourseTopicComment>();
}
