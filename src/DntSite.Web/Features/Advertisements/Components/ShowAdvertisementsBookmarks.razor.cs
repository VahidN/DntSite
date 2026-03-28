using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;

namespace DntSite.Web.Features.Advertisements.Components;

[Authorize]
public partial class ShowAdvertisementsBookmarks
{
    private const int ItemsPerPage = 10;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        AddBreadCrumbs();
    }

    private bool CanUserEditThisPost(Advertisement post)
        => ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt);

    private static List<string> GetTags(Advertisement? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..AdvertisementsBreadCrumbs.DefaultBreadCrumbs]);
}
