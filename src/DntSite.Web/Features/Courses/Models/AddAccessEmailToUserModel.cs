using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class AddAccessEmailToUserModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Id { get; set; }
}
