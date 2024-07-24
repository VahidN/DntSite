namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     Provides version hash for a specified .css file.
/// </summary>
public partial class DntFileVersionedCssLink
{
    [CascadingParameter] public HttpContext HttpContext { set; get; } = null!;

    /// <summary>
    ///     The path of the .css file to which version should be added
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string CssFilePath { set; get; }
}
