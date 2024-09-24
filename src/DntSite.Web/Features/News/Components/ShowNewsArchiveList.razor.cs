using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;

namespace DntSite.Web.Features.News.Components;

public partial class ShowNewsArchiveList
{
    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    private string PageTitle => string.Create(CultureInfo.InvariantCulture, $"{MainTitle}، صفحه: {CurrentPage ?? 1}");

    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    [Parameter] [EditorRequired] public PagedResultModel<DailyNewsItem>? Posts { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private static List<string> GetTags(DailyNewsItem? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private bool CanUserEditThisPost(DailyNewsItem post)
        => ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt);

    private string GetDeleteScreenshotUrl(DailyNewsItem post)
        => CanUserDeleteThisPost && !string.IsNullOrWhiteSpace(post.PageThumbnail)
            ? NewsRoutingConstants.WriteNewsDeleteDeleteScreenshotIdBase.CombineUrl(
                ProtectionProvider.Encrypt(post.Id.ToString(CultureInfo.InvariantCulture)))
            : "";
}
