using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.WebToolkit;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.Models;
using DntSite.Web.Features.RoadMaps.ModelsMappings;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RoadMaps.Services.Contracts;

namespace DntSite.Web.Features.RoadMaps.Components;

public partial class LearningPathsArchiveDetails
{
    private string? _documentTypeIdHash;
    private LearningPathDetailsModel? _learningPath;

    [Parameter] public int? LearningPathId { set; get; }

    private LearningPath? CurrentPost => _learningPath?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Description ?? "").FirstOrDefault();

    [InjectComponentScoped] internal ILearningPathService LearningPathService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private bool CanUserEditThisPost
        => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId, CurrentPost?.Audit.CreatedAt);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (!LearningPathId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _learningPath = await LearningPathService.LearningPathDetailsAsync(LearningPathId.Value);

        if (!IsLearningPathPublic())
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        SetSimilarPostsId();

        await LearningPathService.UpdateStatAsync(LearningPathId.Value,
            ApplicationState.NavigationManager.IsFromFeed());
    }

    private void SetSimilarPostsId()
        => _documentTypeIdHash = _learningPath?.CurrentItem?.MapToWhatsNewItemModel(siteRootUri: "").DocumentTypeIdHash;

    private bool IsLearningPathPublic()
        => _learningPath?.CurrentItem is not null && !_learningPath.CurrentItem.IsDeleted;

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..RoadMapsBreadCrumbs.DefaultBreadCrumbs]);
}
