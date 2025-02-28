using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ShowProjectsComments
{
    private const int PostItemsPerPage = 10;

    private string _pageTitle = "نظرات پروژه‌‌ها";

    private PagedResultModel<ProjectIssueComment>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IProjectIssueCommentsService ProjectIssueCommentsService { set; get; } = null!;

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public int? ProjectId { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private string GetBasePath()
    {
        if (HasUserFriendlyName)
        {
            return $"{ProjectsRoutingConstants.ProjectsComments}/{Uri.EscapeDataString(UserFriendlyName)}";
        }

        return ProjectId.HasValue
            ? string.Create(CultureInfo.InvariantCulture,
                $"{ProjectsRoutingConstants.ProjectCommentsBase}/{ProjectId.Value}")
            : ProjectsRoutingConstants.ProjectsComments;
    }

    private async Task<string> GetMainTitleAsync()
    {
        if (HasUserFriendlyName)
        {
            return $@"آرشیو نظرات پروژه‌های {UserFriendlyName}";
        }

        if (ProjectId.HasValue)
        {
            var title = await GetProjectTitleAsync();

            return $"نظرات پروژه {title}";
        }

        return "نظرات پروژه‌ها";
    }

    private async Task<string?> GetProjectTitleAsync()
        => ProjectId.HasValue ? (await ProjectsService.FindProjectAsync(ProjectId.Value))?.Title : "";

    protected override async Task OnInitializedAsync()
    {
        _pageTitle = await GetMainTitleAsync();

        if (HasUserFriendlyName)
        {
            await ShowUserCommentsAsync();
        }
        else if (ProjectId.HasValue)
        {
            await ShowProjectCommentsListAsync(ProjectId.Value);
        }
        else
        {
            await ShowAllCommentsListAsync();
        }
    }

    private async Task ShowProjectCommentsListAsync(int projectId)
    {
        CurrentPage ??= 1;

        _posts = await ProjectIssueCommentsService.GetPagedLastProjectIssueCommentsIncludeBlogPostAndUserAsync(
            projectId, CurrentPage.Value - 1, PostItemsPerPage);

        AddProjectBreadCrumbs(await GetProjectTitleAsync() ?? "");
    }

    private void AddProjectBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);

    private async Task ShowAllCommentsListAsync()
    {
        CurrentPage ??= 1;

        _posts = await ProjectIssueCommentsService.GetLastPagedIssueCommentsAsNoTrackingAsync(CurrentPage.Value - 1,
            PostItemsPerPage);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task ShowUserCommentsAsync()
    {
        CurrentPage ??= 1;

        _posts = await ProjectIssueCommentsService.GetLastPagedProjectIssuesCommentsAsNoTrackingAsync(UserFriendlyName!,
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

    private static string GetPostAbsoluteUrl(ProjectIssueComment issueComment)
        => string.Create(CultureInfo.InvariantCulture,
            $"{ProjectsRoutingConstants.ProjectFeedbacksBase}/{issueComment.Parent.ProjectId}/{issueComment.ParentId}");
}
