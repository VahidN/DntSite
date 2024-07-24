using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Projects.Models;

public class NewProjectHelpEmailModel : BaseEmailModel
{
    public required string Id { get; set; }

    public required string Title { get; set; }
}
