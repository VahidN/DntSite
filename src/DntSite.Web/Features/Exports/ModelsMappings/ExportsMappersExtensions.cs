using System.Text;
using System.Web;
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

    public static string ToHtmlWithLocalImageUrls(this string? html,
        string articleImagesFolderPath,
        string courseImagesFolderPath)
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
