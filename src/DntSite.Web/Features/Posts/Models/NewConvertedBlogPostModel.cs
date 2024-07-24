using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.Posts.Models;

public class NewConvertedBlogPostModel : BaseEmailModel
{
    public required string Source { get; set; }

    public required string Dest { get; set; }
}
