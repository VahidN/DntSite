using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectFilesModel
{
    public Project? Project { set; get; }

    public IList<ProjectRelease> ProjectReleases { set; get; } = new List<ProjectRelease>();

    public ProjectPostFileModel? ProjectPostFile { set; get; }
}
