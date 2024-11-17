using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectsSideMenuModel
{
    public int ProjectId { set; get; }

    public int NewProjectIssueStatusCount { set; get; }

    public IList<SimpleItemModel> IssueTypes { set; get; } = [];

    public IList<SimpleItemModel> IssueStatus { set; get; } = [];

    public ProjectsStatModel? ProjectStat { set; get; }
}
