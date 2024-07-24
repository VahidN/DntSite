using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseItemModel
{
    public Course? CurrentCourse { set; get; }

    public Course? NextCourse { set; get; }

    public Course? PreviousCourse { set; get; }
}
