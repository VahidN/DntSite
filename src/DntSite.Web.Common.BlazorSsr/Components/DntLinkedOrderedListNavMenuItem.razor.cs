namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntLinkedOrderedListNavMenuItem
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    [EditorRequired] [Parameter] public required string Title { set; get; }

    [Parameter] public bool IsTitleBold { set; get; }

    [Parameter] [EditorRequired] public required string Url { set; get; }

    [EditorRequired] [Parameter] public required string GlyphIcon { set; get; }

    [Parameter] public object? BadgeValue { set; get; }

    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool IsVisible { set; get; } = true;

    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool IsBadgeValueVisible { set; get; } = true;

    /// <summary>
    ///     Its default value is `TextBgColor.Secondary`
    /// </summary>
    [Parameter]
    public TextBgColor BadgeValueBgColor { set; get; } = TextBgColor.Secondary;
}
