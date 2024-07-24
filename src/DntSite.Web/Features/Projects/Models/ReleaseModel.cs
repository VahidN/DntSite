using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class ReleaseModel
{
    public ProjectRelease? CurrentItem { set; get; }

    public ProjectRelease? NextItem { set; get; }

    public ProjectRelease? PreviousItem { set; get; }
}
