using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class ActivatedAccountModel : BaseEmailModel
{
    public required string Username { get; set; }
}
