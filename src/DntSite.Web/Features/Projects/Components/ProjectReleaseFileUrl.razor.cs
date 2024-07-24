using DntSite.Web.Features.Common.RoutingConstants;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectReleaseFileUrl
{
    private string FormName => Invariant($"ProjectReleaseFileUrl_{ProjectRelease?.Id}");

    [Parameter] public ProjectRelease? ProjectRelease { get; set; }

    [InjectComponentScoped] internal IStatService StatService { set; get; } = null!;

    [Inject] internal NavigationManager NavigationManager { set; get; } = null!;

    private async Task PerformAsync()
    {
        if (ProjectRelease is null)
        {
            return;
        }

        await StatService.UpdateProjectFileNumberOfDownloadsAsync(ProjectRelease.Id);

        var redirectUrl =
            $"{ApiUrlsRoutingConstants.File.HttpAny.ProjectFile}?name={Uri.EscapeDataString(ProjectRelease.FileName)}";

        NavigationManager.NavigateTo(redirectUrl, forceLoad: true);
    }
}
