using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.Models;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectFaqDetails
{
    private FaqModel? _faqs;

    [Parameter] public int? ProjectId { set; get; }

    [Parameter] public int? FaqId { set; get; }

    private string PostUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.ProjectFaqsBase}/{ProjectId}/{FaqId}");

    private string EditPostUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.WriteProjectFaqEditBase}/{ProjectId}/{Uri.EscapeDataString(EncryptedFaqId)}");

    private string DeletePostUrlTemplate => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.WriteProjectFaqDeleteBase}/{ProjectId}/{Uri.EscapeDataString(EncryptedFaqId)}");

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    private string EncryptedFaqId => ProtectionProvider.Encrypt(FaqId?.ToString(CultureInfo.InvariantCulture)) ?? "";

    private ProjectFaq? CurrentPost => _faqs?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IProjectFaqsService ProjectFaqsService { set; get; } = null!;

    [InjectComponentScoped] internal IStatService StatService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private string LastPostUrl => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.ProjectFaqsBase}/{ProjectId}/{_faqs!.PreviousItem?.Id}");

    private string NextPostUrl => string.Create(CultureInfo.InvariantCulture,
        $"{ProjectsRoutingConstants.ProjectFaqsBase}/{ProjectId}/{_faqs!.NextItem?.Id}");

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        if (!ProjectId.HasValue || !FaqId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _faqs = await ProjectFaqsService.ShowFaqAsync(ProjectId.Value, FaqId.Value);

        if (_faqs.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();
        }

        AddBreadCrumbs(_faqs.CurrentItem?.Project.Title ?? "");

        await StatService.UpdateProjectNumberOfViewsOfFaqAsync(FaqId.Value,
            ApplicationState.NavigationManager.IsFromFeed());
    }

    private void AddBreadCrumbs(string name)
        => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultProjectBreadCrumbs(name, ProjectId)]);
}
