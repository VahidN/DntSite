using DntSite.Web.Features.Projects.Entities;

namespace DntSite.Web.Features.Projects.Models;

public class FaqModel
{
    public ProjectFaq? CurrentItem { set; get; }

    public ProjectFaq? NextItem { set; get; }

    public ProjectFaq? PreviousItem { set; get; }
}
