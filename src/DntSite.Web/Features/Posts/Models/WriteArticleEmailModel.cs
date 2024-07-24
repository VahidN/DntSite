using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Posts.Models;

public class WriteArticleEmailModel : BaseEmailModel
{
    public required string Title { get; set; }

    public required string Body { get; set; }

    public required string PmId { get; set; }
}
