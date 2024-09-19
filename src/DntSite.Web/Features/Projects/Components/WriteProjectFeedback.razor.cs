using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

[Authorize]
public partial class WriteProjectFeedback
{
    private Dictionary<string, int>? _issuePriorities;
    private Dictionary<string, int>? _issueTypes;

    [Parameter] public int? ProjectId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm(FormName = nameof(WriteProjectFeedback))]
    public IssueModel IssueModel { get; set; } = new();

    [Parameter] public string? EditId { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [InjectComponentScoped] internal IProjectIssuesService ProjectIssuesService { set; get; } = null!;

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [InjectComponentScoped] internal IIssuePrioritiesService IssuePrioritiesService { set; get; } = null!;

    [InjectComponentScoped] internal IIssueTypesService IssueTypesService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!ProjectId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        var project = await ProjectsService.FindProjectIncludeTagsAndUserAsync(ProjectId.Value);

        if (project is null)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        await InitDropDownsAsync();

        AddBreadCrumbs(project.Title);

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        await PerformPossibleDeleteAsync();

        await FillPossibleEditFormAsync();
    }

    private async Task InitDropDownsAsync()
    {
        _issueTypes = (await IssueTypesService.GetAllProjectIssueTypesListAsNoTrackingAsync(count: 2000)).Select(x
                => new
                {
                    x.Id,
                    Text = x.Name
                })
            .ToDictionary(x => x.Text, x => x.Id);

        _issuePriorities = (await IssuePrioritiesService.GetAllProjectIssuePrioritiesListAsNoTrackingAsync(count: 2000))
            .Select(x => new
            {
                x.Id,
                Text = x.Name
            })
            .ToDictionary(x => x.Text, x => x.Id);
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (string.IsNullOrWhiteSpace(DeleteId))
        {
            return;
        }

        var projectIssue = await GetProjectIssueAsync(DeleteId.ToInt());
        await ProjectIssuesService.MarkAsDeletedAsync(projectIssue);
        await ProjectIssuesService.NotifyDeleteChangesAsync(projectIssue, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{ProjectId}"));
    }

    private void AddBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);

    private async Task FillPossibleEditFormAsync()
    {
        if (string.IsNullOrWhiteSpace(EditId))
        {
            return;
        }

        var item = await GetProjectIssueAsync(EditId.ToInt());

        if (item is null)
        {
            return;
        }

        IssueModel = Mapper.Map<ProjectIssue, IssueModel>(item);
    }

    private async Task<ProjectIssue?> GetProjectIssueAsync(int id)
    {
        var projectIssue = await ProjectIssuesService.GetProjectIssueAsync(id);

        if (projectIssue is null || !ApplicationState.CanCurrentUserEditThisItem(projectIssue.UserId,
                projectIssue.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return projectIssue;
    }

    private async Task PerformAsync()
    {
        if (!ProjectId.HasValue)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        var user = ApplicationState.CurrentUser?.User;

        ProjectIssue? projectIssue;

        if (!string.IsNullOrWhiteSpace(EditId))
        {
            projectIssue = await GetProjectIssueAsync(EditId.ToInt());
            await ProjectIssuesService.UpdateProjectIssueAsync(projectIssue, IssueModel);
        }
        else
        {
            projectIssue = await ProjectIssuesService.AddProjectIssueAsync(IssueModel, user, ProjectId.Value);
        }

        await ProjectIssuesService.NotifyAddOrUpdateChangesAsync(projectIssue, IssueModel, user);

        ApplicationState.NavigateTo(string.Create(CultureInfo.InvariantCulture,
            $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{ProjectId}/{projectIssue?.Id}"));
    }
}
