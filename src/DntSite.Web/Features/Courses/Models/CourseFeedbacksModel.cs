using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseFeedbacksModel
{
    public Course? Course { set; get; }

    public IList<CourseQuestionComment> CourseQuestionComments { set; get; } = [];

    public IList<CourseTopicComment> CourseTopicComments { set; get; } = [];
}
