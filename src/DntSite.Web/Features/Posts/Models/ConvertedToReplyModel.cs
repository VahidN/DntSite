using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Posts.Models;

public class ConvertedToReplyModel : BaseEmailModel
{
    public required string PostId { get; set; }
}
