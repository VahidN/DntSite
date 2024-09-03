using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Searches.Models;

public class LuceneSearchResult : WhatsNewItemModel
{
    public float Score { set; get; }

    public int LuceneDocId { init; get; }
}
