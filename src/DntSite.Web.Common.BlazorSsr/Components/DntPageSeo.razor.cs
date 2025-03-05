namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     A custom PageSeo component
/// </summary>
public partial class DntPageSeo
{
    /// <summary>
    ///     Title of the document
    /// </summary>
    [Parameter]
    public string? Title { set; get; }

    /// <summary>
    ///     Url of the largest image of the document
    /// </summary>
    [Parameter]
    public string? ImageUrl { set; get; }

    /// <summary>
    ///     Description of the largest image of the document
    /// </summary>
    [Parameter]
    public string? ImageDescription { set; get; }

    /// <summary>
    ///     Then mame of your site
    /// </summary>
    [Parameter]
    public string? SiteName { set; get; }

    /// <summary>
    ///     Description of the document
    /// </summary>
    [Parameter]
    public string? Description { set; get; }

    /// <summary>
    ///     The address of the sitemap
    /// </summary>
    [Parameter]
    public string? SiteMapUrl { set; get; }

    /// <summary>
    ///     The address of the rss-feed
    /// </summary>
    [Parameter]
    public string? RssUrl { set; get; }

    /// <summary>
    ///     https://llmstxt.org/
    /// </summary>
    public bool ShowLlmsTxt { set; get; } = true;

    /// <summary>
    ///     The name of the writer of this document
    /// </summary>
    [Parameter]
    public string? AuthorName { set; get; }

    /// <summary>
    ///     The average rating value of this article
    /// </summary>
    [Parameter]
    public decimal? AverageRating { set; get; }

    /// <summary>
    ///     How many users have voted for this article?
    /// </summary>
    [Parameter]
    public int? TotalRaters { set; get; }

    [Inject] internal NavigationManager NavigationManager { set; get; } = null!;

    private Uri CanonicalUrl => NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

    /// <summary>
    ///     Your Twitter Handle starting with @
    /// </summary>
    [Parameter]
    public string? YourTwitterHandle { set; get; }

    /// <summary>
    ///     The OpenSearchUrl
    /// </summary>
    [Parameter]
    public string? OpenSearchUrl { set; get; }

    /// <summary>
    ///     The publishing date of the article
    /// </summary>
    [Parameter]
    public DateTime? DatePublished { set; get; }

    /// <summary>
    ///     The modification date of the article
    /// </summary>
    [Parameter]
    public DateTime? DateModified { set; get; }

    /// <summary>
    ///     The tags of the article
    /// </summary>
    [Parameter]
    public IReadOnlyList<string> Tags { set; get; } = [];

    private string Keywords => Tags.Any()
        ? Tags.Aggregate((s1, s2) => string.Format(CultureInfo.InvariantCulture, format: "{0}, {1}", s1, s2))
        : "";

    private string? LastModified => DateModified?.ToUniversalTime().ToString(format: "R", CultureInfo.InvariantCulture);

    private string? PubDate => DatePublished?.ToUniversalTime().ToString(format: "R", CultureInfo.InvariantCulture);

    private bool HasRating => AverageRating is > 0 && TotalRaters is > 0;
}
