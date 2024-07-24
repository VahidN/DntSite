using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectsModel
{
    public Project? CurrentItem { set; get; }

    public Project? NextItem { set; get; }

    public Project? PreviousItem { set; get; }
}
