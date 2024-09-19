using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class ShowAdvertisementsArchiveList
{
    private string PageTitle => string.Create(CultureInfo.InvariantCulture, $"{MainTitle}، صفحه: {CurrentPage ?? 1}");

    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    [Parameter] [EditorRequired] public PagedResultModel<Advertisement>? Posts { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private static List<string> GetTags(Advertisement? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private bool CanUserEditThisPost(Advertisement post)
        => ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt);
}
