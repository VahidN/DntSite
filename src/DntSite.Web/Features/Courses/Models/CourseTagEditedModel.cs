using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class CourseTagEditedModel : BaseEmailModel
{
    public required string OldName { get; set; }

    public required string NewName { get; set; }
}
