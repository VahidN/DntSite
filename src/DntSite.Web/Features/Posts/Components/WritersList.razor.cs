using DntSite.Web.Features.UserProfiles.Entities;

namespace DntSite.Web.Features.Posts.Components;

public partial class WritersList
{
    [Parameter] [EditorRequired] public IList<(User User, string BadgeValue)>? Users { set; get; }

    private string PageTitle => string.Create(CultureInfo.InvariantCulture, $"{MainTitle}، صفحه: {CurrentPage ?? 1}");

    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    /// <summary>
    ///     Its default value is `TextBgColor.Secondary`
    /// </summary>
    [Parameter]
    public TextBgColor BadgeValueBgColor { set; get; } = TextBgColor.Secondary;

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [Parameter] public string? BaseUsersPath { set; get; }

    [Parameter] [EditorRequired] public int TotalItemCount { set; get; }

    private string GetUrl(string name)
        => string.IsNullOrWhiteSpace(BaseUsersPath)
            ? BasePath.CombineUrl(name, escapeRelativeUrl: true)
            : BaseUsersPath.CombineUrl(name, escapeRelativeUrl: true);
}
