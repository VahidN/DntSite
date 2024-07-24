using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Posts.Models;

public class NewDraftEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Body { get; set; }

    public required string IsReady { get; set; }

    public required string PmId { get; set; }
}
