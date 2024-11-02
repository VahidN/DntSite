using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectsArchiveDetails
{
    private ProjectsModel? _projects;

    [Parameter] public int? ProjectId { set; get; }

    private Project? CurrentPost => _projects?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        if (!ProjectId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _projects = await ProjectsService.GetProjectsLastAndNextAsync(ProjectId.Value);

        if (_projects.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();
        }

        AddBreadCrumbs(_projects.CurrentItem?.Title ?? "");
    }

    private void AddBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);
}
