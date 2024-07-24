using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.UserProfiles.Models;

public class RecycleEmailModel : BaseEmailModel
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }
}
