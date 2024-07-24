namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntPrintUserBrowser
{
    private bool IsEdge => BrowserName.Contains("edge", StringComparison.OrdinalIgnoreCase);

    private bool IsIE => BrowserName.Contains("msie", StringComparison.OrdinalIgnoreCase) ||
                         BrowserName.Contains("ie", StringComparison.OrdinalIgnoreCase) ||
                         BrowserName.Contains("internetexplorer", StringComparison.OrdinalIgnoreCase);

    private bool IsFirefox => BrowserName.Contains("firefox", StringComparison.OrdinalIgnoreCase);

    private bool IsChrome => BrowserName.Contains("chrome", StringComparison.OrdinalIgnoreCase);

    private bool IsSafari => BrowserName.Contains("safari", StringComparison.OrdinalIgnoreCase);

    private bool IsOpera => BrowserName.Contains("opera", StringComparison.OrdinalIgnoreCase);

    [Parameter] [EditorRequired] public required string BrowserName { set; get; }
}
