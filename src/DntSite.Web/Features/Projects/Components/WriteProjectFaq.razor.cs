using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

[Authorize]
public partial class WriteProjectFaq
{
    [Parameter] public int? ProjectId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm(FormName = nameof(WriteProjectFaq))]
    public ProjectFaqFormModel WriteProjectFaqFormModel { get; set; } = new();

    [Parameter] public int? EditId { set; get; }

    [Parameter] public int? DeleteId { set; get; }

    [InjectComponentScoped] internal IProjectFaqsService ProjectFaqsService { set; get; } = null!;

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

        var projectFaq = await GetProjectFaqAsync(DeleteId.Value);
        await ProjectFaqsService.MarkAsDeletedAsync(projectFaq);
        await ProjectFaqsService.NotifyDeleteChangesAsync(projectFaq, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{ProjectId}"));
    }

    private void AddBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);

    private async Task FillPossibleEditFormAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        var item = await GetProjectFaqAsync(EditId.Value);

        if (item is null)
        {
            return;
        }

        WriteProjectFaqFormModel = Mapper.Map<ProjectFaq, ProjectFaqFormModel>(item);
    }

    private async Task<ProjectFaq?> GetProjectFaqAsync(int id)
    {
        var projectFaq = await ProjectFaqsService.GetProjectFaqAsync(id);

        if (projectFaq is null ||
            !ApplicationState.CanCurrentUserEditThisItem(projectFaq.UserId, projectFaq.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return projectFaq;
    }

    private async Task PerformAsync()
    {
        if (!ProjectId.HasValue)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        var user = ApplicationState.CurrentUser?.User;

        ProjectFaq? projectFaq;

        if (EditId.HasValue)
        {
            projectFaq = await GetProjectFaqAsync(EditId.Value);
            await ProjectFaqsService.UpdateProjectFaqAsync(projectFaq, WriteProjectFaqFormModel);
        }
        else
        {
            projectFaq = await ProjectFaqsService.AddProjectFaqAsync(WriteProjectFaqFormModel, user, ProjectId.Value);
        }

        await ProjectFaqsService.NotifyAddOrUpdateChangesAsync(projectFaq, WriteProjectFaqFormModel, user);

        ApplicationState.NavigateTo(
            Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{ProjectId}/{projectFaq?.Id}"));
    }
}
