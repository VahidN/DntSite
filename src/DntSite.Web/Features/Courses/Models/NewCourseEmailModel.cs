using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Courses.Models;

public class NewCourseEmailModel : BaseEmailModel
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string HowToPay { get; set; }

    public required string Requirements { get; set; }

    public required string TopicsList { get; set; }
}
