using System.Web;

namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntSocialLinks
{
    private string? _encodedHashtag;

    private string? _encodedPostUrl;

    [Inject] internal NavigationManager NavigationManager { set; get; } = null!;

    [Parameter] [EditorRequired] public required string Url { set; get; }

    [Parameter] [EditorRequired] public required string Title { set; get; }

    [Parameter] public string? HashTags { set; get; }

    private string EncodedTitle => Uri.EscapeDataString(Title);

    private string EncodedHashtag => _encodedHashtag ??= GetEncodedHashtag();

    private string EncodedPostUrl => _encodedPostUrl ??= GetEncodedPostUrl();

    private string GetEncodedPostUrl()
    {
        if (Uri.IsWellFormedUriString(Url, UriKind.Absolute))
        {
            return HttpUtility.UrlEncode(Url);
        }

        if (Uri.IsWellFormedUriString(Url, UriKind.Relative))
        {
            return HttpUtility.UrlEncode(NavigationManager.BaseUri.CombineUrl(Url, false));
        }

        return string.Empty;
    }

    private string GetEncodedHashtag()
        => string.IsNullOrWhiteSpace(HashTags)
            ? ""
            : Uri.EscapeDataString(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(HashTags).GetPostSlug()!);
}
