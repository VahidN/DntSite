using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Posts.Components;

[Authorize]
public partial class MyDrafts
{
    private IList<BlogPostDraft>? _blogPostDrafts;

    private CurrentUserModel? _currentUser;

    [InjectComponentScoped] internal IBlogPostDraftsService BlogPostDraftsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = ApplicationState.CurrentUser;

        _blogPostDrafts =
            await BlogPostDraftsService.FindUsersNotConvertedBlogPostDraftsAsync(_currentUser?.UserId ?? 0);

        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..PostsBreadCrumbs.DefaultBreadCrumbs]);
}
