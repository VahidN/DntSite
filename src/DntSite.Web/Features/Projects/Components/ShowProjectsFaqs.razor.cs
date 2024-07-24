using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ShowProjectsFaqs
{
    private const int PostItemsPerPage = 10;

    private string _pageTitle = "راهنماهای پروژه‌‌ها";

    private PagedResultModel<ProjectFaq>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IProjectFaqsService ProjectFaqsService { set; get; } = null!;

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public int? ProjectId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private string GetBasePath()
    {
        if (HasUserFriendlyName)
        {
            return $"{ProjectsRoutingConstants.ProjectsFaqs}/{Uri.EscapeDataString(UserFriendlyName)}";
        }

        return ProjectId.HasValue
            ? Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{ProjectId.Value}")
            : ProjectsRoutingConstants.ProjectsFaqs;
    }

    private async Task<string> GetMainTitleAsync()
    {
        if (HasUserFriendlyName)
        {
            return $@"آرشیو راهنماهای پروژه‌های {UserFriendlyName}";
        }

        if (ProjectId.HasValue)
        {
            var title = await GetProjectTitleAsync();

            return $"راهنماهای پروژه {title}";
        }

        return "راهنماهای پروژه‌ها";
    }

    private async Task<string?> GetProjectTitleAsync()
        => ProjectId.HasValue ? (await ProjectsService.FindProjectAsync(ProjectId.Value))?.Title : "";

    protected override async Task OnInitializedAsync()
    {
        _pageTitle = await GetMainTitleAsync();

        if (HasUserFriendlyName)
        {
            await ShowUserFaqsAsync();
        }
        else if (ProjectId.HasValue)
        {
            await ShowProjectFaqsListAsync(ProjectId.Value);
        }
        else
        {
            await ShowAllFaqsListAsync();
        }
    }

    private async Task ShowProjectFaqsListAsync(int projectId)
    {
        CurrentPage ??= 1;

        _posts = await ProjectFaqsService.GetLastPagedProjectFaqsAsync(projectId, CurrentPage.Value - 1);

        AddProjectBreadCrumbs(await GetProjectTitleAsync() ?? "");
    }

    private void AddProjectBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);

    private async Task ShowAllFaqsListAsync()
    {
        CurrentPage ??= 1;

        _posts = await ProjectFaqsService.GetLastPagedAllProjectsFaqsAsNoTrackingAsync(CurrentPage.Value - 1,
            PostItemsPerPage);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task ShowUserFaqsAsync()
    {
        CurrentPage ??= 1;

        _posts = await ProjectFaqsService.GetLastPagedProjectFaqsOfUserAsync(UserFriendlyName!, CurrentPage.Value - 1,
            PostItemsPerPage);

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

    private string GetPostAbsoluteUrl(ProjectFaq faq)
        => Invariant($"{ProjectsRoutingConstants.ProjectFaqsBase}/{faq.ProjectId}/{faq.Id}");
}
