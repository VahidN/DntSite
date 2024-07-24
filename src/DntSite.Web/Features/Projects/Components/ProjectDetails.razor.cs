using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectDetails
{
    [Parameter] public Project? Project { set; get; }
}
