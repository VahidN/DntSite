namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntPrintNoScript
{
    [Inject] internal IBlazorRenderingContext BlazorRenderingContext { set; get; } = null!;

    /// <summary>
    ///     Its default value is `/NoScript.html`
    /// </summary>
    [Parameter]
    public string RedirectUrlOnError { set; get; } = "/NoScript.html";
}
