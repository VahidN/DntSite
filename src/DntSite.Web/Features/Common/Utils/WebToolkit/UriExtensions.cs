namespace DntSite.Web.Features.Common.Utils.WebToolkit;

public static class UriExtensions
{
    public static string? GetNormalizedPostUrl(this string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        if (!url.IsValidUrl())
        {
            return null;
        }

        if (!url.Contains(value: "/post/", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        var uri = new Uri(url);

        if (uri.Segments.Length < 2)
        {
            return url;
        }

        var id = uri.Segments[2].Replace(oldValue: "/", string.Empty, StringComparison.OrdinalIgnoreCase).ToInt();

        var domain = uri.IsDefaultPort
            ? uri.Host
            : string.Create(CultureInfo.InvariantCulture, $"{uri.Host}:{uri.Port}");

        return string.Create(CultureInfo.InvariantCulture, $"{uri.Scheme}://{domain}/post/{id}");
    }
}
