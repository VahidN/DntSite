using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectFaqEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Body { get; set; }

    public required string ProjectId { get; set; }

    public required string FaqId { get; set; }
}
