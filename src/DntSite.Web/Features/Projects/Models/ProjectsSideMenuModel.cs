using DntSite.Web.Features.Stats.Models;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectsSideMenuModel
{
    public int ProjectId { set; get; }

    public int NewProjectIssueStatusCount { set; get; }

    public IList<SimpleItemModel> IssueTypes { set; get; } = new List<SimpleItemModel>();

    public IList<SimpleItemModel> IssueStatus { set; get; } = new List<SimpleItemModel>();

    public ProjectsStatModel? ProjectStat { set; get; }
}
