using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Backlogs.ModelsMappings;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;

namespace DntSite.Web.Features.Backlogs.Components;

public partial class BacklogsArchiveDetails
{
    private BacklogDetailsModel? _backlogs;

    private string? _documentTypeIdHash;

    [Parameter] public int? BacklogId { set; get; }

    private Backlog? CurrentPost => _backlogs?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IBacklogsService BacklogsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private ManageBacklogModel BacklogStatModel => new()
    {
        ConvertedBlogPostId = CurrentPost?.ConvertedBlogPostId,
        DaysEstimate = CurrentPost?.DaysEstimate,
        TakenByUser = CurrentPost?.DoneByUser,
        Id = CurrentPost?.Id ?? 0,
        DoneDate = CurrentPost?.DoneDate,
        StartDate = CurrentPost?.StartDate
    };

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (!BacklogId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _backlogs = await BacklogsService.BacklogDetailsAsync(BacklogId.Value);

        if (_backlogs.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();
        }

        await BacklogsService.UpdateStatAsync(BacklogId.Value, ApplicationState.NavigationManager.IsFromFeed());

        SetSimilarPostsId();
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _backlogs?.CurrentItem
            ?.MapToWhatsNewItemModel(siteRootUri: "", showBriefDescription: false)
            .DocumentTypeIdHash;

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..BacklogsBreadCrumbs.DefaultBreadCrumbs]);
}
