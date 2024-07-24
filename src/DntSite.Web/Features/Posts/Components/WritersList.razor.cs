using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Posts.Components;

public partial class WritersList
{
    [Parameter] [EditorRequired] public IList<(User User, int NumberOfPosts)>? Users { set; get; }

    private string PageTitle => Invariant($"{MainTitle}، صفحه: {CurrentPage ?? 1}");

    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    /// <summary>
    ///     Its default value is `TextBgColor.Secondary`
    /// </summary>
    [Parameter]
    public TextBgColor BadgeValueBgColor { set; get; } = TextBgColor.Secondary;

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [Parameter] [EditorRequired] public int TotalItemCount { set; get; }

    private string GetUrl(string name) => $"{BasePath.TrimEnd('/')}/{Uri.EscapeDataString(name)}";
}
