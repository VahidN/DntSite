namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntSiteVerification
{
    /// <summary>
    ///     Go to the Google search console website and click “start now” to get your token.
    ///     https://search.google.com/search-console/about
    ///     The purpose of this verification is to know that you are the owner of this domain or any other third person.
    ///     Select the `URL prefix`, enter your site's address and then select `HTML tag` to get your token.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string? GoogleSiteVerificationToken { set; get; }
}
