using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseTopicsModel
{
    public int Cid { set; get; }

    public IList<CourseTopic> CourseTopics { set; get; } = [];
}
