using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.RssFeeds.Utils;

public static class WhatsNewItemTypeExtensions
{
    public static bool IsCommentOrReply(this WhatsNewItemType? whatsNewItemType)
    {
        if (whatsNewItemType is null)
        {
            return false;
        }

        return whatsNewItemType.Value.Contains(WhatsNewItemType.CommentsOf,
            StringComparison.CurrentCultureIgnoreCase) || whatsNewItemType.Value.Contains(WhatsNewItemType.RepliesOf,
            StringComparison.CurrentCultureIgnoreCase);
    }
}
