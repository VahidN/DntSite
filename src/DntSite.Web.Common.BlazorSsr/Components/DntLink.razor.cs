namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntLink
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    [Parameter] [EditorRequired] public RenderFragment? ChildContent { set; get; }

    [Parameter] public bool IsExternal { set; get; }

    [Parameter]
    public string CssClass { set; get; } =
        "link-offset-2 link-offset-3-hover link-underline link-underline-opacity-0 link-underline-opacity-75-hover";

    [Parameter] [EditorRequired] public required string Url { set; get; }
}
