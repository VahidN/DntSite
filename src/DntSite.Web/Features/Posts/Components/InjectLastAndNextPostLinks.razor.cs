namespace DntSite.Web.Features.Posts.Components;

public partial class InjectLastAndNextPostLinks
{
    private string NextPostItemUrl => !string.IsNullOrWhiteSpace(PostUrlTemplate)
        ? string.Format(CultureInfo.InvariantCulture, PostUrlTemplate, NextPostId)
        : NextPostUrl ?? "";

    private string LastPostItemUrl => !string.IsNullOrWhiteSpace(PostUrlTemplate)
        ? string.Format(CultureInfo.InvariantCulture, PostUrlTemplate, LastPostId)
        : LastPostUrl ?? "";

    [Parameter] public string? NextPostUrl { set; get; }

    [Parameter] public string? LastPostUrl { set; get; }

    [Parameter] public string? PostUrlTemplate { set; get; }

    [Parameter] public int? NextPostId { set; get; }

    [Parameter] [EditorRequired] public string? NextPostTitle { set; get; }

    [Parameter] public int? LastPostId { set; get; }

    [Parameter] [EditorRequired] public string? LastPostTitle { set; get; }
}
