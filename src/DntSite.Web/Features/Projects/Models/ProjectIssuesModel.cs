using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectIssuesModel
{
    public IList<ProjectIssue> ProjectIssues { set; get; } = [];

    public Project? Project { set; get; }
}
