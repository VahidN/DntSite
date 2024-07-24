using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

[Authorize]
public partial class WriteProject
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteProject))]
    public ProjectModel WriteProjectModel { get; set; } = new();

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public int? EditId { set; get; }

    [Parameter] public int? DeleteId { set; get; }

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!ApplicationState.CanCurrentUserCreateANewProject())
        {
            Alert.ShowAlert(AlertType.Danger, title: "عدم دسترسی",
                message: "برای ایجاد یک پروژه جدید نیاز است حداقل یک مطلب ارسالی در سایت داشته باشید.");

            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);
        AddBreadCrumbs();

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

        var project = await GetProjectAsync(DeleteId.Value);
        await ProjectsService.MarkAsDeletedAsync(project);
        await ProjectsService.NotifyDeleteChangesAsync(project, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(ProjectsRoutingConstants.Projects);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task FillPossibleEditFormAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        var item = await GetProjectAsync(EditId.Value);

        if (item is null)
        {
            return;
        }

        WriteProjectModel = Mapper.Map<Project, ProjectModel>(item);
    }

    private async Task<Project?> GetProjectAsync(int id)
    {
        var project = await ProjectsService.FindProjectIncludeTagsAndUserAsync(id);

        if (project is null || !ApplicationState.CanCurrentUserEditThisItem(project.UserId, project.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return project;
    }

    private async Task PerformAsync()
    {
        var user = ApplicationState.CurrentUser?.User;

        Project? project;

        if (EditId.HasValue)
        {
            project = await GetProjectAsync(EditId.Value);
            await ProjectsService.UpdateProjectAsync(project, WriteProjectModel);
        }
        else
        {
            project = await ProjectsService.AddProjectAsync(WriteProjectModel, user);
        }

        await ProjectsService.NotifyAddOrUpdateChangesAsync(project, WriteProjectModel, user);

        ApplicationState.NavigateTo(Invariant($"{ProjectsRoutingConstants.ProjectsDetailsBase}/{project?.Id}"));
    }
}
