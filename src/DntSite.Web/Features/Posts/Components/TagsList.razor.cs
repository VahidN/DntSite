using DntSite.Web.Common.BlazorSsr.Utils;

namespace DntSite.Web.Features.Posts.Components;

public partial class TagsList
{
    [Parameter] [EditorRequired] public IList<(string Name, int InUseCount)>? Tags { set; get; }

    private string PageTitle => string.Create(CultureInfo.InvariantCulture, $"{MainTitle}، صفحه: {CurrentPage ?? 1}");

    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    [Parameter] public string? GlyphIcon { set; get; } = DntBootstrapIcons.BiTag;

    /// <summary>
    ///     Its default value is `TextBgColor.Secondary`
    /// </summary>
    [Parameter]
    public TextBgColor BadgeValueBgColor { set; get; } = TextBgColor.Secondary;

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [Parameter] [EditorRequired] public int TotalItemCount { set; get; }

    private string GetUrl(string tagName) => $"{BasePath.TrimEnd(trimChar: '/')}/{Uri.EscapeDataString(tagName)}";
}
