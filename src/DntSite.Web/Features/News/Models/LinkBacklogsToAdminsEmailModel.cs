using DntSite.Web.Features.Common.Models;

namespace DntSite.Web.Features.News.Models;

public class LinkBacklogsToAdminsEmailModel : BaseEmailModel
{
    public required IList<FeedItem> Items { get; set; }
}
