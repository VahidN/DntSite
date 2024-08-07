namespace DntSite.Web.Features.Common.Utils.WebToolkit;

public static class ImageMimeType
{
    public const string Png = "image/png";
    public const string Jpg = "image/jpeg";
    public const string Gif = "image/gif";

    public static string MimeType(this string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Jpg;
        }

        var ext = Path.GetExtension(fileName).ToUpperInvariant();

        return ext switch
        {
            ".PNG" => Png,
            ".GIF" => Gif,
            ".JPG" => Jpg,
            _ => Jpg
        };
    }

    public static bool IsImageFileUrl(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        string[] extensions = [".jpg", ".gif", ".png", ".bmp", ".jpeg"];
        var fileExt = Path.GetExtension(url);

        return extensions.Any(ext => string.Equals(fileExt, ext, StringComparison.OrdinalIgnoreCase));
    }
}
