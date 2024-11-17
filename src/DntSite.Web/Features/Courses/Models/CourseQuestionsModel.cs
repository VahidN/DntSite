using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseQuestionsModel
{
    public IList<CourseQuestion> CourseQuestions { set; get; } = [];

    public Course? Course { set; get; }
}
