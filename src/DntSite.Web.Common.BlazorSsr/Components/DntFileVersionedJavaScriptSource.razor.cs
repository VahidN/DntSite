namespace DntSite.Web.Common.BlazorSsr.Components;

/// <summary>
///     Provides version hash for a specified .js file.
/// </summary>
public partial class DntFileVersionedJavaScriptSource
{
    [CascadingParameter] public HttpContext HttpContext { set; get; } = null!;

    /// <summary>
    ///     The path of the .js file to which version should be added
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required string JsFilePath { set; get; }
}
