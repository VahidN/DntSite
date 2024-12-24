using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Posts.Components;

public partial class TagsList
{
    [Parameter] [EditorRequired] public bool ShowExportedFile { set; get; }

    [Parameter] public WhatsNewItemType? ItemType { get; set; }

    [Parameter] [EditorRequired] public IList<(string Name, int Id, int InUseCount)>? Tags { set; get; }

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
