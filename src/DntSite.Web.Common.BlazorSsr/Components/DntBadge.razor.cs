namespace DntSite.Web.Common.BlazorSsr.Components;

public partial class DntBadge
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?> AdditionalAttributes { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    [Parameter] public string? Title { set; get; }

    [Parameter] public object? BadgeValue { set; get; }

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
