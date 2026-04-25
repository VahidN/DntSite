namespace DntSite.Web.Features.Exports.Models;

public record EPubListItem(EPubContentItem Item, IList<EPubContentItem>? SubItems);
