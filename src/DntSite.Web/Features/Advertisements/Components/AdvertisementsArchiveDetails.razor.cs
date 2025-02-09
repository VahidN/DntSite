using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.Models;
using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Posts.Models;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class AdvertisementsArchiveDetails
{
    private List<AdvertisementComment>? _advertisementComments;

    private AdvertisementModel? _advertisements;

    [Parameter] public int? AdvertisementId { set; get; }

    private Advertisement? CurrentPost => _advertisements?.CurrentItem;

    private DateTime? ModifiedAt => CurrentPost?.AuditActions.Count > 0
        ? CurrentPost?.AuditActions[^1].CreatedAt
        : CurrentPost?.Audit.CreatedAt;

    private string? CurrentPostImageUrl
        => HtmlHelperService.ExtractImagesLinks(CurrentPost?.Body ?? "").FirstOrDefault();

    [InjectComponentScoped] internal IAdvertisementsService AdvertisementsService { set; get; } = null!;

    [InjectComponentScoped] internal IAdvertisementCommentsService AdvertisementCommentsService { set; get; } = null!;

    [Inject] internal IHtmlHelperService HtmlHelperService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public string? Slug { set; get; }

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private bool CanUserEditThisPost => ApplicationState.CanCurrentUserEditThisItem(CurrentPost?.UserId,
        CurrentPost?.Audit.CreatedAt);

    private List<string> GetTags() => CurrentPost?.Tags.Select(x => x.Name).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (!AdvertisementId.HasValue)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _advertisements = await AdvertisementsService.GetAdvertisementLastAndNextPostAsync(AdvertisementId.Value);

        if (_advertisements.CurrentItem is null)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await UpdateNumberOfAdvertisementViewsAsync();

        await GetCommentsAsync(AdvertisementId.Value);
    }

    private async Task UpdateNumberOfAdvertisementViewsAsync()
        => await AdvertisementsService.UpdateNumberOfAdvertisementViewsAsync(_advertisements?.CurrentItem,
            ApplicationState.NavigationManager.IsFromFeed());

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..AdvertisementsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task GetCommentsAsync(int id)
        => _advertisementComments = await AdvertisementCommentsService.GetRootCommentsOfAdvertisementAsync(id);

    private async Task HandleCommentActionAsync(CommentActionModel model)
    {
        switch (model.CommentAction)
        {
            case CommentAction.Delete:
                await AdvertisementCommentsService.DeleteCommentAsync(model.FormCommentId);

                break;
            case CommentAction.SubmitEditedComment:
                await AdvertisementCommentsService.EditReplyAsync(model.FormCommentId, model.Comment ?? "");

                break;
            case CommentAction.SubmitNewComment:
                await AdvertisementCommentsService.AddReplyAsync(model.FormCommentId, model.FormPostId,
                    model.Comment ?? "", ApplicationState.CurrentUser?.UserId ?? 0);

                break;
            case CommentAction.ReplyToComment:
            case CommentAction.Edit:
            case CommentAction.Cancel:
            default:
                return;
        }

        await GetCommentsAsync(model.FormPostId);
        StateHasChanged();
    }
}
