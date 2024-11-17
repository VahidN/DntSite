using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseSideMenuModel
{
    public Course? ThisCourse { set; get; }

    public IList<CourseQuestionComment> CourseQuestionComments { set; get; } = [];

    public IList<CourseTopicComment> CourseTopicComments { set; get; } = [];
}
