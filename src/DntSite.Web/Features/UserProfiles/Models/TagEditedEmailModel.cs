using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class TagEditedEmailModel : BaseEmailModel
{
    public required string OldName { get; set; }

    public required string NewName { get; set; }
}
