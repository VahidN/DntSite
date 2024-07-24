using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class NewCourseHelpEmailModel : BaseEmailModel
{
    public required string Id { get; set; }

    public required string Title { get; set; }
}
