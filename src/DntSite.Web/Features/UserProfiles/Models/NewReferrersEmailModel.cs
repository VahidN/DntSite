using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class NewReferrersEmailModel : BaseEmailModel
{
    public required string Source { get; set; }

    public required string Dest { get; set; }

    public required string AdminUrl { get; set; }
}
