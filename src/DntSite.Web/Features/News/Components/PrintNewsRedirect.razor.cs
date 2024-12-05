using DntSite.Web.Features.News.RoutingConstants;

namespace DntSite.Web.Features.News.Components;

public partial class PrintNewsRedirect
{
    [Parameter] [EditorRequired] public int Id { get; set; }

    [Parameter] [EditorRequired] public HttpStatusCode? LastHttpStatusCode { get; set; }

    [Parameter] [EditorRequired] public DateTime? LastHttpStatusCodeCheckDateTime { get; set; }

    [Parameter] [EditorRequired] public string? Url { get; set; }

    private string StatusIcon => LastHttpStatusCode is HttpStatusCode.OK ? "bg-success" : "bg-warning";

    private string HttpStatusCodeText => LastHttpStatusCode is null
        ? "نامشخص"
        : string.Create(CultureInfo.InvariantCulture, $"{(int)LastHttpStatusCode.Value}, {LastHttpStatusCode.Value}");

    private string UrlHost => Url.IsValidUrl() ? new Uri(Url).Host : "";

    private string RedirectUrl
        => string.Create(CultureInfo.InvariantCulture, $"{NewsRoutingConstants.NewsRedirectBase}/{Id}");
}
