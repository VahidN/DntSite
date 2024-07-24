using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Components;

public partial class CourseDetails
{
    [Parameter] [EditorRequired] public Course? Course { set; get; }
}
