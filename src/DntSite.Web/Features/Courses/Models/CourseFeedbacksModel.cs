using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseFeedbacksModel
{
    public Course? Course { set; get; }

    public IList<CourseQuestionComment> CourseQuestionComments { set; get; } = new List<CourseQuestionComment>();

    public IList<CourseTopicComment> CourseTopicComments { set; get; } = new List<CourseTopicComment>();
}
