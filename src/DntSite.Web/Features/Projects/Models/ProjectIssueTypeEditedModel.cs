using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectIssueTypeEditedModel : BaseEmailModel
{
    public required string OldName { get; set; }

    public required string NewName { get; set; }
}
