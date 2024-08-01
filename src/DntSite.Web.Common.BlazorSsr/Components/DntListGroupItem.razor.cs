namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntListGroupItem
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    [Parameter] public string? Title { set; get; }

    [Parameter] public bool IsTitleBold { set; get; }

    [Parameter] public string? GlyphIcon { set; get; }

    [Parameter] public object? BadgeValue { set; get; }

    [Parameter] public RenderFragment? ChildContent { set; get; }

    /// <summary>
    ///     Its default value is true
    /// </summary>
    [Parameter]
    public bool LeftAlignChildContent { set; get; } = true;

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
