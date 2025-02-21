using System.Text;
using System.Web;
using DntSite.Web.Features.Exports.Models;

namespace DntSite.Web.Features.Exports.ModelsMappings;

public static class ExportsMappersExtensions
{
    private static readonly CompositeFormat ParsedHtmlDocumentBodyWithCommentsTemplate = CompositeFormat.Parse(
        format: """
                <div style="page-break-inside:avoid;page-break-before:always; margin-bottom:5pt;"></div>
                <div>{0}</div>
                {1}
                <div style="page-break-inside:avoid;page-break-before:always;"></div>
                {2}
                """);

    private static readonly CompositeFormat ParsedHtmlDocumentBodyWithoutCommentsTemplate = CompositeFormat.Parse(
        format: """
                <div style="page-break-inside:avoid;page-break-before:always; margin-bottom:5pt;"></div>
                <div>{0}</div>
                {1}
                """);

    private static readonly CompositeFormat ParsedPostBodyTemplate =
        CompositeFormat.Parse(format: "<div class='postBody'>{0}</div>");

    private static readonly CompositeFormat ParsedPostTitleTemplate = CompositeFormat.Parse(format: """
        <div class='main'>
         	<span class='titles' lang='fa'>عنوان: </span><h1 dir='{5}'>{0}</h1><br/>
        	<span class='titles' lang='fa'>نویسنده: </span>{1}<br/>
        	<span class='titles' lang='fa'>تاریخ: </span><span dir='ltr'>{2}</span><br/>
        	<span class='titles' lang='fa'>آدرس: </span>
        	<a dir='ltr' id='mainPostUrl' href='{3}' target='_blank'>{4}</a>
        </div>
        """);

    public static string ToHtmlDocumentBody(this ExportDocument doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        var postTitle = doc.ToPostTitle();
        var postBody = doc.ToPostBody();
        var commentsTree = doc.ToCommentsTree();

        return commentsTree.IsEmpty()
            ? string.Format(CultureInfo.InvariantCulture, ParsedHtmlDocumentBodyWithoutCommentsTemplate, postTitle,
                postBody, commentsTree)
            : string.Format(CultureInfo.InvariantCulture, ParsedHtmlDocumentBodyWithCommentsTemplate, postTitle,
                postBody, commentsTree);
    }

    public static string ToHtmlWithLocalImageUrls(this string? html,
        string articleImagesFolderPath,
        string courseImagesFolderPath,
        string newsImagesFolderPath)
    {
        if (html.IsEmpty())
        {
            return string.Empty;
        }

        return html.ReplaceImageUrlsWithNewImageUrls(imageUrl =>
        {
            if (!imageUrl.IsValidUrl())
            {
                return null;
            }

            var fileName = HttpUtility.ParseQueryString(new Uri(imageUrl).Query).Get(name: "name");

            if (fileName.IsEmpty())
            {
                return null;
            }

            var relativePath =
                GetImageRelativePath(pattern: "file/image?name=", imageUrl, articleImagesFolderPath, fileName);

            if (!relativePath.IsEmpty())
            {
                return relativePath;
            }

            relativePath = GetImageRelativePath(pattern: "file/courseimages?name=", imageUrl, courseImagesFolderPath,
                fileName);

            if (!relativePath.IsEmpty())
            {
                return relativePath;
            }

            relativePath = GetImageRelativePath(pattern: "file/NewsThumb?name=", imageUrl, newsImagesFolderPath,
                fileName);

            if (!relativePath.IsEmpty())
            {
                return relativePath;
            }

            return null;
        });
    }

    private static string? GetImageRelativePath(string pattern,
        string imageUrl,
        string imagesFolderPath,
        string fileName)
    {
        if (!imageUrl.Contains(pattern, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var path = Path.Combine(imagesFolderPath, fileName);

        if (!path.FileExists())
        {
            return null;
        }

        var pathParts = imagesFolderPath.Split(Path.DirectorySeparatorChar);

        return $"../../{pathParts[^2]}/{pathParts[^1]}/{fileName}";
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
            : string.Format(CultureInfo.InvariantCulture, ParsedPostTitleTemplate, doc.Title.ApplyRle(), doc.Author,
                doc.PersianDate.ToPersianNumbers(), doc.Url, new Uri(doc.Url).Host, doc.Title.GetDir());
    }

    private static string ToCommentsTree(this ExportDocument doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        var (commentsHtml, numberOfComments) = new HtmlTreeViewBuilder<ExportComment, int?>
        {
            Items = doc.Comments,
            OuterDivTemplate = data => $"<div id='userComments'>{data}</div>",
            TreeItemBodyTemplate = comment => string.Format(CultureInfo.InvariantCulture, format: """
                    <div class='postBody'>
                      <div class='replyInfo'>
                           <strong>{0}</strong>  در  <span dir='ltr'>{1}</span>
                      </div>
                      {2}
                    </div>
                    """, comment.Author, comment.PersianDate.ToPersianNumbers(),
                comment.Body.WrapInDirectionalDiv(fontFamily: "inherit", fontSize: "inherit"))
        }.ItemsToHtml();

        if (string.IsNullOrWhiteSpace(commentsHtml) || numberOfComments == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        sb.AppendLine(value: "<div class='postBody mt5'>");
        sb.AppendLine(value: "<h3>نظرات</h3>");
        sb.AppendLine(commentsHtml);
        sb.AppendLine(value: "</div>");

        return sb.ToString();
    }
}
