using System.Text;
using DntSite.Web.Features.Exports.Models;

namespace DntSite.Web.Features.Exports.Utils;

public class CommentsTreeView(IList<ExportComment>? listItems)
{
    private readonly StringBuilder _htmlBuilder = new();
    private int _numberOfComments;

    public (string HtmlDoc, int NumberOfComments) CommentsToHtml()
    {
        if (listItems is null || listItems.Count == 0)
        {
            return (string.Empty, 0);
        }

        _htmlBuilder.Append(value: "<ul>");

        foreach (var item in listItems.Where(x => !x.ReplyToId.HasValue))
        {
            BuildNestedTag(item);
        }

        _htmlBuilder.Append(value: "</ul>");

        return ($"<div id='userComments'>{_htmlBuilder}</div>", _numberOfComments);
    }

    private void AppendKids(ExportComment parentItem)
    {
        var kids = GetKids(parentItem);

        if (kids is null || kids.Count == 0)
        {
            return;
        }

        _htmlBuilder.Append(value: "<ul>");

        foreach (var kid in kids)
        {
            BuildNestedTag(kid);
        }

        _htmlBuilder.Append(value: "</ul>");
    }

    private List<ExportComment>? GetKids(ExportComment parentItem)
        => listItems?.Where(x => x.ReplyToId.HasValue && x.ReplyToId.Value == parentItem.Id).ToList();

    private void BuildNestedTag(ExportComment item)
    {
        _htmlBuilder.Append(value: "<li>");

        _htmlBuilder.AppendFormat(CultureInfo.InvariantCulture,
            format:
            "<div class='postBody'><div class='replyInfo'><strong>{0}</strong>  در  <span dir='ltr'>{1}</span></div>{2}</div>",
            item.Author, item.PersianDate.ToPersianNumbers(), item.Body);

        _numberOfComments++;
        AppendKids(item);
        _htmlBuilder.Append(value: "</li>");
    }
}
