using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class CommonFileEditedEmailModel : BaseEmailModel
{
    public required string Name { get; set; }

    public required string Description { get; set; }
}
