using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectUrl
{
    [Parameter] public Project? Project { set; get; }
}
