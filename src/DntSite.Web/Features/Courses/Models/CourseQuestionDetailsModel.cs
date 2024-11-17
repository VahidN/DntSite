using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseQuestionDetailsModel
{
    public CourseQuestion? CurrentItem { set; get; }

    public CourseQuestion? NextItem { set; get; }

    public CourseQuestion? PreviousItem { set; get; }

    public IList<CourseQuestionComment> CommentsList { set; get; } = [];
}
