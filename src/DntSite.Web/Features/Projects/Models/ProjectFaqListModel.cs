using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class ProjectFaqListModel
{
    public IList<ProjectFaq> ProjectFaqs { set; get; } = new List<ProjectFaq>();

    public Project? Project { set; get; }

    public ProjectFaqFormModel? FormData { set; get; }
}
