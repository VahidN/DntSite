using System.Text;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Utils;

namespace DntSite.Web.Features.Exports.ModelsMappings;

public static class ExportsMappersExtensions
{
    private static readonly CompositeFormat ParsedHtmlDocumentBodyTemplate = CompositeFormat.Parse(format: """
        <div style="page-break-inside:avoid;page-break-before:always; margin-bottom:5pt;"></div>
        <div>{0}</div>
        {1}
        <div style="page-break-inside:avoid;page-break-before:always;"></div>
        {2}
        """);

    private static readonly CompositeFormat ParsedPostBodyTemplate =
        CompositeFormat.Parse(format: "<div class='postBody'>{0}</div>");

    private static readonly CompositeFormat ParsedPostTitleTemplate = CompositeFormat.Parse(format: """
        <div class='main'>
         	<span class='titles' lang='fa'>عنوان: </span><h1>{0}</h1><br/>
        	<span class='titles' lang='fa'>نویسنده: </span>{1}<br/>
        	<span class='titles' lang='fa'>تاریخ: </span><span dir='ltr'>{2}</span><br/>
        	<span class='titles' lang='fa'>آدرس: </span>
        	<a dir='ltr' id='mainPostUrl' href='{3}' target='_blank'>{4}</a>
        </div>
        """);

    public static string ToHtmlDocumentBody(this ExportDocument doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        return string.Format(CultureInfo.InvariantCulture, ParsedHtmlDocumentBodyTemplate, doc.ToPostTitle(),
            doc.ToPostBody(), doc.ToCommentsTree());
    }

    private static string ToPostBody(this ExportDocument doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        return string.Format(CultureInfo.InvariantCulture, ParsedPostBodyTemplate, doc.Body);
    }

    private static string ToPostTitle(this ExportDocument doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        return string.IsNullOrWhiteSpace(doc.Title)
            ? string.Empty
            : string.Format(CultureInfo.InvariantCulture, ParsedPostTitleTemplate, doc.Title, doc.Author,
                doc.PersianDate.ToPersianNumbers(), doc.Url, new Uri(doc.Url).Host);
    }

    private static string ToCommentsTree(this ExportDocument doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        var (commentsHtml, numberOfComments) = new CommentsTreeView(doc.Comments).CommentsToHtml();

        if (string.IsNullOrWhiteSpace(commentsHtml) || numberOfComments == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        sb.AppendLine(value: "<div class='postBody'>");
        sb.AppendLine(value: "<h3>نظرات</h3>");
        sb.AppendLine(commentsHtml);
        sb.AppendLine(value: "</div>");

        return sb.ToString();
    }
}
