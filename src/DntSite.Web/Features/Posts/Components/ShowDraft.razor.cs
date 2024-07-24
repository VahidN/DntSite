using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Posts.Components;

public partial class ShowDraft
{
    private IList<BlogPostDraft>? _blogPostDrafts;

    private CurrentUserModel? _currentUser;

    [Parameter] public int? ShowId { set; get; }

    private string ShowDraftUrl => Invariant($"{PostsRoutingConstants.ShowDraftBase}/{ShowId}");

    [InjectComponentScoped] internal IBlogPostDraftsService BlogPostDraftsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!ShowId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _currentUser = ApplicationState.CurrentUser;

        var draft = await BlogPostDraftsService.FindBlogPostDraftAsync(ShowId.Value);

        if (!_currentUser.CanUserEditThisDraft(draft))
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _blogPostDrafts = [draft];

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..PostsBreadCrumbs.DefaultBreadCrumbs]);
}
