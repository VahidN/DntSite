using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

[Authorize]
public partial class WriteProjectRelease
{
    [Parameter] public int? ProjectId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm(FormName = nameof(WriteProjectRelease))]
    public ProjectPostFileModel ProjectPostFileModel { get; set; } = new();

    [Parameter] public int? EditId { set; get; }

    [Parameter] public int? DeleteId { set; get; }

    [InjectComponentScoped] internal IProjectReleasesService ProjectReleasesService { set; get; } = null!;

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!ProjectId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        var project = await ProjectsService.FindProjectIncludeTagsAndUserAsync(ProjectId.Value);

        if (project is null || !ApplicationState.CanCurrentUserEditThisItem(project.UserId))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        AddBreadCrumbs(project.Title);

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        await PerformPossibleDeleteAsync();

        await FillPossibleEditFormAsync();
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (!DeleteId.HasValue)
        {
            return;
        }

        var projectRelease = await GetProjectReleaseAsync(DeleteId.Value);
        await ProjectReleasesService.MarkAsDeletedAsync(projectRelease);
        await ProjectReleasesService.NotifyDeleteChangesAsync(projectRelease, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{ProjectId}"));
    }

    private void AddBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);

    private async Task FillPossibleEditFormAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        var item = await GetProjectReleaseAsync(EditId.Value);

        if (item is null)
        {
            return;
        }

        ProjectPostFileModel = Mapper.Map<ProjectRelease, ProjectPostFileModel>(item);
    }

    private async Task<ProjectRelease?> GetProjectReleaseAsync(int id)
    {
        var project = await ProjectReleasesService.GetProjectReleaseAsync(id);

        if (project is null || !ApplicationState.CanCurrentUserEditThisItem(project.UserId, project.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return project;
    }

    private async Task PerformAsync()
    {
        if (!ProjectId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        var user = ApplicationState.CurrentUser?.User;

        ProjectRelease? projectRelease;

        if (EditId.HasValue)
        {
            projectRelease = await GetProjectReleaseAsync(EditId.Value);
            await ProjectReleasesService.UpdateProjectReleaseAsync(projectRelease, ProjectPostFileModel);
        }
        else
        {
            projectRelease =
                await ProjectReleasesService.AddProjectReleaseAsync(ProjectPostFileModel, user, ProjectId.Value);
        }

        await ProjectReleasesService.NotifyAddOrUpdateChangesAsync(projectRelease, ProjectPostFileModel, user);

        ApplicationState.NavigateTo(
            Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{ProjectId}/{projectRelease?.Id}"));
    }
}
