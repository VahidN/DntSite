using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class AddAccessEmailToAdminModel : BaseEmailModel
{
    public required string UserName { get; set; }

    public required string Title { get; set; }

    public required string Operation { get; set; }
}
