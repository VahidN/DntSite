using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Posts.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class ShowAllDrafts
{
    private IList<BlogPostDraft>? _blogPostDrafts;
    private CurrentUserModel? _currentUser;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [InjectComponentScoped] internal IBlogPostDraftsService BlogPostDraftsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = ApplicationState.CurrentUser;
        _blogPostDrafts = await BlogPostDraftsService.FindAllNotConvertedBlogPostDraftsAsync();
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange(
            [..PostsBreadCrumbs.DefaultBreadCrumbs, PostsBreadCrumbs.AllDraftsList]);
}
