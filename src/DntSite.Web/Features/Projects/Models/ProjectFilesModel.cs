using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectFilesModel
{
    public Project? Project { set; get; }

    public IList<ProjectRelease> ProjectReleases { set; get; } = [];

    public ProjectPostFileModel? ProjectPostFile { set; get; }
}
