using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Projects.Models;

public class ApplyIssueStatusEmailModel : BaseEmailModel
{
    public required string ProjectId { get; set; }

    public required string IssueId { get; set; }

    public required string IssueTitle { get; set; }

    public required string ProjectName { get; set; }

    public required string StatusName { get; set; }
}
