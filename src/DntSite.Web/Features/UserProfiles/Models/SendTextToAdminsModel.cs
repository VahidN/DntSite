using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class SendTextToAdminsModel : BaseEmailModel
{
    public required string Text { get; set; }
}
