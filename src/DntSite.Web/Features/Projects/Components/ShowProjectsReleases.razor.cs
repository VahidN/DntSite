using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ShowProjectsReleases
{
    private const int PostItemsPerPage = 10;

    private string _pageTitle = "فایل‌های پروژه‌‌ها";

    private PagedResultModel<ProjectRelease>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IProjectReleasesService ProjectReleasesService { set; get; } = null!;

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public int? ProjectId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private string GetBasePath()
    {
        if (HasUserFriendlyName)
        {
            return $"{ProjectsRoutingConstants.ProjectsReleases}/{Uri.EscapeDataString(UserFriendlyName)}";
        }

        return ProjectId.HasValue
            ? Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{ProjectId.Value}")
            : ProjectsRoutingConstants.ProjectsReleases;
    }

    private async Task<string> GetMainTitleAsync()
    {
        if (HasUserFriendlyName)
        {
            return $@"آرشیو فایل‌های پروژه‌های {UserFriendlyName}";
        }

        if (ProjectId.HasValue)
        {
            var title = await GetProjectTitleAsync();

            return $"فایل‌های پروژه {title}";
        }

        return "فایل‌های پروژه‌ها";
    }

    private async Task<string?> GetProjectTitleAsync()
        => ProjectId.HasValue ? (await ProjectsService.FindProjectAsync(ProjectId.Value))?.Title : "";

    protected override async Task OnInitializedAsync()
    {
        _pageTitle = await GetMainTitleAsync();

        if (HasUserFriendlyName)
        {
            await ShowUserProjectReleasesAsync();
        }
        else if (ProjectId.HasValue)
        {
            await ShowProjectReleasesListAsync(ProjectId.Value);
        }
        else
        {
            await ShowAllProjectReleasesListAsync();
        }
    }

    private async Task ShowProjectReleasesListAsync(int projectId)
    {
        CurrentPage ??= 1;

        _posts = await ProjectReleasesService.GetAllProjectReleasesAsync(projectId, CurrentPage.Value - 1,
            PostItemsPerPage);

        AddProjectBreadCrumbs(await GetProjectTitleAsync() ?? "");
    }

    private void AddProjectBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);

    private async Task ShowAllProjectReleasesListAsync()
    {
        CurrentPage ??= 1;

        _posts = await ProjectReleasesService.GetAllProjectsReleasesIncludeProjectsAsync(CurrentPage.Value - 1,
            PostItemsPerPage);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task ShowUserProjectReleasesAsync()
    {
        CurrentPage ??= 1;

        _posts = await ProjectReleasesService.GetLastPagedProjectReleasesOfUserAsync(UserFriendlyName!,
            CurrentPage.Value - 1, PostItemsPerPage);

        AddUserCommentsBreadCrumbs();
    }

    private void AddUserCommentsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..ProjectsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = _pageTitle,
                Url = GetBasePath(),
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private string GetPostAbsoluteUrl(ProjectRelease projectRelease)
        => Invariant($"{ProjectsRoutingConstants.ProjectReleasesBase}/{projectRelease.ProjectId}/{projectRelease.Id}");
}
