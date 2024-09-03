using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntRemoteAutoComplete
{
    [Inject] internal NavigationManager NavigationManager { set; get; } = null!;

    [Parameter] public int WidthPercent { set; get; } = 50;

    [Parameter] public string Placeholder { set; get; } = "جستجو ...";

    [Parameter] public string FieldIcon { set; get; } = DntBootstrapIcons.BiSearch;

    [Parameter] [EditorRequired] public required bool IsVisible { set; get; }

    [Parameter] [EditorRequired] public required string RemoteQueryApiUrl { set; get; }

    [Parameter] [EditorRequired] public required string RemoteLogApiUrl { set; get; }

    [Parameter] [EditorRequired] public required string RemoteQueryString { set; get; }

    private string GetAbsoluteApiUrl(string url)
        => Uri.TryCreate(url, UriKind.Absolute, out _) ? url : NavigationManager.ToAbsoluteUri(url).ToString();
}
