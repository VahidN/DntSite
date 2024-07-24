using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Courses.Models;

public class CourseAllowedUsersModel
{
    public Course? ThisCourse { set; get; }

    public IList<User> AllowedUsers { set; get; } = new List<User>();

    [Display(Name = "نام مستعار کاربر:")]
    [Required(ErrorMessage = "نام کاربر خالی است")]
    public required string UserName { set; get; }

    [Required(ErrorMessage = "نام دوره خالی است")]
    public required string Pid { set; get; }
}
